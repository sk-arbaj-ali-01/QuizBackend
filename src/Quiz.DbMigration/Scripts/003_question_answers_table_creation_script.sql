CREATE TABLE `rel_user_groups` (
    `user_id` CHAR(36) NOT NULL,
    `group_id` CHAR(36) NOT NULL,
    `assigned_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`user_id`, `group_id`),
    CONSTRAINT `fk_rel_user_groups_group_id`
        FOREIGN KEY (`group_id`)
        REFERENCES `question_groups`(`group_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_rel_user_groups_group_id`
    ON `rel_user_groups`(`group_id`);

CREATE TABLE `rel_user_questions` (
    `user_id` CHAR(36) NOT NULL,
    `question_id` CHAR(36) NOT NULL,
    `question_type` VARCHAR(20) NOT NULL,
    `option_id` CHAR(36),
    `answered_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`user_id`, `question_id`),
    INDEX `ix_rel_user_questions_question_id` (`question_id`),
    INDEX `ix_rel_user_questions_option_id` (`option_id`),
    INDEX `ix_rel_user_questions_question_type` (`question_type`)
);
