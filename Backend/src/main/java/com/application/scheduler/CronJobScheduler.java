package com.application.scheduler;

import com.application.services.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.util.logging.Logger;

@Service
public class CronJobScheduler {

    private static final Logger log = Logger.getLogger(CronJobScheduler.class.getName());

    @Autowired
    private UserService userService;

    @Scheduled(cron = "*/30 * * * * *")
    public void updateAllUsersSettings() {
        userService.updateSpeedForAllUsers();
        userService.updateLevelOfExpertiseForAllUsers();
    }

}
