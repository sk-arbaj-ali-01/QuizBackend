using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Quiz.BL.Abstractions;
using Quiz.DL.Abstractions;
using Quiz.Shared.DTO.Result.Response;
using Quiz.Shared.Models;

namespace Quiz.BL.Services;
public class ResultService(
    ILogger<ResultService> logger,
    IResultRepository resultRepository,
    IUserRepository userRepository,
    IGroupRepository groupRepository)
    : BaseService, IResultService
{

    public async Task<byte[]> GetExamReport(Guid userId, Guid groupId)
    {
        ExamReportData examReportData = await CalculateResult(userId, groupId);

        return GeneratePdfReport(examReportData);
    }
    private async Task<ExamReportData> CalculateResult(Guid userId, Guid groupId)
    {
        ExamQuestionsResponseDto examQuestions = 
            await resultRepository.GetExamQuestionsByGroupId(groupId);

        var mcqQuestinIds = examQuestions.McqQuestions.Select(x => x.QuestionId);
        var msqQuestinIds = examQuestions.MsqQuestions.Select(x => x.QuestionId);
        var tfQuestinIds = examQuestions.TrueFalseQuestions.Select(x => x.QuestionId);
        var saQuestinIds = examQuestions.ShortAnswerQuestions.Select(x => x.QuestionId);

        StudentAnswersResponseDto studentAnswers =
            await resultRepository.GetStudentAnswersByUserAndGroupId(
                userId,
                groupId,
                mcqQuestinIds,
                msqQuestinIds,
                tfQuestinIds,
                saQuestinIds);

        List<ExamQuestion> reportForPdf = new();
        int totalMarksObtained = 0;
        
        foreach(var item in studentAnswers.McqAnswers)
        {
            foreach(var question in examQuestions.McqQuestions)
            {
                if(item.QuestionId == question.QuestionId)
                {
                    foreach(var option in question.Options)
                    {
                        if(item.OptionId == option.OptionId)
                        {
                            if(option.IsCorrect == true)
                            {
                                totalMarksObtained += question.Points;

                                reportForPdf.Add(new ExamQuestion(
                                    Number: "01",
                                    Type: question.QuestionType.ToString(),
                                    IsCorrect: true,
                                    QuestionText: question.QuestionText,
                                    Options: question.Options.Select(x => x.OptionText).ToArray(),
                                    SelectedOptions: [option.OptionText],
                                    CorrectOptions: [option.OptionText],
                                    StudentAnswer: "",
                                    PointsAwarded: question.Points,
                                    MaxPoints: question.Points
                                ));
                            }
                            else
                            {
                                reportForPdf.Add(new ExamQuestion(
                                    Number: "01",
                                    Type: question.QuestionType.ToString(),
                                    IsCorrect: false,
                                    QuestionText: question.QuestionText,
                                    Options: question.Options.Select(x => x.OptionText).ToArray(),
                                    SelectedOptions: [option.OptionText],
                                    CorrectOptions: [option.OptionText],
                                    StudentAnswer: "",
                                    PointsAwarded: 0,
                                    MaxPoints: question.Points
                                ));
                            }
                        }
                    }
                }
            }
        }

        foreach(var item in studentAnswers.MsqAnswers)
        {
            foreach(var question in examQuestions.MsqQuestions)
            {
                if(item.QuestionId == question.QuestionId)
                {
                    HashSet<Guid> correctOptions = question.Options
                        .Where(x => x.IsCorrect == true)
                        .Select(x => x.OptionId)
                        .ToHashSet();

                    HashSet<Guid> studentSelections = item.OptionIds.ToHashSet();

                    if(correctOptions.Intersect(studentSelections).Count() == 0)
                    {
                        totalMarksObtained += question.Points;

                        reportForPdf.Add(new ExamQuestion(
                            Number: "01",
                            Type: question.QuestionType.ToString(),
                            IsCorrect: true,
                            QuestionText: question.QuestionText,
                            Options: question.Options.Select(x => x.OptionText).ToArray(),
                            SelectedOptions: question.Options.Where(x => item.OptionIds.Contains(x.OptionId)).Select(x => x.OptionText).ToArray(),
                            CorrectOptions: question.Options.Where(x => x.IsCorrect).Select(x => x.OptionText).ToArray(),
                            StudentAnswer: "",
                            PointsAwarded: question.Points,
                            MaxPoints: question.Points
                        ));
                    }
                    else
                    {
                        reportForPdf.Add(new ExamQuestion(
                            Number: "01",
                            Type: question.QuestionType.ToString(),
                            IsCorrect: false,
                            QuestionText: question.QuestionText,
                            Options: question.Options.Select(x => x.OptionText).ToArray(),
                            SelectedOptions: question.Options.Where(x => item.OptionIds.Contains(x.OptionId)).Select(x => x.OptionText).ToArray(),
                            CorrectOptions: question.Options.Where(x => x.IsCorrect).Select(x => x.OptionText).ToArray(),
                            StudentAnswer: "",
                            PointsAwarded: 0,
                            MaxPoints: question.Points
                        ));
                    }
                }
            }
        }

        foreach(var item in studentAnswers.TrueFalseAnswers)
        {
            foreach(var question in examQuestions.TrueFalseQuestions)
            {
                if(item.QuestionId == question.QuestionId)
                {
                    if(item.Answer == question.CorrectAnswer)
                    {
                        totalMarksObtained += question.Points;

                        reportForPdf.Add(new ExamQuestion(
                            Number: "01",
                            Type: question.QuestionType.ToString(),
                            IsCorrect: true,
                            QuestionText: question.QuestionText,
                            Options: new[] { "True", "False" },
                            SelectedOptions: [item.Answer ? "True" : "False"],
                            CorrectOptions: [question.CorrectAnswer ? "True" : "False"],
                            StudentAnswer: "",
                            PointsAwarded: question.Points,
                            MaxPoints: question.Points
                        ));
                    }
                    else
                    {
                        reportForPdf.Add(new ExamQuestion(
                            Number: "01",
                            Type: question.QuestionType.ToString(),
                            IsCorrect: false,
                            QuestionText: question.QuestionText,
                            Options: new[] { "True", "False" },
                            SelectedOptions: [item.Answer ? "True" : "False"],
                            CorrectOptions: [question.CorrectAnswer ? "True" : "False"],
                            StudentAnswer: "",
                            PointsAwarded: 0,
                            MaxPoints: question.Points
                        ));
                    }
                }
            }
        }

        foreach(var item in studentAnswers.ShortAnswers)
        {
            foreach(var question in examQuestions.ShortAnswerQuestions)
            {
                if(item.QuestionId == question.QuestionId)
                {
                    if(item.IsCorrect)
                    {
                        totalMarksObtained += question.Points;

                        reportForPdf.Add(new ExamQuestion(
                            Number: "01",
                            Type: question.QuestionType.ToString(),
                            IsCorrect: true,
                            QuestionText: question.QuestionText,
                            Options: [],
                            SelectedOptions: [item.AnswerText],
                            CorrectOptions: ["Verified Correct"],
                            StudentAnswer: "",
                            PointsAwarded: question.Points,
                            MaxPoints: question.Points
                        ));
                    }
                    else
                    {
                        reportForPdf.Add(new ExamQuestion(
                            Number: "01",
                            Type: question.QuestionType.ToString(),
                            IsCorrect: false,
                            QuestionText: question.QuestionText,
                            Options: [],
                            SelectedOptions: [item.AnswerText],
                            CorrectOptions: [],
                            StudentAnswer: "",
                            PointsAwarded: 0,
                            MaxPoints: question.Points
                        ));
                    }
                }
            }
        }

        var userData = await userRepository.GetUserById(userId);
        var groupData = await groupRepository.GetGroupById(groupId);

        ExamReportData report = new ExamReportData(
            userData.FullName,
            userData.EmailId,
            groupData.GroupName,
            groupData.Description,
            "",
            groupData.TotalPoints,
            totalMarksObtained,
            "",
            "",
            "",
            reportForPdf);

        return report;
    }

    private byte[] GeneratePdfReport(ExamReportData report)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.EnableDebugging = true;

        var document = new ExamReportDocument(report);

        return document.GeneratePdf();
    }
}
