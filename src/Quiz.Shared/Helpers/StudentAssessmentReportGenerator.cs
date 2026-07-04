using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Quiz.Shared.Enums;

// NuGet: QuestPDF (Community license is fine for this use case)
// QuestPDF.Settings.License = LicenseType.Community; // set this once at app startup

namespace Quiz.Shared.Helpers
{
    #region Models

    public class Question
    {
        public int Number { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string Text { get; set; } = "";

        // Used by MCQ / MSQ to render the option grid
        public List<string> Options { get; set; } = new();

        // The "ground truth" — for MCQ/TF this has 1 item, for MSQ it can have many.
        // Not used for SA.
        public List<string> CorrectAnswers { get; set; } = new();

        // What the student actually picked — for MCQ/TF 1 item, for MSQ many.
        // For SA this is ignored; use StudentAnswerText instead.
        public List<string> StudentAnswers { get; set; } = new();

        // Free-text answer, only used for SA questions
        public string StudentAnswerText { get; set; } = "";

        public bool IsCorrect { get; set; }
        public int PointsAwarded { get; set; }
    }

    public class ReportData
    {
        public string ReportLabel { get; set; } = "STUDENT ASSESSMENT REPORT";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public string CandidateName { get; set; } = "";
        public string CandidateEmail { get; set; } = "";
        public DateTime AttemptedDate { get; set; }

        public int TotalPoints { get; set; }
        public int PointsReceived { get; set; }
        public string Grade { get; set; } = "";
        public string Remarks { get; set; } = "";

        public List<Question> Questions { get; set; } = new();
    }

    #endregion

    #region Theme

    internal static class Theme
    {
        public static readonly string Ink = "#0F172A";       // near-black navy text
        public static readonly string SubText = "#64748B";   // muted gray text
        public static readonly string FaintText = "#94A3B8"; // labels
        public static readonly string Border = "#E2E8F0";     // card borders
        public static readonly string PanelBg = "#F8FAFC";    // light gray panel bg
        public static readonly string Green = "#15803D";      // correct green
        public static readonly string GreenBg = "#F0FDF4";
        public static readonly string GreenBorder = "#86EFAC";
        public static readonly string Red = "#DC2626";        // wrong red
        public static readonly string RedBg = "#FEF2F2";
        public static readonly string RedBorder = "#FCA5A5";
        public static readonly string BadgeBg = "#F1F5F9";
        public static readonly string Navy = "#0F172A";
    }

    #endregion

    public class StudentReportDocument : IDocument
    {
        private readonly ReportData _data;

        public StudentReportDocument(ReportData data) => _data = data;

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(28);
                page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri).FontSize(10).FontColor(Theme.Ink));

                page.Content().Column(col =>
                {
                    col.Spacing(18);
                    col.Item().Element(ComposeHeaderCard);
                    col.Item().Element(ComposeAnalysisSection);
                    col.Item().Element(ComposeFinalResult);
                });
            });
        }

        // ---------------------------------------------------------------
        // HEADER CARD
        // ---------------------------------------------------------------
        private void ComposeHeaderCard(IContainer container)
        {
            container
                .Border(1).BorderColor(Theme.Border)
                .Padding(20)
                .Row(row =>
                {
                    row.RelativeItem(3).Column(c =>
                    {
                        c.Spacing(4);
                        c.Item().Text(_data.ReportLabel)
                            .FontSize(8).Bold().FontColor(Theme.SubText).LetterSpacing(0.08f);

                        c.Item().PaddingTop(2).Text(_data.Title)
                            .FontSize(20).Bold().FontColor(Theme.Ink);

                        c.Item().PaddingTop(2).Text(_data.Description)
                            .FontSize(9.5f).FontColor(Theme.SubText);

                        c.Item().PaddingTop(12).Row(info =>
                        {
                            info.RelativeItem().Column(cc =>
                            {
                                cc.Item().Text("CANDIDATE").FontSize(7.5f).Bold()
                                    .FontColor(Theme.FaintText).LetterSpacing(0.06f);
                                cc.Item().Text(_data.CandidateName).FontSize(10).Bold();
                                cc.Item().Text(_data.CandidateEmail).FontSize(8.5f).FontColor(Theme.SubText);
                            });

                            info.RelativeItem().Column(cc =>
                            {
                                cc.Item().Text("ATTEMPTED DATE").FontSize(7.5f).Bold()
                                    .FontColor(Theme.FaintText).LetterSpacing(0.06f);
                                cc.Item().Text(_data.AttemptedDate.ToString("MMMM d, yyyy")).FontSize(10).Bold();
                            });
                        });
                    });

                    row.ConstantItem(150).Background(Theme.PanelBg).Border(1).BorderColor(Theme.Border)
                        .Padding(14).Column(g =>
                        {
                            g.Spacing(4);
                            g.Item().AlignCenter().Text("FINAL GRADE").FontSize(7.5f).Bold()
                                .FontColor(Theme.FaintText).LetterSpacing(0.06f);

                            g.Item().AlignCenter().Text(_data.Grade)
                                .FontSize(28).Bold().FontColor(Theme.Navy);

                            var pct = _data.TotalPoints == 0
                                ? 0
                                : Math.Clamp((float)_data.PointsReceived / _data.TotalPoints, 0, 1);

                            g.Item().PaddingTop(4).Height(5).Row(bar =>
                            {
                                bar.RelativeItem(pct).Background(Theme.Green);
                                bar.RelativeItem(1 - pct).Background(Theme.Border);
                            });

                            g.Item().AlignCenter().PaddingTop(2)
                                .Text($"{_data.PointsReceived} / {_data.TotalPoints} Points")
                                .FontSize(8).FontColor(Theme.SubText);
                        });
                });
        }

        // ---------------------------------------------------------------
        // DETAILED ANALYSIS SECTION
        // ---------------------------------------------------------------
        private void ComposeAnalysisSection(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(12);

                col.Item().Text("Detailed Analysis").FontSize(13).Bold().FontColor(Theme.Ink);

                foreach (var q in _data.Questions)
                    col.Item().Element(c => ComposeQuestionCard(c, q));
            });
        }

        private void ComposeQuestionCard(IContainer container, Question q)
        {
            container.Border(1).BorderColor(Theme.Border).Row(row =>
            {
                // ---- LEFT: question body -------------------------------------------------
                row.RelativeItem(2).Padding(16).Column(left =>
                {
                    left.Spacing(8);

                    left.Item().Row(head =>
                    {
                        head.AutoItem().Background(Theme.BadgeBg).Padding(4)
                            .Text($"QUESTION {q.Number:00} • {TypeLabel(q.Type)}")
                            .FontSize(7.5f).Bold().FontColor(Theme.SubText).LetterSpacing(0.04f);

                        head.ConstantItem(6);

                        var (badgeText, badgeColor, badgeBg, badgeBorder) = q.IsCorrect
                            ? ("CORRECT", Theme.Green, Theme.GreenBg, Theme.GreenBorder)
                            : ("WRONG", Theme.Red, Theme.RedBg, Theme.RedBorder);

                        head.AutoItem().Background(badgeBg).Border(1).BorderColor(badgeBorder).Padding(4)
                            .Text(badgeText).FontSize(7.5f).Bold().FontColor(badgeColor);
                    });

                    left.Item().Text(q.Text).FontSize(10.5f).FontColor(Theme.Ink);

                    switch (q.Type)
                    {
                        case QuestionTypeEnum.MCQ:
                            left.Item().Element(c => ComposeOptionList(c, q, multiSelect: false));
                            break;
                        case QuestionTypeEnum.MSQ:
                            left.Item().Element(c => ComposeOptionList(c, q, multiSelect: true));
                            break;
                        case QuestionTypeEnum.TF:
                            // No option grid — matches the reference design (question text only)
                            break;
                        case QuestionTypeEnum.SA:
                            left.Item().Background(Theme.PanelBg).Border(1).BorderColor(Theme.Border)
                                .Padding(10)
                                .Text($"\"{q.StudentAnswerText}\"")
                                .FontSize(9.5f).Italic().FontColor(Theme.Ink);
                            break;
                    }
                });

                // ---- divider ----
                row.ConstantItem(1).Background(Theme.Border);

                // ---- RIGHT: answer / verdict panel ----------------------------------------
                row.RelativeItem(1).Background(Theme.PanelBg).Padding(16)
                    .Element(c => ComposeAnswerPanel(c, q));
            });
        }

        private void ComposeOptionList(IContainer container, Question q, bool multiSelect)
        {
            container.PaddingTop(4).Column(list =>
            {
                list.Spacing(6);

                // Render two-per-row for MSQ (checkbox grid), one-per-row for MCQ (radio list)
                if (multiSelect)
                {
                    for (int i = 0; i < q.Options.Count; i += 2)
                    {
                        list.Item().Row(r =>
                        {
                            r.Spacing(8);
                            r.RelativeItem().Element(c => ComposeOptionBox(c, q, q.Options[i], multiSelect));
                            if (i + 1 < q.Options.Count)
                                r.RelativeItem().Element(c => ComposeOptionBox(c, q, q.Options[i + 1], multiSelect));
                            else
                                r.RelativeItem();
                        });
                    }
                }
                else
                {
                    foreach (var opt in q.Options)
                        list.Item().Element(c => ComposeOptionBox(c, q, opt, multiSelect));
                }
            });
        }

        private void ComposeOptionBox(IContainer container, Question q, string option, bool multiSelect)
        {
            bool isCorrectOption = q.CorrectAnswers.Contains(option);
            bool isStudentPick = q.StudentAnswers.Contains(option);

            // Determine visual state: green if correct & picked, red if incorrectly picked,
            // subtle green outline if correct-but-unpicked (rare), else neutral gray.
            string borderColor = Theme.Border;
            string bg = Colors.White;
            string textColor = Theme.SubText;

            if (isStudentPick && isCorrectOption)
            {
                borderColor = Theme.GreenBorder;
                bg = Theme.GreenBg;
                textColor = Theme.Green;
            }
            else if (isStudentPick && !isCorrectOption)
            {
                borderColor = Theme.RedBorder;
                bg = Theme.RedBg;
                textColor = Theme.Red;
            }

            container.Border(1).BorderColor(borderColor).Background(bg).Padding(8).Row(r =>
            {
                r.Spacing(6);
                r.AutoItem().Text(isStudentPick ? (multiSelect ? "☑" : "●") : (multiSelect ? "☐" : "○"))
                    .FontSize(10).FontColor(textColor);
                r.RelativeItem().Text(option).FontSize(9.5f).FontColor(textColor)
                    .Bold();
            });
        }

        private void ComposeAnswerPanel(IContainer container, Question q)
        {
            container.Column(panel =>
            {
                panel.Spacing(10);

                if (q.Type == QuestionTypeEnum.SA)
                {
                    // Short answer: no canonical "correct answer" line — just verdict + status
                    var (vText, vColor) = q.IsCorrect ? ("[CORRECT]", Theme.Green) : ("[WRONG]", Theme.Red);
                    panel.Item().Text(vText).FontSize(12).Bold().FontColor(vColor);
                    panel.Item().PaddingTop(4).LineHorizontal(1).LineColor(Theme.Border);
                    panel.Item().Text("GRADING STATUS").FontSize(7.5f).Bold()
                        .FontColor(Theme.FaintText).LetterSpacing(0.05f);
                    panel.Item().Text(q.IsCorrect ? "✓ Verified Correct" : "✗ Verified Incorrect")
                        .FontSize(9.5f).Bold().FontColor(q.IsCorrect ? Theme.Green : Theme.Red);
                    return;
                }

                // MCQ / MSQ / TF share the same panel shape
                panel.Item().Text(q.Type == QuestionTypeEnum.MSQ ? "CORRECT ANSWERS" : "CORRECT ANSWER")
                    .FontSize(7.5f).Bold().FontColor(Theme.FaintText).LetterSpacing(0.05f);
                panel.Item().Text(string.Join(", ", q.CorrectAnswers)).FontSize(10).Bold();

                panel.Item().Text(q.Type == QuestionTypeEnum.MSQ ? "STUDENT SELECTION" : "STUDENT INPUT")
                    .FontSize(7.5f).Bold().FontColor(Theme.FaintText).LetterSpacing(0.05f);

                if (q.Type == QuestionTypeEnum.MSQ)
                {
                    panel.Item().Column(sel =>
                    {
                        sel.Spacing(2);
                        foreach (var a in q.StudentAnswers)
                            sel.Item().Text($"•  {a}").FontSize(9.5f).Bold().FontColor(Theme.Green);
                    });
                }
                else
                {
                    var studentText = q.StudentAnswers.FirstOrDefault() ?? "-";
                    var icon = q.IsCorrect ? "✓" : "✗";
                    var color = q.IsCorrect ? Theme.Green : Theme.Red;
                    panel.Item().Text($"{icon}  {studentText}").FontSize(9.5f).Bold().FontColor(color);
                }

                panel.Item().PaddingTop(4).LineHorizontal(1).LineColor(Theme.Border);

                var (verdictText, verdictColor) = q.IsCorrect ? ("[CORRECT]", Theme.Green) : ("[WRONG]", Theme.Red);
                panel.Item().Text(verdictText).FontSize(12).Bold().FontColor(verdictColor);
                panel.Item().Text($"+{q.PointsAwarded} Points Awarded").FontSize(8).FontColor(Theme.SubText);
            });
        }

        private static string TypeLabel(QuestionTypeEnum t) => t switch
        {
            QuestionTypeEnum.MCQ => "MCQ",
            QuestionTypeEnum.MSQ => "MSQ",
            QuestionTypeEnum.TF => "TRUE/FALSE",
            QuestionTypeEnum.SA => "SHORT ANSWER",
            _ => t.ToString()
        };

        // ---------------------------------------------------------------
        // FINAL RESULT
        // ---------------------------------------------------------------
        private void ComposeFinalResult(IContainer container)
        {
            container.Column(col =>
            {
                col.Spacing(10);
                col.Item().Text("Final Result").FontSize(13).Bold().FontColor(Theme.Ink);

                col.Item().Border(1).BorderColor(Theme.Border).Padding(18).Column(inner =>
                {
                    inner.Spacing(10);

                    inner.Item().Row(r =>
                    {
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("TOTAL POINTS").FontSize(7.5f).Bold()
                                .FontColor(Theme.FaintText).LetterSpacing(0.05f);
                            c.Item().Text(_data.TotalPoints.ToString()).FontSize(16).Bold();
                        });

                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("POINTS RECEIVED").FontSize(7.5f).Bold()
                                .FontColor(Theme.FaintText).LetterSpacing(0.05f);
                            c.Item().Text(_data.PointsReceived.ToString()).FontSize(16).Bold();
                        });

                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("GRADE").FontSize(7.5f).Bold()
                                .FontColor(Theme.FaintText).LetterSpacing(0.05f);
                            c.Item().Text(_data.Grade).FontSize(16).Bold().FontColor(Theme.Green);
                        });
                    });

                    inner.Item().LineHorizontal(1).LineColor(Theme.Border);

                    inner.Item().Column(c =>
                    {
                        c.Item().Text("REMARKS").FontSize(7.5f).Bold()
                            .FontColor(Theme.FaintText).LetterSpacing(0.05f);
                        c.Item().PaddingTop(2).Text($"\"{_data.Remarks}\"")
                            .FontSize(9.5f).Italic().FontColor(Theme.SubText);
                    });
                });
            });
        }
    }

    #region Sample usage

    public static class Program
    {
        public static void Main()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var data = new ReportData
            {
                Title = "Advanced Data Structures & Algorithms",
                Description = "Comprehensive final evaluation of semester topics including complexity analysis and graph theory.",
                CandidateName = "Alex Johnson",
                CandidateEmail = "alex.johnson@university.edu",
                AttemptedDate = new DateTime(2023, 10, 24),
                TotalPoints = 100,
                PointsReceived = 85,
                Grade = "A+",
                Remarks = "Excellent performance in algorithmic complexity. Review sorting algorithm edge cases to reach 100% mastery.",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Number = 1,
                        Type = QuestionTypeEnum.MCQ,
                        Text = "What is the time complexity of searching in a balanced Binary Search Tree?",
                        Options = new() { "O(1)", "O(n)", "O(log n)", "O(n log n)" },
                        CorrectAnswers = new() { "O(log n)" },
                        StudentAnswers = new() { "O(log n)" },
                        IsCorrect = true,
                        PointsAwarded = 25
                    },
                    new Question
                    {
                        Number = 2,
                        Type = QuestionTypeEnum.MSQ,
                        Text = "Which of the following are NP-Complete problems?",
                        Options = new() { "Knapsack Problem", "Traveling Salesperson", "Merge Sort", "Dijkstra's" },
                        CorrectAnswers = new() { "Knapsack Problem", "Traveling Salesperson" },
                        StudentAnswers = new() { "Knapsack Problem", "Traveling Salesperson" },
                        IsCorrect = true,
                        PointsAwarded = 25
                    },
                    new Question
                    {
                        Number = 3,
                        Type = QuestionTypeEnum.TF,
                        Text = "QuickSort has a worst-case complexity of O(n^2).",
                        CorrectAnswers = new() { "True" },
                        StudentAnswers = new() { "False" },
                        IsCorrect = false,
                        PointsAwarded = 0
                    },
                    new Question
                    {
                        Number = 4,
                        Type = QuestionTypeEnum.SA,
                        Text = "Explain the main advantage of a Hash Map.",
                        StudentAnswerText = "Hash maps provide constant time complexity O(1) for average case search, insert, and delete operations.",
                        IsCorrect = true,
                        PointsAwarded = 25
                    }
                }
            };

            Document.Create(container => new StudentReportDocument(data).Compose(container))
                .GeneratePdf("StudentAssessmentReport.pdf");
        }
    }

    #endregion
}