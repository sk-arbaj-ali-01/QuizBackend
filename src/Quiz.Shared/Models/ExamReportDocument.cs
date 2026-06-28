using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Quiz.Shared.ExtensionHelpers;

namespace Quiz.Shared.Models;
public class ExamReportDocument : IDocument
{
    private readonly ExamReportData _data;

    public ExamReportDocument(ExamReportData data)
    {
        _data = data;
    }

    public DocumentMetadata GetMetadata() => new DocumentMetadata
    {
        Title = $"Exam Report – {_data.StudentName}",
        Author = "Exam Analysis Pro",
        Subject = _data.ExamTitle,
    };

    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x
                .FontFamily("Arial")
                .FontSize(FontSizes.Body)
                .FontColor(Colors.DarkText));

            page.Content().Column(col =>
            {
                // Navigation bar
                col.Item().ComposeNavBar();

                // Page body with padding
                col.Item().Padding(24).Column(body =>
                {
                    // Header card (student info + grade)
                    body.Item().ComposeHeaderCard(_data);

                    // Section title
                    body.Item()
                        .PaddingTop(20)
                        .PaddingBottom(4)
                        .Text("Detailed Analysis")
                        .FontSize(FontSizes.Large)
                        .SemiBold()
                        .FontColor(Colors.DarkText);

                    // Question cards
                    foreach (var question in _data.Questions)
                        body.Item()
                            .PaddingTop(10)
                            .ComposeQuestionCard(question);

                    // Final performance review
                    body.Item()
                        .PaddingTop(20)
                        .ComposeFinalReview(_data);
                });
            });

            page.Footer().ComposePageFooter();
        });
    }
}

