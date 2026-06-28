using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Quiz.Shared.Models;

namespace Quiz.Shared.ExtensionHelpers;
public static class HeaderComponent
{
    public static void ComposeNavBar(this IContainer container)
    {
        container
            .Background(Colors.White)
            .BorderBottom(1).BorderColor(Colors.Border)
            .Padding(12).PaddingHorizontal(24)
            .Row(row =>
            {
                row.AutoItem()
                    .AlignMiddle()
                    .Text("Exam Analysis Pro")
                    .FontSize(FontSizes.Large)
                    .Bold()
                    .FontColor(Colors.DarkGreen);

                row.RelativeItem()
                    .AlignMiddle()
                    .PaddingLeft(24)
                    .Row(nav =>
                    {
                        foreach (var (label, active) in new[]
                        {
                            ("Dashboard", true),
                            ("Detailed Report", false),
                            ("Performance Metrics", false),
                            ("Syllabus Coverage", false),
                        })
                        {
                            nav.AutoItem()
                                .PaddingRight(20)
                                .AlignMiddle()
                                .Text(label)
                                .FontSize(FontSizes.Body)
                                .FontColor(active ? Colors.DarkGreen : Colors.Gray)
                                .SemiBold();
                        }
                    });

                row.AutoItem()
                    .AlignMiddle()
                    .Background(Colors.DarkGreen)
                    .Padding(7).PaddingHorizontal(14)
                    .Text("⬇  Download PDF")
                    .FontSize(FontSizes.Small)
                    .Bold()
                    .FontColor(Colors.White);
            });
    }

    public static void ComposeHeaderCard(this IContainer container, ExamReportData data)
    {
        container
            .Border(1).BorderColor(Colors.Border)
            .Background(Colors.White)
            .Padding(20)
            .Row(row =>
            {
                // Left: exam info
                row.RelativeItem().Column(col =>
                {
                    col.Item()
                        .Text("STUDENT ASSESSMENT REPORT")
                        .FontSize(FontSizes.Label)
                        .FontColor(Colors.Gray)
                        .LetterSpacing(1);

                    col.Item()
                        .PaddingTop(4)
                        .Text(data.ExamTitle)
                        .FontSize(FontSizes.XLarge)
                        .Bold()
                        .FontColor(Colors.DarkText);

                    col.Item()
                        .PaddingTop(6)
                        .Text(data.ExamDescription)
                        .FontSize(FontSizes.Small)
                        .FontColor(Colors.Gray);

                    col.Item().PaddingTop(14).Row(r =>
                    {
                        r.AutoItem().Column(c =>
                        {
                            c.Item().Text("CANDIDATE")
                                .FontSize(FontSizes.Label)
                                .FontColor(Colors.Gray)
                                .LetterSpacing(1);
                            c.Item().PaddingTop(2)
                                .Text(data.StudentName)
                                .FontSize(FontSizes.Body)
                                .SemiBold();
                            c.Item().Text(data.StudentEmail)
                                .FontSize(FontSizes.Small)
                                .FontColor(Colors.Gray);
                        });

                        r.AutoItem().PaddingLeft(28).Column(c =>
                        {
                            c.Item().Text("ATTEMPTED DATE")
                                .FontSize(FontSizes.Label)
                                .FontColor(Colors.Gray)
                                .LetterSpacing(1);
                            c.Item().PaddingTop(2)
                                .Text(data.AttemptedDate)
                                .FontSize(FontSizes.Body)
                                .SemiBold();
                        });
                    });
                });

                // Right: grade card
                row.AutoItem()
                    .Width(130)
                    .Border(1).BorderColor(Colors.Border)
                    .Padding(14)
                    .Column(col =>
                    {
                        col.Item()
                            .AlignCenter()
                            .Text("FINAL GRADE")
                            .FontSize(FontSizes.Label)
                            .FontColor(Colors.Gray)
                            .LetterSpacing(1);

                        col.Item()
                            .AlignCenter()
                            .PaddingTop(2)
                            .Text(data.Grade)
                            .FontSize(FontSizes.Grade)
                            .Bold()
                            .FontColor(Colors.DarkText);

                        // Progress bar
                        float pct = (float)data.ReceivedPoints / data.TotalPoints;
                        col.Item().PaddingTop(6).Column(bar =>
                        {
                            bar.Item().Height(5).Background(Colors.Border).Column(inner =>
                                inner.Item()
                                    .Width(pct * 110)   // 110 ≈ inner width after padding
                                    .Height(5)
                                    .Background(Colors.AccentGreen));
                        });

                        col.Item()
                            .PaddingTop(5)
                            .AlignCenter()
                            .Text($"{data.ReceivedPoints} / {data.TotalPoints} Points")
                            .FontSize(FontSizes.Small)
                            .FontColor(Colors.Gray);
                    });
            });
    }
}
