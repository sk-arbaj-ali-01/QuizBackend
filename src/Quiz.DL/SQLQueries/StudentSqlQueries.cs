namespace Quiz.DL.SQLQueries;
public static class StudentSqlQueries
{
    public const string GetGroupsForStudents = @"
        SELECT
            QG.group_id                 AS GroupId,
            QG.group_name               AS GroupName,
            QG.description              AS Description,
            QG.exam_duration            AS ExamDuration,
            QG.total_points             AS TotalPoints,
            U.full_name                 AS TeacherName
        FROM rel_student_teacher RST
        LEFT JOIN question_groups QG
            ON RST.teacher_id = QG.created_by
        LEFT JOIN users U
            ON QG.created_by = U.user_id
        WHERE
            RST.student_id = @UserId
            AND QG.is_active = TRUE
            AND QG.is_archived = FALSE
            AND QG.group_id NOT IN (
                SELECT group_id
                FROM rel_user_groups
                WHERE user_id = @UserId
            )
    ";

    public const string GetAttemptedQuizzes = @"
        SELECT
            RUG.group_id                    AS GroupId,
            QG.group_name                   AS GroupName,
            QG.description                  AS Description,
            QG.total_points                 AS TotalPoints,
            RUG.under_review                AS UnderReview,
            RUG.marks_obtained              AS MarksObtained
        FROM rel_user_groups RUG
        INNER JOIN question_groups QG
            ON RUG.group_id = QG.group_id
        WHERE
            RUG.user_id = @UserId
    ";
}
