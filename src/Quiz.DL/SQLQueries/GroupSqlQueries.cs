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
            created_by
        )
        VALUES
        (
            @GroupId,
            @GroupName,
            @Description,
            @IsActive,
            @ActiveForDays,
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
            created_at      AS CreatedAt
        FROM question_groups  
    ";

    public const string GetGroupByIdQuery = @"
    SELECT
        group_id        AS GroupId,
        group_name      AS GroupName,
        description     AS Description,
        is_active       AS IsActive,
        is_archived     AS IsArchieved
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
