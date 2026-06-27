CREATE TABLE `question_groups` (
    `group_id` CHAR(36) PRIMARY KEY,
    `group_name` VARCHAR(255) NOT NULL,
    `description` TEXT,
    `is_active` BOOLEAN NOT NULL DEFAULT FALSE,
    `is_archived` BOOLEAN NOT NULL DEFAULT FALSE,
    `active_for_days` INT,
    `exam_duration` INT NOT NULL DEFAULT 0,
    `total_points`  INT DEFAULT 0,
    `review_required`   BOOLEAN DEFAULT FALSE,
    `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `created_by` CHAR(36) NOT NULL,
    `modified_at` TIMESTAMP,
    `modified_by` CHAR(36)
);
