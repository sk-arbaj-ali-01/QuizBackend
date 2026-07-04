namespace Quiz.DL.SQLQueries;
public static class ResultSqlQueries
{
    public const string GetMcqQuestionAndOptionsByGroupId = @"
        select 
			mq.question_id					as QuestionId,
			mq.text							as QuestionText,
			mq.type 						as QuestionType,
			mq.points						as Points,
			mqo.option_id					as OptionId,
			mqo.option_text					as OptionText,
			mqo.is_correct					as IsCorrect
		from mcq_questions mq
		inner join mcq_question_options mqo
			on mq.question_id = mqo.question_id 
		where 
			mq.question_group_id = @GroupId
    ";

    public const string GetMsqQuestionAndOptionsByGroupId = @"
        select 
			mq.question_id					as QuestionId,
			mq.text							as QuestionText,
			mq.type 						as QuestionType,
			mq.points						as Points,
			mqo.option_id					as OptionId,
			mqo.option_text					as OptionText,
			mqo.is_correct					as IsCorrect
		from msq_questions mq
		inner join msq_question_options mqo
			on mq.question_id = mqo.question_id 
		where 
			mq.question_group_id = @GroupId
    ";

	public const string GetTrueFalseQuestionsByGroupId = @"
		select 
			tfq.question_id					as QuestionId,
			tfq.text						as QuestionText,
			tfq.type 						as QuestionType,
			tfq.points						as Points,
			tfq.correct_answer				as CorrectAnswer
		from true_false_questions tfq
		where 
			tfq.question_group_id = @GroupId
	";

	public const string GetShortAnswerQuestionsByGroupId = @"
		select 
			saq.question_id					as QuestionId,
			saq.text						as QuestionText,
			saq.type 						as QuestionType,
			saq.points						as Points
		from short_answer_questions saq
		where 
			saq.question_group_id = @GroupId
	";

	public const string GetMcqOrMsqSubmissionDataByUserId = @"
		select 
			ruafmm.question_id				as QuestionId,
			ruafmm.answered_at				as AnsweredAt,
			ruafmm.option_id				as OptionId
		from rel_user_answers_for_mcq_or_msq ruafmm
		where 
			ruafmm.question_id IN @QuestionIds
			AND ruafmm.user_id = @UserId
	";

	public const string GetTrueFalseSubmissionDataByUserId = @"
		select 
			ruaftf.question_id				as QuestionId,
			ruaftf.answer					as Answer,
			ruaftf.answered_at				as AnsweredAt
		from rel_user_answers_for_true_false ruaftf
		where 
			ruaftf.question_id IN (@QuestionIds)
			AND ruaftf.user_id = @UserId
	";

	public const string GetShortAnswerSubmissionDataByUserId = @"
		select 
			ruafsa.question_id				as QuestionId,
			ruafsa.answer_text				as AnswerText,
			ruafsa.is_correct				as IsCorrect,
			ruafsa.answered_at				as AnsweredAt
		from rel_user_answers_for_short_answer ruafsa
		where 
			ruafsa.question_id IN (@QuestionIds)
			AND ruafsa.user_id = @UserId
	";
}
