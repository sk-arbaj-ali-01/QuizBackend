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
            .Column(col =>
            {
                col.Item()
                    .Text("Exam Analysis Pro")
                    .FontSize(FontSizes.Large)
                    .Bold()
                    .FontColor(Colors.DarkGreen);

                col.Item().PaddingTop(4)
                    .Text("Dashboard • Detailed Report • Performance Metrics • Syllabus Coverage")
                    .FontSize(FontSizes.Small)
                    .FontColor(Colors.Gray);
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

                        var safeTotalPoints = data.TotalPoints <= 0 ? 1 : data.TotalPoints;
                        var pct = Math.Clamp((float)data.ReceivedPoints / safeTotalPoints, 0f, 1f);

                        col.Item().PaddingTop(6).Column(bar =>
                        {
                            bar.Item().Height(5).Background(Colors.Border).Column(inner =>
                                inner.Item()
                                    .Width(pct * 110)
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
