using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Quiz.Shared.Models;

namespace Quiz.Shared.ExtensionHelpers;
public static class FooterComponent
{
    public static void ComposeFinalReview(this IContainer container, ExamReportData data)
    {
        container
            .Background(Colors.DarkGreen)
            .Padding(24)
            .Row(row =>
            {
                // Left: summary + remarks
                row.RelativeItem().Column(col =>
                {
                    col.Item()
                        .Text("Final Performance Review")
                        .FontSize(FontSizes.Large)
                        .Bold()
                        .FontColor(Colors.White);

                    col.Item().PaddingTop(6)
                        .Text(data.PerformanceSummary)
                        .FontSize(FontSizes.Body)
                        .FontColor("#A7F3D0");

                    col.Item().PaddingTop(14)
                        .Background("#FFFFFF10")   // ~6% white overlay
                        .Padding(12)
                        .Column(remarks =>
                        {
                            remarks.Item()
                                .Text("REMARKS")
                                .FontSize(FontSizes.Label)
                                .FontColor("#A7F3D0")
                                .LetterSpacing(1);

                            remarks.Item().PaddingTop(4)
                                .Text($"\"{data.Remarks}\"")
                                .FontSize(FontSizes.Body)
                                .Italic()
                                .FontColor(Colors.White);
                        });
                });

                // Right: score boxes + grade
                row.AutoItem().Width(190).PaddingLeft(20).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        // Total Points box
                        r.RelativeItem()
                            .Background("#FFFFFF18")
                            .Padding(12)
                            .AlignCenter()
                            .Column(c =>
                            {
                                c.Item()
                                    .AlignCenter()
                                    .Text("TOTAL POINTS")
                                    .FontSize(FontSizes.Label)
                                    .FontColor("#A7F3D0")
                                    .LetterSpacing(1);
                                c.Item()
                                    .AlignCenter()
                                    .Text(data.TotalPoints.ToString())
                                    .FontSize(22)
                                    .Bold()
                                    .FontColor(Colors.White);
                            });

                        r.AutoItem().Width(6);

                        // Received Points box
                        r.RelativeItem()
                            .Background("#FFFFFF18")
                            .Padding(12)
                            .Column(c =>
                            {
                                c.Item()
                                    .AlignCenter()
                                    .Text("RECEIVED")
                                    .FontSize(FontSizes.Label)
                                    .FontColor("#A7F3D0")
                                    .LetterSpacing(1);
                                c.Item()
                                    .AlignCenter()
                                    .Text(data.ReceivedPoints.ToString())
                                    .FontSize(22)
                                    .Bold()
                                    .FontColor(Colors.White);
                            });
                    });

                    // Grade button
                    col.Item().PaddingTop(8)
                        .Background(Colors.AccentGreen)
                        .Padding(14)
                        .AlignCenter()
                        .Text($"GRADE: {data.Grade}")
                        .FontSize(FontSizes.Large)
                        .Bold()
                        .FontColor(Colors.White);
                });
            });
    }

    public static void ComposePageFooter(this IContainer container)
    {
        container
            .BorderTop(1).BorderColor(Colors.Border)
            .Background(Colors.White)
            .Padding(10).PaddingHorizontal(24)
            .Column(col =>
            {
                col.Item().AlignCenter()
                    .Text("ACADEMIC ASSESSMENT AUTHORITY")
                    .FontSize(FontSizes.Label)
                    .Bold()
                    .FontColor(Colors.LightGray)
                    .LetterSpacing(1);

                col.Item().AlignCenter()
                    .Text("© 2024 Academic Assessment Authority. All rights reserved.")
                    .FontSize(FontSizes.Label)
                    .FontColor(Colors.LightGray);
            });
    }
}
