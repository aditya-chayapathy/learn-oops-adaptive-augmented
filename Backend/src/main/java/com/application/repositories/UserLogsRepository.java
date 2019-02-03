package com.application.repositories;

import com.application.pojo.UserLogs;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface UserLogsRepository extends JpaRepository<UserLogs, Integer> {

    List<UserLogs> findByUserId(String userId);

    UserLogs findTop1ByUserIdAndIsSuccessOrderByTimestampDesc(String userId, String isSuccess);

    List<UserLogs> findByUserIdAndTaskNumberAndIsSuccess(String userId, Integer taskNumber, String isSuccess);

    List<UserLogs> findByTaskNumberAndIsSuccess(Integer taskNumber, String isSuccess);

    UserLogs findTop1ByUserIdAndTaskNumberAndIsSuccessOrderByTimestampDesc(String userId, Integer taskNumber, String isSuccess);

    UserLogs findTop1ByUserIdAndTaskNumberOrderByTimestampDesc(String userId, Integer taskNumber);

    List<UserLogs> findByUserIdAndProcessed(String userId, String processed);

}
