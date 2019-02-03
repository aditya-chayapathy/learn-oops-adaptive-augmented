package com.application.pojo;

import javax.persistence.*;
import java.util.Date;

@Entity
@Table(name = "USER_LOGS")
public class UserLogs {

    @Id
    @GeneratedValue
    private Integer id;

    @Column(name = "SPEED")
    private Integer speed;

    @Column(name = "NO_OF_WALL_COLLISIONS")
    private Integer numberOfWallCollisions;

    @Column(name = "TASK_NUMBER")
    private Integer taskNumber;

    @Column(name = "USER_ID")
    private String userId;

    @Column(name = "IS_SUCCESS")
    private String isSuccess;

    @Column(name = "TIME_TAKEN")
    private Long timeTaken;

    @Column(name = "TIMESTAMP")
    private Date timestamp;

    @Column(name = "PROCESSED")
    private String processed;

    public UserLogs() {

    }

    public UserLogs(Integer speed, Integer numberOfWallCollisions, Integer taskNumber, String userId, String isSuccess, Long timeTaken, Date timestamp, String processed) {
        this.speed = speed;
        this.numberOfWallCollisions = numberOfWallCollisions;
        this.taskNumber = taskNumber;
        this.userId = userId;
        this.isSuccess = isSuccess;
        this.timeTaken = timeTaken;
        this.timestamp = timestamp;
        this.processed = processed;
    }

    public String getProcessed() {
        return processed;
    }

    public void setProcessed(String processed) {
        this.processed = processed;
    }

    public Integer getSpeed() {
        return speed;
    }

    public void setSpeed(Integer speed) {
        this.speed = speed;
    }

    public Integer getNumberOfWallCollisions() {
        return numberOfWallCollisions;
    }

    public void setNumberOfWallCollisions(Integer numberOfWallCollisions) {
        this.numberOfWallCollisions = numberOfWallCollisions;
    }

    public Integer getTaskNumber() {
        return taskNumber;
    }

    public void setTaskNumber(Integer taskNumber) {
        this.taskNumber = taskNumber;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getIsSuccess() {
        return isSuccess;
    }

    public void setIsSuccess(String isSuccess) {
        this.isSuccess = isSuccess;
    }

    public Long getTimeTaken() {
        return timeTaken;
    }

    public void setTimeTaken(Long timeTaken) {
        this.timeTaken = timeTaken;
    }

    public Date getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(Date timestamp) {
        this.timestamp = timestamp;
    }
}
