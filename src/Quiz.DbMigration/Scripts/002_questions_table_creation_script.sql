CREATE TABLE `mcq_questions` (
    `question_id` CHAR(36) PRIMARY KEY,
    `question_group_id` CHAR(36) NOT NULL,
    `type` VARCHAR(50) NOT NULL,
    `text` TEXT NOT NULL,
    `points` INT NOT NULL,
    CONSTRAINT `fk_mcq_questions_group_id`
        FOREIGN KEY (`question_group_id`)
        REFERENCES `question_groups`(`group_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_mcq_questions_question_group_id`
    ON `mcq_questions`(`question_group_id`);

CREATE TABLE `mcq_question_options` (
    `option_id` CHAR(36) PRIMARY KEY,
    `question_id` CHAR(36) NOT NULL,
    `option_text` TEXT NOT NULL,
    `is_correct` BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT `fk_mcq_question_options_question_id`
        FOREIGN KEY (`question_id`)
        REFERENCES `mcq_questions`(`question_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_mcq_question_options_question_id`
    ON `mcq_question_options`(`question_id`);

CREATE TABLE `msq_questions` (
    `question_id` CHAR(36) PRIMARY KEY,
    `question_group_id` CHAR(36) NOT NULL,
    `type` VARCHAR(50) NOT NULL,
    `text` TEXT NOT NULL,
    `points` INT NOT NULL,
    CONSTRAINT `fk_msq_questions_group_id`
        FOREIGN KEY (`question_group_id`)
        REFERENCES `question_groups`(`group_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_msq_questions_question_group_id`
    ON `msq_questions`(`question_group_id`);

CREATE TABLE `msq_question_options` (
    `option_id` CHAR(36) PRIMARY KEY,
    `question_id` CHAR(36) NOT NULL,
    `option_text` TEXT NOT NULL,
    `is_correct` BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT `fk_msq_question_options_question_id`
        FOREIGN KEY (`question_id`)
        REFERENCES `msq_questions`(`question_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_msq_question_options_question_id`
    ON `msq_question_options`(`question_id`);

CREATE TABLE `true_false_questions` (
    `question_id` CHAR(36) PRIMARY KEY,
    `question_group_id` CHAR(36) NOT NULL,
    `type` VARCHAR(50) NOT NULL,
    `text` TEXT NOT NULL,
    `points` INT NOT NULL,
    `correct_answer` BOOLEAN NOT NULL,
    CONSTRAINT `fk_true_false_questions_group_id`
        FOREIGN KEY (`question_group_id`)
        REFERENCES `question_groups`(`group_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_true_false_questions_question_group_id`
    ON `true_false_questions`(`question_group_id`);

CREATE TABLE `short_answer_questions` (
    `question_id` CHAR(36) PRIMARY KEY,
    `question_group_id` CHAR(36) NOT NULL,
    `type` VARCHAR(50) NOT NULL,
    `text` TEXT NOT NULL,
    `points` INT NOT NULL,
    CONSTRAINT `fk_short_answer_questions_group_id`
        FOREIGN KEY (`question_group_id`)
        REFERENCES `question_groups`(`group_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_short_answer_questions_question_group_id`
    ON `short_answer_questions`(`question_group_id`);