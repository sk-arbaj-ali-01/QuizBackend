using Quiz.Shared.Enums;

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

    public const string GetTeachersData = @"
        with filtered_teacher as(
	        select 
		        student_id,
		        teacher_id
	        from rel_student_teacher
	        where student_id = @StudentId
        )
        SELECT
            U.user_id         AS UserId,
            U.full_name       AS FullName
        FROM
            users U
        left join filtered_teacher FT 
	        on U.user_id = FT.teacher_id
        where
            FT.teacher_id is null
	        and U.role = 'TEACHER'
    ";

    public const string CreateStudentAndTeacherData = @"
        INSERT INTO rel_student_teacher
        (
            student_id,
            teacher_id
        )
        VALUES
        (
            @StudentId,
            @TeacherId
        )
    ";

    public const string CheckIfStudentAndTeacherDataAlreadyExists = @"
        SELECT
            TRUE
        FROM rel_student_teacher
        WHERE
            student_id = @StudentId
            AND teacher_id = @TeacherId
    ";
}
