using Quiz.DL.Entities;
using Quiz.Shared.DTO.Question.Request;

namespace Quiz.BL.DtoToEntityExtensions.Question;
public static class QuestionCreateDtoToQuestionEntity
{
    public static List<McqQuestionEntity> ConvertToEntity(this List<McqQuestion> mcqs)
    {
        IEnumerable<McqQuestionEntity> mcqEntity = mcqs.Select(x =>
            new McqQuestionEntity
            {
                QuestionId = Guid.NewGuid(),
                Type = x.Type.ToString(),
                Text = x.Text,
                Points = x.Points,
                Options = x.Options,
                CorrectAnswer = x.CorrectAnswer
            });

        return mcqEntity.ToList();
    }

    public static List<MsqQuestionEntity> ConvertToEntity(this List<MsqQuestion> msqs)
    {
        IEnumerable<MsqQuestionEntity> msqEntity = msqs.Select(x =>
            new MsqQuestionEntity
            {
                QuestionId = Guid.NewGuid(),
                Type = x.Type.ToString(),
                Text = x.Text,
                Points = x.Points,
                Options = x.Options,
                CorrectAnswer = x.CorrectAnswer
            });

        return msqEntity.ToList();
    }

    public static List<TrueFalseQuestionEntity> ConvertToEntity(this List<TrueFalseQuestion> questions)
    {
        IEnumerable<TrueFalseQuestionEntity> ques = questions.Select(x =>
            new TrueFalseQuestionEntity
            {
                QuestionId = Guid.NewGuid(),
                Type = x.Type.ToString(),
                Text = x.Text,
                Points = x.Points,
                CorrectAnswer = x.CorrectAnswer
            });

        return ques.ToList();
    }

    public static List<ShortQuestionEntity> ConvertToEntity(this List<ShortQuestion> questions)
    {
        IEnumerable<ShortQuestionEntity> ques = questions.Select(x =>
            new ShortQuestionEntity
            {
                QuestionId = Guid.NewGuid(),
                Type = x.Type.ToString(),
                Text = x.Text,
                Points = x.Points
            });

        return ques.ToList();
    }

    public static QuestionEntity ConvertToEntity(this QuestionCreateRequestDto question)
    {
        QuestionEntity ques = 
            new QuestionEntity
            {
                GroupId = question.GroupId,
                mcq = question.mcq.ConvertToEntity(),
                msq = question.msq.ConvertToEntity(),
                tf = question.tf.ConvertToEntity(),
                sa = question.sa.ConvertToEntity(),
            };

        return ques;
    }
}
