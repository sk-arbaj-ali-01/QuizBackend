CREATE TABLE `rel_student_teacher` (
    `student_id` CHAR(36) NOT NULL,
    `teacher_id` CHAR(36) NOT NULL,
    `created_at` DATETIME(6) DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`student_id`, `teacher_id`),
    CONSTRAINT `fk_rel_student_teacher_student_id`
        FOREIGN KEY (`student_id`)
        REFERENCES `users`(`user_id`)
        ON DELETE CASCADE,
    CONSTRAINT `fk_rel_student_teacher_teacher_id`
        FOREIGN KEY (`teacher_id`)
        REFERENCES `users`(`user_id`)
        ON DELETE CASCADE
);

CREATE INDEX `ix_rel_student_teacher_teacher_id`
    ON `rel_student_teacher`(`teacher_id`);
