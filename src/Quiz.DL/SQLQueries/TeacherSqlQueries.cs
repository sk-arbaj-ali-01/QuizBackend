namespace Quiz.DL.SQLQueries;
public static class TeacherSqlQueries
{
    public const string GetExamDataToBeReviewed = @"
        select 
			qg.group_id 				as GroupId,
			rug.user_id 				as StudentId,
			u.full_name 				as FullName,
			qg.total_points 			as TotalPoints
		from
		question_groups qg 
		inner join rel_user_groups rug 
			on (
			qg.group_id  = rug.group_id 
			and qg.is_active = true
			and rug.under_review = true
			)
		inner join users u 
			on rug.user_id = u.user_id 
		where
			qg.created_by = @UserId
    ";

	public const string GetShortAnswerQuestionByGroupId = @"
		select 
			saq.question_id				as QuestionId,
			saq.text					as QuestionText,
			saq.points					as Points
		from short_answer_questions saq
		where 
			saq.question_group_id = @GroupId
	";

	public const string GetShortAnswersByStudentId = @"
		select
			ruafsa.question_id				as QuestionId,
			ruafsa.answer_text				as AnswerText
		from rel_user_answers_for_short_answer ruafsa
		where
			ruafsa.question_id in (@QuestionIds)
			and ruafsa.user_id = @StudentId
	";
}
