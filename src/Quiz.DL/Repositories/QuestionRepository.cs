using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.DL.Service;
using Quiz.DL.SQLQueries;
using Quiz.Shared.DTO.Question.Request;
using Quiz.Shared.DTO.Question.Response;
using System.Data;

namespace Quiz.DL.Repositories;
public class QuestionRepository(
    ILogger<QuestionRepository> logger,
    IConfiguration configuration)
    : DbConnectionManager(configuration, logger), IQuestionRepository
{
    public async Task CreateQuestions(QuestionEntity question)
    {
        ArgumentNullException.ThrowIfNull(question);

        int totalPoints = 0;

        await DbOperationInTransaction(async (conn, transaction) =>
        {
            foreach (McqQuestionEntity mcqQuestion in question.mcq)
            {
                await conn.ExecuteAsync(
                    QuestionSqlQueries.CreateMcqQuestion,
                    new
                    {
                        mcqQuestion.QuestionId,
                        QuestionGroupId = question.GroupId,
                        mcqQuestion.Type,
                        mcqQuestion.Text,
                        mcqQuestion.Points
                    },
                    transaction);

                totalPoints += mcqQuestion.Points;

                ValidateMcqAnswerIndex(mcqQuestion);

                for (int i = 0; i < mcqQuestion.Options.Count; i++)
                {
                    await conn.ExecuteAsync(
                        QuestionSqlQueries.CreateMcqOption,
                        new
                        {
                            OptionId = Guid.NewGuid(),
                            mcqQuestion.QuestionId,
                            OptionText = mcqQuestion.Options[i],
                            IsCorrect = i == mcqQuestion.CorrectAnswer
                        },
                        transaction);
                }
            }

            foreach (MsqQuestionEntity msqQuestion in question.msq)
            {
                await conn.ExecuteAsync(
                    QuestionSqlQueries.CreateMsqQuestion,
                    new
                    {
                        msqQuestion.QuestionId,
                        QuestionGroupId = question.GroupId,
                        msqQuestion.Type,
                        msqQuestion.Text,
                        msqQuestion.Points
                    },
                    transaction);

                totalPoints += msqQuestion.Points;

                HashSet<int> correctOptionIndexes = new(msqQuestion.CorrectAnswer);

                for (int i = 0; i < msqQuestion.Options.Count; i++)
                {
                    await conn.ExecuteAsync(
                        QuestionSqlQueries.CreateMsqOption,
                        new
                        {
                            OptionId = Guid.NewGuid(),
                            msqQuestion.QuestionId,
                            OptionText = msqQuestion.Options[i],
                            IsCorrect = correctOptionIndexes.Contains(i)
                        },
                        transaction);
                }
            }

            foreach (TrueFalseQuestionEntity trueFalseQuestion in question.tf)
            {
                await conn.ExecuteAsync(
                    QuestionSqlQueries.CreateTrueFalseQuestion,
                    new
                    {
                        trueFalseQuestion.QuestionId,
                        QuestionGroupId = question.GroupId,
                        trueFalseQuestion.Type,
                        trueFalseQuestion.Text,
                        trueFalseQuestion.Points,
                        trueFalseQuestion.CorrectAnswer
                    },
                    transaction);

                totalPoints += trueFalseQuestion.Points;
            }

            foreach (ShortQuestionEntity shortQuestion in question.sa)
            {
                await conn.ExecuteAsync(
                    QuestionSqlQueries.CreateShortAnswerQuestion,
                    new
                    {
                        shortQuestion.QuestionId,
                        QuestionGroupId = question.GroupId,
                        shortQuestion.Type,
                        shortQuestion.Text,
                        shortQuestion.Points
                    },
                    transaction);

                totalPoints += shortQuestion.Points;
            }

            await conn.ExecuteAsync(
                QuestionSqlQueries.UpdatePointsForGroupWhileCreatingTheQuestions,
                new
                {
                    TotalPoints = totalPoints,
                    question.GroupId
                }, transaction);

            return 1;
        });
    }

    public async Task UpdateQuestions(QuestionEntity question)
    {
        ArgumentNullException.ThrowIfNull(question);

        await DbOperationInTransaction(async (conn, transaction) =>
        {
            HashSet<Guid> existingMcqQuestionIds = (await conn.QueryAsync<Guid>(
                QuestionSqlQueries.GetMcqQuestionIdsByGroupId,
                new { GroupId = question.GroupId },
                transaction)).ToHashSet();

            HashSet<Guid> existingMsqQuestionIds = (await conn.QueryAsync<Guid>(
                QuestionSqlQueries.GetMsqQuestionIdsByGroupId,
                new { GroupId = question.GroupId },
                transaction)).ToHashSet();

            HashSet<Guid> existingTrueFalseQuestionIds = (await conn.QueryAsync<Guid>(
                QuestionSqlQueries.GetTrueFalseQuestionIdsByGroupId,
                new { GroupId = question.GroupId },
                transaction)).ToHashSet();

            HashSet<Guid> existingShortQuestionIds = (await conn.QueryAsync<Guid>(
                QuestionSqlQueries.GetShortQuestionIdsByGroupId,
                new { GroupId = question.GroupId },
                transaction)).ToHashSet();

            await UpdateMcqQuestions(question, existingMcqQuestionIds, conn, transaction);
            await UpdateMsqQuestions(question, existingMsqQuestionIds, conn, transaction);
            await UpdateTrueFalseQuestions(question, existingTrueFalseQuestionIds, conn, transaction);
            await UpdateShortQuestions(question, existingShortQuestionIds, conn, transaction);

            return 1;
        });
    }

    public async Task SubmitAnswers(QuestionSubmissionEntity submission)
    {
        ArgumentNullException.ThrowIfNull(submission);

        await DbOperationInTransaction(async (conn, transaction) =>
        {
            await conn.ExecuteAsync(
                QuestionSqlQueries.CreateRelUserGroup,
                new
                {
                    submission.UserId,
                    submission.GroupId,
                    UnderReview = submission.ShortAnswers.Any() ? true : false,
                },
                transaction);

            if(submission.McqAnswers.Count > 0)
            {
                foreach(var answer in submission.McqAnswers)
                {
                    await conn.ExecuteAsync(
                        QuestionSqlQueries.SubmitAnswerForMcqQuestions,
                        new
                        {
                            submission.UserId,
                            answer.QuestionId,
                            answer.QuestionType,
                            answer.OptionId
                        }, transaction);
                }
            }

            if(submission.MsqAnswers.Count > 0)
            {
                foreach(var answer in submission.MsqAnswers)
                {
                    foreach(var option in answer.OptionIds)
                    {
                        await conn.ExecuteAsync(
                        QuestionSqlQueries.SubmitAnswerForMsqQuestions,
                        new
                        {
                            submission.UserId,
                            answer.QuestionId,
                            answer.QuestionType,
                            OptionId = option
                        }, transaction);
                    }
                }
            }

            if(submission.TrueFalseAnswers.Count > 0)
            {
                foreach(var answer in submission.TrueFalseAnswers)
                {
                    await conn.ExecuteAsync(
                        QuestionSqlQueries.SubmitAnswerForTfQuestions,
                        new
                        {
                            submission.UserId,
                            answer.QuestionId,
                            answer.Answer
                        }, transaction);
                }
            }

            if(submission.ShortAnswers.Count > 0)
            {
                foreach(var answer in submission.ShortAnswers)
                {
                    await conn.ExecuteAsync(
                        QuestionSqlQueries.SubmitAnswerForSaQuestions,
                        new
                        {
                            submission.UserId,
                            answer.QuestionId,
                            answer.Answer
                        }, transaction);
                }

                await conn.ExecuteAsync(QuestionSqlQueries.UpdateGroupInfoUnderReviewStatus,
                    new
                    {
                        submission.UserId,
                        submission.GroupId
                    }, transaction);
            }

            return 1;
        });
    }

    public async Task<QuestionResponseDto> GetQuestions(Guid groupId)
    {
        QuestionGroupMetadataRow? groupMetadata =
            await DbOperation(async conn =>
                await conn.QuerySingleOrDefaultAsync<QuestionGroupMetadataRow>(
                    QuestionSqlQueries.GetQuestionGroupMetadataByGroupId,
                    new { GroupId = groupId }));

        IEnumerable<McqQuestionWithOptionRow> mcqRows =
            await DbOperation(async conn =>
                await conn.QueryAsync<McqQuestionWithOptionRow>(
                    QuestionSqlQueries.GetMcqQuestionsByGroupId,
                    new { GroupId = groupId }));

        IEnumerable<MsqQuestionWithOptionRow> msqRows =
            await DbOperation(async conn =>
                await conn.QueryAsync<MsqQuestionWithOptionRow>(
                    QuestionSqlQueries.GetMsqQuestionsByGroupId,
                    new { GroupId = groupId }));

        IEnumerable<TrueFalseQuestionResponseDto> trueFalseQuestions =
            await DbOperation(async conn =>
                await conn.QueryAsync<TrueFalseQuestionResponseDto>(
                    QuestionSqlQueries.GetTrueFalseQuestionsByGroupId,
                    new { GroupId = groupId }));

        IEnumerable<ShortQuestionResponseDto> shortQuestions =
            await DbOperation(async conn =>
                await conn.QueryAsync<ShortQuestionResponseDto>(
                    QuestionSqlQueries.GetShortQuestionsByGroupId,
                    new { GroupId = groupId }));

        return new QuestionResponseDto
        {
            GroupId = groupId,
            ExamDuration = groupMetadata?.ExamDuration ?? 0,
            mcq = BuildMcqQuestions(mcqRows),
            msq = BuildMsqQuestions(msqRows),
            tf = trueFalseQuestions.ToList(),
            sa = shortQuestions.ToList()
        };
    }

    private static void ValidateMcqAnswerIndex(McqQuestionEntity mcqQuestion)
    {
        if (mcqQuestion.Options.Count == 0)
        {
            throw new ArgumentException("MCQ options cannot be empty.");
        }

        if (mcqQuestion.CorrectAnswer < 0 || mcqQuestion.CorrectAnswer >= mcqQuestion.Options.Count)
        {
            throw new ArgumentException("MCQ correct answer index is out of range.");
        }
    }

    private static QuestionIdActionModel DetermineQuestionIds(HashSet<Guid> existingQuestionIds, IEnumerable<Guid> incomingQuestionIds)
    {
        HashSet<Guid> incoming = incomingQuestionIds.ToHashSet();

        return new QuestionIdActionModel
        {
            ToUpdate = existingQuestionIds.Intersect(incoming).ToHashSet(),
            ToDelete = existingQuestionIds.Except(incoming).ToHashSet(),
            ToInsert = incoming.Except(existingQuestionIds).ToHashSet()
        };
    }

    private async Task UpdateMcqQuestions(
        QuestionEntity question,
        HashSet<Guid> existingQuestionIds,
        System.Data.IDbConnection conn,
        System.Data.IDbTransaction transaction)
    {
        Dictionary<Guid, McqQuestionEntity> incomingQuestionMap = question.mcq.ToDictionary(x => x.QuestionId);
        QuestionIdActionModel actionModel = DetermineQuestionIds(existingQuestionIds, incomingQuestionMap.Keys);

        foreach (Guid questionId in actionModel.ToUpdate)
        {
            McqQuestionEntity mcqQuestion = incomingQuestionMap[questionId];
            ValidateMcqAnswerIndex(mcqQuestion);

            await conn.ExecuteAsync(
                QuestionSqlQueries.UpdateMcqQuestion,
                new
                {
                    mcqQuestion.QuestionId,
                    QuestionGroupId = question.GroupId,
                    mcqQuestion.Type,
                    mcqQuestion.Text,
                    mcqQuestion.Points
                },
                transaction);

            await conn.ExecuteAsync(
                QuestionSqlQueries.DeleteMcqOptionsByQuestionId,
                new { mcqQuestion.QuestionId },
                transaction);

            for (int i = 0; i < mcqQuestion.Options.Count; i++)
            {
                await conn.ExecuteAsync(
                    QuestionSqlQueries.CreateMcqOption,
                    new
                    {
                        OptionId = Guid.NewGuid(),
                        mcqQuestion.QuestionId,
                        OptionText = mcqQuestion.Options[i],
                        IsCorrect = i == mcqQuestion.CorrectAnswer
                    },
                    transaction);
            }
        }

        foreach (Guid questionId in actionModel.ToInsert)
        {
            McqQuestionEntity mcqQuestion = incomingQuestionMap[questionId];
            ValidateMcqAnswerIndex(mcqQuestion);

            await conn.ExecuteAsync(
                QuestionSqlQueries.CreateMcqQuestion,
                new
                {
                    mcqQuestion.QuestionId,
                    QuestionGroupId = question.GroupId,
                    mcqQuestion.Type,
                    mcqQuestion.Text,
                    mcqQuestion.Points
                },
                transaction);

            for (int i = 0; i < mcqQuestion.Options.Count; i++)
            {
                await conn.ExecuteAsync(
                    QuestionSqlQueries.CreateMcqOption,
                    new
                    {
                        OptionId = Guid.NewGuid(),
                        mcqQuestion.QuestionId,
                        OptionText = mcqQuestion.Options[i],
                        IsCorrect = i == mcqQuestion.CorrectAnswer
                    },
                    transaction);
            }
        }

        if (actionModel.ToDelete.Count > 0)
        {
            await conn.ExecuteAsync(
                QuestionSqlQueries.DeleteMcqQuestionsByIds,
                new
                {
                    QuestionGroupId = question.GroupId,
                    QuestionIds = actionModel.ToDelete
                },
                transaction);
        }
    }

    private async Task UpdateMsqQuestions(
        QuestionEntity question,
        HashSet<Guid> existingQuestionIds,
        System.Data.IDbConnection conn,
        System.Data.IDbTransaction transaction)
    {
        Dictionary<Guid, MsqQuestionEntity> incomingQuestionMap = question.msq.ToDictionary(x => x.QuestionId);
        QuestionIdActionModel actionModel = DetermineQuestionIds(existingQuestionIds, incomingQuestionMap.Keys);

        foreach (Guid questionId in actionModel.ToUpdate)
        {
            MsqQuestionEntity msqQuestion = incomingQuestionMap[questionId];

            await conn.ExecuteAsync(
                QuestionSqlQueries.UpdateMsqQuestion,
                new
                {
                    msqQuestion.QuestionId,
                    QuestionGroupId = question.GroupId,
                    msqQuestion.Type,
                    msqQuestion.Text,
                    msqQuestion.Points
                },
                transaction);

            await conn.ExecuteAsync(
                QuestionSqlQueries.DeleteMsqOptionsByQuestionId,
                new { msqQuestion.QuestionId },
                transaction);

            HashSet<int> correctIndexes = new(msqQuestion.CorrectAnswer);

            for (int i = 0; i < msqQuestion.Options.Count; i++)
            {
                await conn.ExecuteAsync(
                    QuestionSqlQueries.CreateMsqOption,
                    new
                    {
                        OptionId = Guid.NewGuid(),
                        msqQuestion.QuestionId,
                        OptionText = msqQuestion.Options[i],
                        IsCorrect = correctIndexes.Contains(i)
                    },
                    transaction);
            }
        }

        foreach (Guid questionId in actionModel.ToInsert)
        {
            MsqQuestionEntity msqQuestion = incomingQuestionMap[questionId];

            await conn.ExecuteAsync(
                QuestionSqlQueries.CreateMsqQuestion,
                new
                {
                    msqQuestion.QuestionId,
                    QuestionGroupId = question.GroupId,
                    msqQuestion.Type,
                    msqQuestion.Text,
                    msqQuestion.Points
                },
                transaction);

            HashSet<int> correctIndexes = new(msqQuestion.CorrectAnswer);

            for (int i = 0; i < msqQuestion.Options.Count; i++)
            {
                await conn.ExecuteAsync(
                    QuestionSqlQueries.CreateMsqOption,
                    new
                    {
                        OptionId = Guid.NewGuid(),
                        msqQuestion.QuestionId,
                        OptionText = msqQuestion.Options[i],
                        IsCorrect = correctIndexes.Contains(i)
                    },
                    transaction);
            }
        }

        if (actionModel.ToDelete.Count > 0)
        {
            await conn.ExecuteAsync(
                QuestionSqlQueries.DeleteMsqQuestionsByIds,
                new
                {
                    QuestionGroupId = question.GroupId,
                    QuestionIds = actionModel.ToDelete
                },
                transaction);
        }
    }

    private async Task UpdateTrueFalseQuestions(
        QuestionEntity question,
        HashSet<Guid> existingQuestionIds,
        System.Data.IDbConnection conn,
        System.Data.IDbTransaction transaction)
    {
        Dictionary<Guid, TrueFalseQuestionEntity> incomingQuestionMap = question.tf.ToDictionary(x => x.QuestionId);
        QuestionIdActionModel actionModel = DetermineQuestionIds(existingQuestionIds, incomingQuestionMap.Keys);

        foreach (Guid questionId in actionModel.ToUpdate)
        {
            TrueFalseQuestionEntity trueFalseQuestion = incomingQuestionMap[questionId];

            await conn.ExecuteAsync(
                QuestionSqlQueries.UpdateTrueFalseQuestion,
                new
                {
                    trueFalseQuestion.QuestionId,
                    QuestionGroupId = question.GroupId,
                    trueFalseQuestion.Type,
                    trueFalseQuestion.Text,
                    trueFalseQuestion.Points,
                    trueFalseQuestion.CorrectAnswer
                },
                transaction);
        }

        foreach (Guid questionId in actionModel.ToInsert)
        {
            TrueFalseQuestionEntity trueFalseQuestion = incomingQuestionMap[questionId];

            await conn.ExecuteAsync(
                QuestionSqlQueries.CreateTrueFalseQuestion,
                new
                {
                    trueFalseQuestion.QuestionId,
                    QuestionGroupId = question.GroupId,
                    trueFalseQuestion.Type,
                    trueFalseQuestion.Text,
                    trueFalseQuestion.Points,
                    trueFalseQuestion.CorrectAnswer
                },
                transaction);
        }

        if (actionModel.ToDelete.Count > 0)
        {
            await conn.ExecuteAsync(
                QuestionSqlQueries.DeleteTrueFalseQuestionsByIds,
                new
                {
                    QuestionGroupId = question.GroupId,
                    QuestionIds = actionModel.ToDelete
                },
                transaction);
        }
    }

    private async Task UpdateShortQuestions(
        QuestionEntity question,
        HashSet<Guid> existingQuestionIds,
        System.Data.IDbConnection conn,
        System.Data.IDbTransaction transaction)
    {
        Dictionary<Guid, ShortQuestionEntity> incomingQuestionMap = question.sa.ToDictionary(x => x.QuestionId);
        QuestionIdActionModel actionModel = DetermineQuestionIds(existingQuestionIds, incomingQuestionMap.Keys);

        foreach (Guid questionId in actionModel.ToUpdate)
        {
            ShortQuestionEntity shortQuestion = incomingQuestionMap[questionId];

            await conn.ExecuteAsync(
                QuestionSqlQueries.UpdateShortAnswerQuestion,
                new
                {
                    shortQuestion.QuestionId,
                    QuestionGroupId = question.GroupId,
                    shortQuestion.Type,
                    shortQuestion.Text,
                    shortQuestion.Points
                },
                transaction);
        }

        foreach (Guid questionId in actionModel.ToInsert)
        {
            ShortQuestionEntity shortQuestion = incomingQuestionMap[questionId];

            await conn.ExecuteAsync(
                QuestionSqlQueries.CreateShortAnswerQuestion,
                new
                {
                    shortQuestion.QuestionId,
                    QuestionGroupId = question.GroupId,
                    shortQuestion.Type,
                    shortQuestion.Text,
                    shortQuestion.Points
                },
                transaction);
        }

        if (actionModel.ToDelete.Count > 0)
        {
            await conn.ExecuteAsync(
                QuestionSqlQueries.DeleteShortAnswerQuestionsByIds,
                new
                {
                    QuestionGroupId = question.GroupId,
                    QuestionIds = actionModel.ToDelete
                },
                transaction);
        }
    }

    private static List<McqQuestionResponseDto> BuildMcqQuestions(IEnumerable<McqQuestionWithOptionRow> rows)
    {
        Dictionary<Guid, McqQuestionResponseDto> groupedQuestions = new();

        foreach (McqQuestionWithOptionRow row in rows)
        {
            if (!groupedQuestions.TryGetValue(row.QuestionId, out McqQuestionResponseDto? question))
            {
                question = new McqQuestionResponseDto
                {
                    QuestionId = row.QuestionId,
                    Type = row.Type,
                    Text = row.Text,
                    Points = row.Points
                };

                groupedQuestions.Add(row.QuestionId, question);
            }

            if (row.OptionId.HasValue)
            {
                question.Options.Add(new QuestionOptionResponseDto
                {
                    OptionId = row.OptionId.Value,
                    OptionText = row.OptionText,
                    IsCorrect = row.IsCorrect
                });
            }
        }

        return groupedQuestions.Values.ToList();
    }

    private static List<MsqQuestionResponseDto> BuildMsqQuestions(IEnumerable<MsqQuestionWithOptionRow> rows)
    {
        Dictionary<Guid, MsqQuestionResponseDto> groupedQuestions = new();

        foreach (MsqQuestionWithOptionRow row in rows)
        {
            if (!groupedQuestions.TryGetValue(row.QuestionId, out MsqQuestionResponseDto? question))
            {
                question = new MsqQuestionResponseDto
                {
                    QuestionId = row.QuestionId,
                    Type = row.Type,
                    Text = row.Text,
                    Points = row.Points
                };

                groupedQuestions.Add(row.QuestionId, question);
            }

            if (row.OptionId.HasValue)
            {
                question.Options.Add(new QuestionOptionResponseDto
                {
                    OptionId = row.OptionId.Value,
                    OptionText = row.OptionText,
                    IsCorrect = row.IsCorrect
                });
            }
        }

        return groupedQuestions.Values.ToList();
    }

    private class McqQuestionWithOptionRow
    {
        public Guid QuestionId { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public int Points { get; set; }

        public Guid? OptionId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }

    private class MsqQuestionWithOptionRow
    {
        public Guid QuestionId { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public int Points { get; set; }

        public Guid? OptionId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }

    private class QuestionGroupMetadataRow
    {
        public Guid GroupId { get; set; }

        public int ExamDuration { get; set; }
    }

    private class QuestionIdActionModel
    {
        public HashSet<Guid> ToUpdate { get; set; } = new();

        public HashSet<Guid> ToDelete { get; set; } = new();

        public HashSet<Guid> ToInsert { get; set; } = new();
    }
}
