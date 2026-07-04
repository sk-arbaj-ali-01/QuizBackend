using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Quiz.BL.Abstractions;
using Quiz.DL.Abstractions;
using Quiz.Shared.DTO.Result.Response;
using Quiz.Shared.Helpers;

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
        ReportData examReportData = await CalculateResult(userId, groupId);

        return GeneratePdfReport(examReportData);
    }
    private async Task<ReportData> CalculateResult(Guid userId, Guid groupId)
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

        List<Question> reportForPdf = new();
        int totalMarksObtained = 0;
        
        foreach(var item in studentAnswers.McqAnswers)
        {
            foreach(var question in examQuestions.McqQuestions)
            {
                if(item.QuestionId == question.QuestionId)
                {
                    var qt = new Question
                    {
                        Number = 01,
                        Type = question.QuestionType,
                        Text = question.QuestionText,
                        Options = question.Options.Select(x => x.OptionText).ToList()
                    };

                    foreach(var option in question.Options)
                    {
                        if (option.IsCorrect)
                        {
                            qt.CorrectAnswers = [option.OptionText];
                        }
                    }

                    foreach(var option in question.Options)
                    {
                        if(item.OptionId == option.OptionId)
                        {
                            if(option.IsCorrect == true)
                            {
                                totalMarksObtained += question.Points;
                                qt.IsCorrect = true;
                                qt.StudentAnswers = [option.OptionText];
                                qt.PointsAwarded = question.Points;
                                reportForPdf.Add(qt);
                            }
                            else
                            {
                                qt.IsCorrect = false;
                                qt.StudentAnswers = [option.OptionText];
                                qt.PointsAwarded = 0;
                                reportForPdf.Add(qt);
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

                        reportForPdf.Add(new Question
                        {
                            Number = 01,
                            Type = question.QuestionType,
                            Text = question.QuestionText,
                            IsCorrect = true,
                            Options = question.Options.Select(x => x.OptionText).ToList(),
                            CorrectAnswers = question.Options.Where(x => x.IsCorrect).Select(x => x.OptionText).ToList(),
                            StudentAnswers = question.Options.Where(x => item.OptionIds.Contains(x.OptionId)).Select(x => x.OptionText).ToList(),
                            PointsAwarded = question.Points
                        });
                    }
                    else
                    {
                        reportForPdf.Add(new Question
                        {
                            Number = 01,
                            Type = question.QuestionType,
                            Text = question.QuestionText,
                            IsCorrect = false,
                            Options = question.Options.Select(x => x.OptionText).ToList(),
                            CorrectAnswers = question.Options.Where(x => x.IsCorrect).Select(x => x.OptionText).ToList(),
                            StudentAnswers = question.Options.Where(x => item.OptionIds.Contains(x.OptionId)).Select(x => x.OptionText).ToList(),
                            PointsAwarded = 0
                        });
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

                        reportForPdf.Add(new Question
                        {
                            Number = 01,
                            Type = question.QuestionType,
                            IsCorrect = true,
                            Text = question.QuestionText,
                            CorrectAnswers = [question.CorrectAnswer ? "True" : "False"],
                            StudentAnswers = [item.Answer ? "True" : "False"],
                            PointsAwarded = question.Points
                        });
                    }
                    else
                    {
                        reportForPdf.Add(new Question
                        {
                            Number = 01,
                            Type = question.QuestionType,
                            IsCorrect = false,
                            Text = question.QuestionText,
                            CorrectAnswers = [question.CorrectAnswer ? "True" : "False"],
                            StudentAnswers = [item.Answer ? "True" : "False"],
                            PointsAwarded = 0
                        });
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

                        reportForPdf.Add(new Question
                        {
                            Number = 01,
                            Type = question.QuestionType,
                            IsCorrect = true,
                            Text = question.QuestionText,
                            StudentAnswerText = item.AnswerText,
                            PointsAwarded = question.Points
                        });
                    }
                    else
                    {
                        reportForPdf.Add(new Question
                        {
                            Number = 01,
                            Type = question.QuestionType,
                            IsCorrect = false,
                            Text = question.QuestionText,
                            StudentAnswerText = item.AnswerText,
                            PointsAwarded = question.Points
                        });
                    }
                }
            }
        }

        var userData = await userRepository.GetUserById(userId);
        var groupData = await groupRepository.GetGroupById(groupId);

        ReportData report = new ReportData
        {
            Title = groupData.GroupName,
            Description = groupData.Description,
            CandidateName = userData.FullName,
            CandidateEmail = userData.EmailId,
            AttemptedDate = DateTime.Now,
            TotalPoints = groupData.TotalPoints,
            PointsReceived = totalMarksObtained,
            Grade = "",
            Remarks = "",
            Questions = reportForPdf
        };

        return report;
    }

    private byte[] GeneratePdfReport(ReportData report)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.EnableDebugging = true;

        // var document = new ExamReportDocument(report);

        return Document
        .Create(container => new StudentReportDocument(report)
        .Compose(container))
        .GeneratePdf();
    }
}
