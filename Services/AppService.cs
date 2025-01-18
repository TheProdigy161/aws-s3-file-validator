using aws_s3_file_validator.Utils;
using aws_s3_file_validator.Models;
using Microsoft.Extensions.Logging;
using aws_s3_file_validator.Services;
using System.Diagnostics;

namespace aws_s3_file_validator;

public class AppService
{
    private AppSettings _appSettings { get; set; }
    private ValidationService _validationService { get; set; }
    private ILogger<AppService> _logger { get; set; }
    private S3Service _s3Service { get; set; }

    public AppService(AppSettings appSettings, ILogger<AppService> logger, ValidationService validationService, S3Service s3Service)
    {
        _appSettings = appSettings;
        _logger = logger;
        _s3Service = s3Service;
        _validationService = validationService;
    }

    public async Task Run(string bucketName, string keyName)
    {
        Performance.Start();

        await _s3Service.DownloadFile(bucketName, keyName);

        string filePath = $"{_appSettings.TempFolder}/{keyName}";

        using (StreamReader reader = new StreamReader(filePath))
        {   
            while (!reader.EndOfStream)
            {
                string? content = await reader.ReadLineAsync();

                if (content == null)
                {
                    break;
                }

                StatsService.IncrementTotalLineCount();

                if (_validationService.IsValid(content))
                {
                    StatsService.IncrementValidLineCount();
                }
                else
                {
                    StatsService.IncrementInvalidLineCount();
                }

                if (StatsService.TotalLineCount % 250_000 == 0)
                {
                    _logger.LogInformation($"Processed {StatsService.TotalLineCount:n0} lines");
                }
            }
        }

        Performance.Stop();
        _logger.LogInformation($"Processed {StatsService.TotalLineCount:n0} lines in {Performance.GetTimeTaken()}");
        ClearFile(filePath);
    }

    public void ClearFile(string filePath)
    {
        File.Delete(filePath);
    }
}