namespace Quiz.DL.SQLQueries;
public static class UserSqlQueries
{
    public const string CreateUser = @"
        INSERT INTO users
        (
            user_id,
            full_name,
            email_id,
            password,
            role,
            created_at
        )
        VALUES
        (
            @UserId,
            @FullName,
            @EmailId,
            @Password,
            @Role,
            @CreatedAt
        )
    ";

    public const string GetUserById = @"
        SELECT
            user_id     AS UserId,
            full_name   AS FullName,
            email_id    AS EmailId,
            role        AS Role,
            created_at  AS CreatedAt,
            created_by  AS CreatedBy,
            modified_at AS ModifiedAt,
            modified_by AS ModifiedBy
        FROM users
        WHERE user_id = @UserId
    ";

    public const string GetUserDeatilsByEmail = @"
        SELECT
            user_id     AS UserId,
            full_name   AS FullName,
            email_id    AS EmailId,
            role        AS Role,
            password    AS Password
        FROM users
        WHERE
            email_id = @Email
    ";
}
