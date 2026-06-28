using Microsoft.Extensions.Logging;
using Quiz.BL.Abstractions;

namespace Quiz.BL.Services;
public class ResultService(
    ILogger<ResultService> logger)
    : BaseService, IResultService
{
}
