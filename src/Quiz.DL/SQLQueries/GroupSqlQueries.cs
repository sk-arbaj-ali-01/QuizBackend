namespace Quiz.DL.SQLQueries;
public static class GroupSqlQueries
{
    public const string CreateGroupQuery = @"
        INSERT INTO question_groups
        (
            group_id,
            group_name,
            description,
            is_active,
            active_for_days,
            exam_duration,
            created_by
        )
        VALUES
        (
            @GroupId,
            @GroupName,
            @Description,
            @IsActive,
            @ActiveForDays,
            @ExamDuration,
            @CreatedBy
        )    
    ";

    public const string GetGroupsQuery = @"
        SELECT
            group_id        AS GroupId,
            group_name      AS GroupName,
            description     AS Description,
            is_active       AS IsActive,
            is_archived     AS IsArchived,
            exam_duration   AS ExamDuration,
            created_at      AS CreatedAt,
            active_for_days AS ActiveForDays
        FROM question_groups  
    ";

    public const string GetGroupByIdQuery = @"
    SELECT
        group_id        AS GroupId,
        group_name      AS GroupName,
        description     AS Description,
        is_active       AS IsActive,
        is_archived     AS IsArchieved,
        exam_duration   AS ExamDuration,
        active_for_days AS ActiveForDays
    FROM question_groups
    WHERE group_id = @GroupId
    ";

    public const string UpdateGroupById = @"
        UPDATE question_groups
        SET
            group_name = @GroupName,
            description = @Description,
            is_active = @IsActive,
            is_archived = @IsArchived,
            exam_duration = @ExamDuration,
            modified_at = CURRENT_TIMESTAMP
        WHERE
            group_id = @GroupId
    ";

    public const string DeleteGroupById = @"
        DELETE FROM question_groups
        WHERE
            group_id = @GroupId
    ";
}
