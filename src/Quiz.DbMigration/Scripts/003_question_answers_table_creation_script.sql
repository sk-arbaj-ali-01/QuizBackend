CREATE TABLE `rel_user_groups` (
    `user_id`                   CHAR(36) NOT NULL,
    `group_id`                  CHAR(36) NOT NULL,
    `assigned_at`               TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `under_review`              BOOLEAN DEFAULT FALSE,
    `marks_obtained`            INT NULL,

    PRIMARY KEY (`user_id`, `group_id`),
    CONSTRAINT `fk_rel_user_groups_group_id`
        FOREIGN KEY (`group_id`)
        REFERENCES `question_groups`(`group_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_rel_user_groups_group_id`
    ON `rel_user_groups`(`group_id`);

CREATE TABLE `rel_user_answers_for_mcq_or_msq` (
    `user_submission_id`            BIGINT AUTO_INCREMENT PRIMARY KEY,
    `user_id`                       CHAR(36) NOT NULL,
    `question_id`                   CHAR(36) NOT NULL,
    `question_type`                 VARCHAR(20) NOT NULL,
    `option_id`                     CHAR(36),
    `answered_at`                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE `rel_user_answers_for_true_false` (
    `user_submission_id`            BIGINT AUTO_INCREMENT PRIMARY KEY,
    `user_id`                       CHAR(36) NOT NULL,
    `question_id`                   CHAR(36) NOT NULL,
    `answer`                        BOOLEAN,
    `answered_at`                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE `rel_user_answers_for_short_answer` (
    `user_submission_id`            BIGINT AUTO_INCREMENT PRIMARY KEY,
    `user_id`                       CHAR(36) NOT NULL,
    `question_id`                   CHAR(36) NOT NULL,
    `answer_text`                   TEXT,
    `is_correct`                    BOOLEAN DEFAULT FALSE,
    `answered_at`                   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
