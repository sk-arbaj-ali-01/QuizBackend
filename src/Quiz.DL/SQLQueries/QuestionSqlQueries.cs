namespace Quiz.DL.SQLQueries;
public static class QuestionSqlQueries
{
    public const string CreateMcqQuestion = @"
        INSERT INTO mcq_questions
        (
            question_id,
            question_group_id,
            type,
            text,
            points
        )
        VALUES
        (
            @QuestionId,
            @QuestionGroupId,
            @Type,
            @Text,
            @Points
        );
    ";

    public const string CreateMcqOption = @"
        INSERT INTO mcq_question_options
        (
            option_id,
            question_id,
            option_text,
            is_correct
        )
        VALUES
        (
            @OptionId,
            @QuestionId,
            @OptionText,
            @IsCorrect
        );
    ";

    public const string CreateMsqQuestion = @"
        INSERT INTO msq_questions
        (
            question_id,
            question_group_id,
            type,
            text,
            points
        )
        VALUES
        (
            @QuestionId,
            @QuestionGroupId,
            @Type,
            @Text,
            @Points
        );
    ";

    public const string CreateMsqOption = @"
        INSERT INTO msq_question_options
        (
            option_id,
            question_id,
            option_text,
            is_correct
        )
        VALUES
        (
            @OptionId,
            @QuestionId,
            @OptionText,
            @IsCorrect
        );
    ";

    public const string CreateTrueFalseQuestion = @"
        INSERT INTO true_false_questions
        (
            question_id,
            question_group_id,
            type,
            text,
            points,
            correct_answer
        )
        VALUES
        (
            @QuestionId,
            @QuestionGroupId,
            @Type,
            @Text,
            @Points,
            @CorrectAnswer
        );
    ";

    public const string CreateShortAnswerQuestion = @"
        INSERT INTO short_answer_questions
        (
            question_id,
            question_group_id,
            type,
            text,
            points
        )
        VALUES
        (
            @QuestionId,
            @QuestionGroupId,
            @Type,
            @Text,
            @Points
        );
    ";

    public const string CreateRelUserGroup = @"
        INSERT INTO rel_user_groups
        (
            user_id,
            group_id
        )
        VALUES
        (
            @UserId,
            @GroupId
        )
        ON DUPLICATE KEY UPDATE
            assigned_at = assigned_at;
    ";

    public const string UpsertRelUserQuestion = @"
        INSERT INTO rel_user_questions
        (
            user_id,
            question_id,
            question_type,
            option_id
        )
        VALUES
        (
            @UserId,
            @QuestionId,
            @QuestionType,
            @OptionId
        )
        ON DUPLICATE KEY UPDATE
            question_type = VALUES(question_type),
            option_id = VALUES(option_id),
            answered_at = CURRENT_TIMESTAMP;
    ";

    public const string GetMcqQuestionsByGroupId = @"
        SELECT
            q.question_id AS QuestionId,
            q.type AS Type,
            q.text AS Text,
            q.points AS Points,
            o.option_id AS OptionId,
            o.option_text AS OptionText,
            o.is_correct AS IsCorrect
        FROM mcq_questions q
        LEFT JOIN mcq_question_options o
            ON q.question_id = o.question_id
        WHERE q.question_group_id = @GroupId
        ORDER BY q.question_id;
    ";

    public const string GetMsqQuestionsByGroupId = @"
        SELECT
            q.question_id AS QuestionId,
            q.type AS Type,
            q.text AS Text,
            q.points AS Points,
            o.option_id AS OptionId,
            o.option_text AS OptionText,
            o.is_correct AS IsCorrect
        FROM msq_questions q
        LEFT JOIN msq_question_options o
            ON q.question_id = o.question_id
        WHERE q.question_group_id = @GroupId
        ORDER BY q.question_id;
    ";

    public const string GetTrueFalseQuestionsByGroupId = @"
        SELECT
            question_id AS QuestionId,
            type AS Type,
            text AS Text,
            points AS Points,
            correct_answer AS CorrectAnswer
        FROM true_false_questions
        WHERE question_group_id = @GroupId
        ORDER BY question_id;
    ";

    public const string GetShortQuestionsByGroupId = @"
        SELECT
            question_id AS QuestionId,
            type AS Type,
            text AS Text,
            points AS Points
        FROM short_answer_questions
        WHERE question_group_id = @GroupId
        ORDER BY question_id;
    ";

    public const string GetQuestionGroupMetadataByGroupId = @"
        SELECT
            group_id AS GroupId,
            exam_duration AS ExamDuration
        FROM question_groups
        WHERE group_id = @GroupId;
    ";

    public const string GetMcqQuestionIdsByGroupId = @"
        SELECT question_id
        FROM mcq_questions
        WHERE question_group_id = @GroupId;
    ";

    public const string GetMsqQuestionIdsByGroupId = @"
        SELECT question_id
        FROM msq_questions
        WHERE question_group_id = @GroupId;
    ";

    public const string GetTrueFalseQuestionIdsByGroupId = @"
        SELECT question_id
        FROM true_false_questions
        WHERE question_group_id = @GroupId;
    ";

    public const string GetShortQuestionIdsByGroupId = @"
        SELECT question_id
        FROM short_answer_questions
        WHERE question_group_id = @GroupId;
    ";

    public const string UpdateMcqQuestion = @"
        UPDATE mcq_questions
        SET
            type = @Type,
            text = @Text,
            points = @Points
        WHERE
            question_id = @QuestionId
            AND question_group_id = @QuestionGroupId;
    ";

    public const string UpdateMsqQuestion = @"
        UPDATE msq_questions
        SET
            type = @Type,
            text = @Text,
            points = @Points
        WHERE
            question_id = @QuestionId
            AND question_group_id = @QuestionGroupId;
    ";

    public const string UpdateTrueFalseQuestion = @"
        UPDATE true_false_questions
        SET
            type = @Type,
            text = @Text,
            points = @Points,
            correct_answer = @CorrectAnswer
        WHERE
            question_id = @QuestionId
            AND question_group_id = @QuestionGroupId;
    ";

    public const string UpdateShortAnswerQuestion = @"
        UPDATE short_answer_questions
        SET
            type = @Type,
            text = @Text,
            points = @Points
        WHERE
            question_id = @QuestionId
            AND question_group_id = @QuestionGroupId;
    ";

    public const string DeleteMcqOptionsByQuestionId = @"
        DELETE FROM mcq_question_options
        WHERE question_id = @QuestionId;
    ";

    public const string DeleteMsqOptionsByQuestionId = @"
        DELETE FROM msq_question_options
        WHERE question_id = @QuestionId;
    ";

    public const string DeleteMcqQuestionsByIds = @"
        DELETE FROM mcq_questions
        WHERE question_group_id = @QuestionGroupId
            AND question_id IN @QuestionIds;
    ";

    public const string DeleteMsqQuestionsByIds = @"
        DELETE FROM msq_questions
        WHERE question_group_id = @QuestionGroupId
            AND question_id IN @QuestionIds;
    ";

    public const string DeleteTrueFalseQuestionsByIds = @"
        DELETE FROM true_false_questions
        WHERE question_group_id = @QuestionGroupId
            AND question_id IN @QuestionIds;
    ";

    public const string DeleteShortAnswerQuestionsByIds = @"
        DELETE FROM short_answer_questions
        WHERE question_group_id = @QuestionGroupId
            AND question_id IN @QuestionIds;
    ";

    public const string UpdatePointsForGroupWhileCreatingTheQuestions = @"
        UPDATE TABLE question_groups
        SET
            table_points = @TotalPoints
        WHERE
            group_id = @GroupId
    ";
}
