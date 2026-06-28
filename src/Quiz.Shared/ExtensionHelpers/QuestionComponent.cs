

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Quiz.Shared.Models;

namespace Quiz.Shared.ExtensionHelpers;
public static class QuestionComponent
{
    public static void ComposeQuestionCard(this IContainer container, ExamQuestion q)
    {
        var borderColor = q.IsCorrect ? Colors.AccentGreen : Colors.Red;
        var badgeBg = q.IsCorrect ? Colors.LightGreen : Colors.LightRed;
        var badgeColor = q.IsCorrect ? Colors.AccentGreen : Colors.Red;

        container
            .Border(1).BorderColor(Colors.Border)
            .Background(Colors.White)
            .Column(col =>
            {
                // ── Main body row ──────────────────────────────────────────
                col.Item().Padding(16).Row(row =>
                {
                    // Left: question + options
                    row.RelativeItem().Column(left =>
                    {
                        // Type badge row
                        left.Item().Row(r =>
                        {
                            r.AutoItem()
                                .Background(Colors.ExtraLightGray)
                                .Border(1).BorderColor(Colors.Border)
                                .Padding(3).PaddingHorizontal(6)
                                .Text($"QUESTION {q.Number} • {q.Type}")
                                .FontSize(FontSizes.Label)
                                .FontColor(Colors.Gray);

                            r.AutoItem().PaddingLeft(6)
                                .Background(badgeBg)
                                .Border(1).BorderColor(badgeColor)
                                .Padding(3).PaddingHorizontal(6)
                                .Text(q.IsCorrect ? "CORRECT" : "WRONG")
                                .FontSize(FontSizes.Label)
                                .Bold()
                                .FontColor(badgeColor);
                        });

                        // Question text
                        left.Item().PaddingTop(8)
                            .Text(q.QuestionText)
                            .FontSize(FontSizes.Normal)
                            .SemiBold();

                        // Type-specific rendering
                        left.Item().PaddingTop(8).ComposeOptions(q);
                    });

                    // Right panel: selection summary
                    row.AutoItem()
                        .Width(155)
                        .PaddingLeft(16)
                        .Column(right =>
                        {
                            right.Item()
                                .Text("STUDENT SELECTION")
                                .FontSize(FontSizes.Label)
                                .FontColor(Colors.Gray)
                                .LetterSpacing(1);

                            if (q.Type == "SHORT ANSWER")
                            {
                                right.Item()
                                    .PaddingTop(2)
                                    .Text("Written response")
                                    .FontSize(FontSizes.Small)
                                    .Italic()
                                    .FontColor(Colors.Gray);
                            }
                            else
                            {
                                foreach (var sel in q.SelectedOptions)
                                    right.Item()
                                        .PaddingTop(2)
                                        .Text($"• {sel}")
                                        .FontSize(FontSizes.Small)
                                        .FontColor(q.IsCorrect ? Colors.AccentGreen : Colors.Red)
                                        .SemiBold();
                            }

                            right.Item()
                                .PaddingTop(8)
                                .Text("CORRECT ANSWER")
                                .FontSize(FontSizes.Label)
                                .FontColor(Colors.Gray)
                                .LetterSpacing(1);

                            foreach (var ans in q.CorrectOptions)
                                right.Item()
                                    .PaddingTop(2)
                                    .Text(ans)
                                    .FontSize(FontSizes.Small)
                                    .FontColor(Colors.BodyText);

                            // Verdict
                            right.Item()
                                .PaddingTop(10)
                                .Text(q.IsCorrect ? "[CORRECT]" : "[WRONG]")
                                .FontSize(FontSizes.Medium)
                                .Bold()
                                .FontColor(q.IsCorrect ? Colors.AccentGreen : Colors.Red);

                            right.Item()
                                .Text($"+{q.PointsAwarded} Points Awarded")
                                .FontSize(FontSizes.Label)
                                .FontColor(Colors.Gray);
                        });
                });

                // ── Banner: explanation / review tip (not for SHORT ANSWER) ──
                if (!string.IsNullOrWhiteSpace(q.StudentAnswer) && q.Type != "SHORT ANSWER")
                {
                    col.Item()
                        .BorderLeft(3).BorderColor(borderColor)
                        .Background(q.IsCorrect ? Colors.VeryLightGreen : Colors.VeryLightRed)
                        .Padding(10).PaddingLeft(14)
                        .Text(text =>
                        {
                            text.Span(q.IsCorrect ? "Explanation: " : "Review Tip: ")
                                .Bold()
                                .Italic()
                                .FontSize(FontSizes.Body);
                            text.Span(q.StudentAnswer)
                                .Italic()
                                .FontSize(FontSizes.Body)
                                .FontColor(Colors.BodyText);
                        });
                }
            });
    }

    // ── Type-aware option renderer ─────────────────────────────────────────
    private static void ComposeOptions(this IContainer container, ExamQuestion q)
    {
        switch (q.Type)
        {
            case "SHORT ANSWER":
                ComposeShortAnswer(container, q);
                break;

            case "TRUE/FALSE":
                ComposeTrueFalse(container, q);
                break;

            case "MSQ":
                ComposeMSQ(container, q);
                break;

            case "MCQ":
            default:
                ComposeMCQ(container, q);
                break;
        }
    }

    // SHORT ANSWER ── student's written text in a styled box
    private static void ComposeShortAnswer(IContainer container, ExamQuestion q)
    {
        var answer = q.SelectedOptions.Length > 0 ? q.SelectedOptions[0] : string.Empty;

        container
            .Border(1).BorderColor(Colors.Border)
            .Background(Colors.ExtraLightGray)
            .Padding(10)
            .Text($"\"{answer}\"")
            .FontSize(FontSizes.Body)
            .Italic()
            .FontColor(Colors.BodyText);
    }

    // TRUE/FALSE ── two pill buttons side by side
    private static void ComposeTrueFalse(IContainer container, ExamQuestion q)
    {
        container.Row(row =>
        {
            foreach (var opt in q.Options)
            {
                var selected = q.SelectedOptions.Contains(opt);
                var correct = q.CorrectOptions.Contains(opt);

                var borderColor = selected && correct ? Colors.AccentGreen
                                : selected && !correct ? Colors.Red
                                : Colors.Border;

                var bgColor = selected && correct ? Colors.LightGreen
                                : selected && !correct ? Colors.LightRed
                                : Colors.White;

                var textColor = selected && correct ? Colors.AccentGreen
                                : selected && !correct ? Colors.Red
                                : Colors.BodyText;

                row.AutoItem()
                    .PaddingRight(8)
                    .Border(1).BorderColor(borderColor)
                    .Background(bgColor)
                    .Padding(6).PaddingHorizontal(20)
                    .Text(opt)
                    .FontSize(FontSizes.Body)
                    .FontColor(textColor)
                    .SemiBold();
            }
        });
    }

    // MSQ ── 2-column checkbox grid
    private static void ComposeMSQ(IContainer container, ExamQuestion q)
    {
        var pairs = q.Options
            .Select((opt, i) => new { opt, i })
            .GroupBy(x => x.i / 2)
            .ToList();

        container.Column(col =>
        {
            foreach (var group in pairs)
            {
                col.Item().PaddingTop(4).Row(row =>
                {
                    foreach (var item in group)
                    {
                        var selected = q.SelectedOptions.Contains(item.opt);
                        var correct = q.CorrectOptions.Contains(item.opt);

                        var borderColor = selected && correct ? Colors.AccentGreen
                                        : selected && !correct ? Colors.Red
                                        : Colors.Border;

                        var bgColor = selected && correct ? Colors.LightGreen
                                        : selected && !correct ? Colors.LightRed
                                        : Colors.White;

                        var textColor = selected && correct ? Colors.AccentGreen
                                        : selected && !correct ? Colors.Red
                                        : Colors.BodyText;

                        row.RelativeItem().PaddingRight(6)
                            .Border(1).BorderColor(borderColor)
                            .Background(bgColor)
                            .Padding(6).PaddingHorizontal(10)
                            .Row(r =>
                            {
                                // Checkbox box
                                r.AutoItem()
                                    .Width(11).Height(11)
                                    .Border(1)
                                    .BorderColor(selected ? Colors.AccentGreen : Colors.LightGray)
                                    .Background(selected ? Colors.AccentGreen : Colors.White)
                                    .AlignCenter().AlignMiddle()
                                    .Text(selected ? "✓" : " ")
                                    .FontSize(7)
                                    .Bold()
                                    .FontColor(Colors.White);

                                r.AutoItem().PaddingLeft(6)
                                    .AlignMiddle()
                                    .Text(item.opt)
                                    .FontSize(FontSizes.Body)
                                    .FontColor(textColor)
                                    .SemiBold();
                            });
                    }

                    // Fill empty column if odd number of options in last row
                    if (group.Count() == 1)
                        row.RelativeItem();
                });
            }
        });
    }

    // MCQ ── radio-style stacked list
    private static void ComposeMCQ(IContainer container, ExamQuestion q)
    {
        container.Column(col =>
        {
            foreach (var opt in q.Options)
            {
                var selected = q.SelectedOptions.Contains(opt);
                var correct = q.CorrectOptions.Contains(opt);

                var borderColor = selected && correct ? Colors.AccentGreen
                                : selected && !correct ? Colors.Red
                                : Colors.Border;

                var bgColor = selected && correct ? Colors.LightGreen
                                : selected && !correct ? Colors.LightRed
                                : Colors.White;

                var textColor = selected && correct ? Colors.AccentGreen
                                : selected && !correct ? Colors.Red
                                : Colors.BodyText;

                col.Item().PaddingTop(4)
                    .Border(1).BorderColor(borderColor)
                    .Background(bgColor)
                    .Padding(6).PaddingHorizontal(10)
                    .Row(row =>
                    {
                        // Radio indicator
                        row.AutoItem()
                            .Width(12).Height(12)
                            .Border(1)
                            .BorderColor(selected ? (correct ? Colors.AccentGreen : Colors.Red) : Colors.LightGray)
                            .Background(selected ? (correct ? Colors.AccentGreen : Colors.Red) : Colors.White)
                            .AlignCenter().AlignMiddle()
                            .Text(selected ? "●" : "○")
                            .FontSize(6)
                            .FontColor(selected ? Colors.White : Colors.LightGray);

                        row.AutoItem().PaddingLeft(8).AlignMiddle()
                            .Text(opt)
                            .FontSize(FontSizes.Body)
                            .FontColor(textColor)
                            .SemiBold();
                    });
            }
        });
    }
}

