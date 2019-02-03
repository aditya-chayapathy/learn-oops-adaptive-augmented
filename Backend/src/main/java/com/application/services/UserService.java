package com.application.services;

import com.application.pojo.User;
import com.application.pojo.UserLogs;
import com.application.repositories.UserLogsRepository;
import com.application.repositories.UserRepository;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Date;
import java.util.List;
import java.util.logging.Logger;

@Service
public class UserService {

    private static final Logger log = Logger.getLogger(UserService.class.getName());

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private UserLogsRepository userLogsRepository;

    public User addUser(String userId, String password, Integer levelOfExpertise) {
        Integer defaultSpeed = 10;
        User user = new User(userId, password, levelOfExpertise, defaultSpeed);
        userRepository.save(user);

        return user;
    }

    public User getUser(String userId) {
        User user = userRepository.findByUserId(userId);

        return user;
    }

    public Boolean checkIfValidCredentials(String userId, String password) {
        User user = userRepository.findByUserIdAndPassword(userId, password);
        if (user == null) {
            return Boolean.FALSE;
        } else {
            return Boolean.TRUE;
        }
    }

    public UserLogs addUserLog(Integer speed, Integer numberOfWallCollisions, Integer taskNumber, String userId, Boolean isSuccess, Long timeTaken) {
        String stringIsSuccess = isSuccess ? "TRUE" : "FALSE";
        Date timestamp = new Date();

        UserLogs log = new UserLogs(speed, numberOfWallCollisions, taskNumber, userId, stringIsSuccess, timeTaken, timestamp, "FALSE");
        userLogsRepository.save(log);

        return log;
    }

    public List<UserLogs> getUserLogs(String userId) {
        List<UserLogs> logs = userLogsRepository.findByUserId(userId);

        return logs;
    }

    public JSONObject getGameSettingsForUser(String userId) {
        User user = userRepository.findByUserId(userId);

        JSONObject result = new JSONObject();
        result.put("speed", user.getSpeed());
        result.put("taskNumber", getNextTaskNumberForUser(userId));

        return result;
    }

    private Integer getNextTaskNumberForUser(String userId) {
        UserLogs latestSuccessfulLog = userLogsRepository.findTop1ByUserIdAndIsSuccessOrderByTimestampDesc(userId, "TRUE");
        if (latestSuccessfulLog == null) {
            return 1;
        }

        if (latestSuccessfulLog.getTaskNumber() == 4) {
            return 4;
        }

        return latestSuccessfulLog.getTaskNumber() + 1;

    }

    public JSONObject getEndOfSceneStats(String userId, Integer taskNumber) {
        JSONObject jsonObject = new JSONObject();

        jsonObject.put("averageGlobalSuccessfulCompletionTime", getAverageGlobalSuccessfulCompletionTime(taskNumber));
        jsonObject.put("averageUserSuccessfulCompletionTime", getAverageUserSuccessfulCompletionTime(taskNumber, userId));
        jsonObject.put("previousCompletionTime", getPreviousCompletionTime(taskNumber, userId));
        jsonObject.put("previousSuccessfulCompletionTime", getPreviousSuccessfulCompletionTime(taskNumber, userId));

        return jsonObject;
    }

    private Long getAverageGlobalSuccessfulCompletionTime(Integer taskNumber) {
        List<UserLogs> logs = userLogsRepository.findByTaskNumberAndIsSuccess(taskNumber, "TRUE");
        if (logs == null || logs.size() == 0) {
        	return 0L;
        }

        Long sum = logs.stream().mapToLong(i -> i.getTimeTaken()).sum();

        return sum / logs.size();
    }

    private Long getAverageUserSuccessfulCompletionTime(Integer taskNumber, String userId) {
        List<UserLogs> logs = userLogsRepository.findByUserIdAndTaskNumberAndIsSuccess(userId, taskNumber, "TRUE");
        if (logs == null || logs.size() == 0) {
        	return 0L;
        }

        Long sum = logs.stream().mapToLong(i -> i.getTimeTaken()).sum();

        return sum / logs.size();
    }

    private Long getPreviousCompletionTime(Integer taskNumber, String userId) {
        UserLogs log = userLogsRepository.findTop1ByUserIdAndTaskNumberOrderByTimestampDesc(userId, taskNumber);
        if (log == null) {
            return 0L;
        }

        return log.getTimeTaken();
    }

    private Long getPreviousSuccessfulCompletionTime(Integer taskNumber, String userId) {
        UserLogs log = userLogsRepository.findTop1ByUserIdAndTaskNumberAndIsSuccessOrderByTimestampDesc(userId, taskNumber, "TRUE");
        if (log == null) {
            return 0L;
        }

        return log.getTimeTaken();
    }

    public void updateSpeedForAllUsers() {
        log.info("Updating speed for all users");
        List<User> users = userRepository.findAll();
        for (User user : users) {
            updateSpeedForUser(user);
        }
    }

    private void updateSpeedForUser(User user) {
        List<UserLogs> logs = userLogsRepository.findByUserIdAndProcessed(user.getUserId(), "FALSE");
        Integer currentSpeed = user.getSpeed();
        Double updatedSpeed = Double.valueOf(currentSpeed);

        for (UserLogs log : logs) {
            Long globalTimeTaken = getAverageGlobalSuccessfulCompletionTime(log.getTaskNumber());
            long difference = globalTimeTaken - log.getTimeTaken();

            updatedSpeed += (difference * 0.05);

            log.setProcessed("TRUE");
            userLogsRepository.save(log);
        }

        if (updatedSpeed < 6) {
            updatedSpeed = 6.0;
        } else if (updatedSpeed > 14) {
            updatedSpeed = 14.0;
        }

        user.setSpeed(updatedSpeed.intValue());
        userRepository.save(user);
    }

    public void updateLevelOfExpertiseForAllUsers() {
        log.info("Updating level of expertise for all users");
        List<User> users = userRepository.findAll();
        for (User user : users) {
            updateLevelOfExpertiseForUser(user);
        }
    }

    private void updateLevelOfExpertiseForUser(User user) {
        Integer levelOfExpertise;
        UserLogs latestSuccessfulLog = userLogsRepository.findTop1ByUserIdAndIsSuccessOrderByTimestampDesc(user.getUserId(), "TRUE");
        if (latestSuccessfulLog == null) {
            levelOfExpertise = 1;
        } else {
            levelOfExpertise = latestSuccessfulLog.getTaskNumber();
        }

        user.setLevelOfExpertise(levelOfExpertise);
        userRepository.save(user);
    }

    public void setSpeedForUser(String userId, Integer timeTaken) {
        User user = userRepository.findByUserId(userId);
        Long globalAverageTimeTaken = getAverageGlobalSuccessfulCompletionTime(0);
        Long difference = globalAverageTimeTaken - timeTaken;
        Double speed = 10 + (difference * 0.05);

        if (speed < 6) {
            speed = 6.0;
        } else if (speed > 14) {
            speed = 14.0;
        }

        user.setSpeed(speed.intValue());
        userRepository.save(user);
    }

}
