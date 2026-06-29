namespace Quiz.Shared.Models;
public record ExamQuestion(
    string Number,
    string Type,             // "MCQ" | "MSQ" | "TRUE/FALSE" | "SHORT ANSWER"
    bool IsCorrect,
    string QuestionText,
    string[] Options,        // Empty for SHORT ANSWER
    string[] SelectedOptions,// For SHORT ANSWER: student's written text in [0]
    string[] CorrectOptions, // For SHORT ANSWER: e.g. ["Verified Correct"]
    string StudentAnswer,    // Explanation text or review tip shown in banner
    int PointsAwarded,
    int MaxPoints
);

public record ExamReportData(
    string StudentName,
    string StudentEmail,
    string ExamTitle,
    string ExamDescription,
    string AttemptedDate,
    int TotalPoints,
    int ReceivedPoints,
    string Grade,
    string PerformanceSummary,
    string Remarks,
    List<ExamQuestion> Questions
);

