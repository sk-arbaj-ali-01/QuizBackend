namespace Quiz.DL.SQLQueries;
public static class StudentSqlQueries
{
    public const string GetGroupsForStudents = @"
        SELECT
            QG.group_id                 AS GroupId,
            QG.group_name               AS GroupName,
            QG.description              AS Description,
            QG.exam_duration            AS ExamDuration,
            QG.total_points             AS TotalPoints
        FROM rel_student_teacher RST
        LEFT JOIN question_groups QG
            ON RST.teacher_id = QG.created_by
        WHERE
            RST.student_id = @UserId
            AND QG.is_active = TRUE
            AND QG.is_archived = FALSE
    ";
}
