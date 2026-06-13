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
            is_archived    AS IsArchieved
        FROM question_groups  
    ";
}
