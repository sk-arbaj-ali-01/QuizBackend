namespace Quiz.DL.SQLQueries;
public static class GroupSqlQueries
{
    public const string CreateGroupQuery = @"
        INSERT INTO groups
        (
            group_id,
            group_name,
            description,
            is_active,
            active_for_days,
            created_by,
            created_at,
            modified_at,
            is_archived
        )
        VALUES
        (
            @GroupId,
            @GroupName,
            @Description,
            @IsActive,
            @ActiveForDays,
            @CreatedBy,
            @CreatedAt,
            @ModifiedAt,
            @IsArchived
        )    
    ";
}
