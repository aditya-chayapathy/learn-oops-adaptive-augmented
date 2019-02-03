package com.application.controller;

import com.application.pojo.User;
import com.application.pojo.UserLogs;
import com.application.scheduler.CronJobScheduler;
import com.application.services.UserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/user")
public class UserController {
	
	private static final Logger LOGGER = LoggerFactory.getLogger(UserController.class);

    @Autowired
    private UserService userService;

    @Autowired
    private CronJobScheduler cronJobScheduler;

    @RequestMapping(value = "/test", method = RequestMethod.GET)
    public String test() {
        return "Test";
    }

    @RequestMapping(value = "/addUser", method = RequestMethod.POST)
    public User addUser(@RequestParam String userId,
                        @RequestParam String password,
                        @RequestParam Integer levelOfExpertise) {
        return userService.addUser(userId, password, levelOfExpertise);
    }

    @RequestMapping(value = "/{userId}", method = RequestMethod.GET)
    public User getUser(@PathVariable("userId") String userId) {
        return userService.getUser(userId);
    }

    @RequestMapping(value = "/checkIfValidCredentials", method = RequestMethod.GET)
    public Boolean checkIfValidCredentials(@RequestParam String userId,
                                           @RequestParam String password) {
    	LOGGER.info("checking if " + userId + " is a valid user!");
        return userService.checkIfValidCredentials(userId, password);
    }

    @RequestMapping(value = "/logs/addUserLog", method = RequestMethod.POST)
    public UserLogs addUserLog(@RequestParam Integer speed,
                               @RequestParam Integer numberOfWallCollisions,
                               @RequestParam Integer taskNumber,
                               @RequestParam String userId,
                               @RequestParam Boolean isSuccess,
                               @RequestParam Long timeTaken) {
    	LOGGER.info("Adding logs after game end!");
        return userService.addUserLog(speed, numberOfWallCollisions, taskNumber, userId, isSuccess, timeTaken);
    }

    @RequestMapping(value = "/logs/{userId}", method = RequestMethod.GET)
    public List<UserLogs> getUserLogs(@PathVariable("userId") String userId) {
        return userService.getUserLogs(userId);
    }

    @RequestMapping(value = "/getGameSettingsForUser", method = RequestMethod.GET)
    public String getGameSettingsForUser(@RequestParam("userId") String userId) {
    	LOGGER.info("Getting game settings for "+userId);
        return userService.getGameSettingsForUser(userId).toString();
    }

    @RequestMapping(value = "/stats/getEndOfSceneStats", method = RequestMethod.GET)
    public String getEndOfSceneStats(@RequestParam("userId") String userId,
                                     @RequestParam("taskNumber") Integer taskNumber) {
    	LOGGER.info("Getting end of scene stats for "+userId);
        return userService.getEndOfSceneStats(userId, taskNumber).toString();
    }

    @RequestMapping(value = "/cronJobs/updateSpeedForAllUsers", method = RequestMethod.GET)
    public String updateSpeedForAllUsers() {
        userService.updateSpeedForAllUsers();
        return "DONE";
    }

    @RequestMapping(value = "/cronJobs/updateLevelOfExpertiseForAllUsers", method = RequestMethod.GET)
    public String updateLevelOfExpertiseForAllUsers() {
        userService.updateLevelOfExpertiseForAllUsers();
        return "DONE";
    }

    @RequestMapping(value = "/settings/setSpeedForUser", method = RequestMethod.POST)
    public void setSpeedForUser(@RequestParam("userId") String userId,
                                @RequestParam("timeTaken") Integer timeTaken) {
        userService.setSpeedForUser(userId, timeTaken);
    }

}
