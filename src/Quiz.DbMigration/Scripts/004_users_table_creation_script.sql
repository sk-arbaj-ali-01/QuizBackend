CREATE TABLE `users` (
    `user_id` CHAR(36) NOT NULL,
    `full_name` VARCHAR(255) NOT NULL,
    `email_id` VARCHAR(255) NOT NULL,
    `password` VARCHAR(255),
    `role`      VARCHAR(10),
    `created_at` DATETIME(6) DEFAULT CURRENT_TIMESTAMP(6),
    `created_by` CHAR(36),
    `modified_at` DATETIME(6) NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(6),
    `modified_by` CHAR(36) NULL,
    PRIMARY KEY (`user_id`),
    UNIQUE KEY `uk_users_email_id` (`email_id`)
);