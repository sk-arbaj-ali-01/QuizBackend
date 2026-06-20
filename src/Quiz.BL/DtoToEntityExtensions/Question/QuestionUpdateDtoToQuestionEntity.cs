using Quiz.DL.Entities;
using Quiz.Shared.DTO.Question.Request;

namespace Quiz.BL.DtoToEntityExtensions.Question;
public static class QuestionUpdateDtoToQuestionEntity
{
    public static List<McqQuestionEntity> ConvertToEntity(this List<McqQuestionUpdateDto> mcqs)
    {
        return mcqs.Select(x =>
            new McqQuestionEntity
            {
                QuestionId = x.QuestionId.GetValueOrDefault(Guid.NewGuid()),
                Type = x.Type.ToString(),
                Text = x.Text,
                Points = x.Points,
                Options = x.Options,
                CorrectAnswer = x.CorrectAnswer
            }).ToList();
    }

    public static List<MsqQuestionEntity> ConvertToEntity(this List<MsqQuestionUpdateDto> msqs)
    {
        return msqs.Select(x =>
            new MsqQuestionEntity
            {
                QuestionId = x.QuestionId.GetValueOrDefault(Guid.NewGuid()),
                Type = x.Type.ToString(),
                Text = x.Text,
                Points = x.Points,
                Options = x.Options,
                CorrectAnswer = x.CorrectAnswer
            }).ToList();
    }

    public static List<TrueFalseQuestionEntity> ConvertToEntity(this List<TrueFalseQuestionUpdateDto> questions)
    {
        return questions.Select(x =>
            new TrueFalseQuestionEntity
            {
                QuestionId = x.QuestionId.GetValueOrDefault(Guid.NewGuid()),
                Type = x.Type.ToString(),
                Text = x.Text,
                Points = x.Points,
                CorrectAnswer = x.CorrectAnswer
            }).ToList();
    }

    public static List<ShortQuestionEntity> ConvertToEntity(this List<ShortQuestionUpdateDto> questions)
    {
        return questions.Select(x =>
            new ShortQuestionEntity
            {
                QuestionId = x.QuestionId.GetValueOrDefault(Guid.NewGuid()),
                Type = x.Type.ToString(),
                Text = x.Text,
                Points = x.Points
            }).ToList();
    }

    public static QuestionEntity ConvertToEntity(this QuestionUpdateRequestDto question)
    {
        return new QuestionEntity
        {
            GroupId = question.GroupId,
            mcq = question.mcq.ConvertToEntity(),
            msq = question.msq.ConvertToEntity(),
            tf = question.tf.ConvertToEntity(),
            sa = question.sa.ConvertToEntity()
        };
    }
}
