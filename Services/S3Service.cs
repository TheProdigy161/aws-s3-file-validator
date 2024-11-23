using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;

namespace aws_s3_file_validator.Services;

public class S3Service
{
    private readonly TransferUtility _fileTransferUtility;
    private readonly AppSettings _appSettings;
    private readonly ILogger<S3Service> _logger;

    public S3Service(AppSettings appSettings, ILogger<S3Service> logger)
    {   
        _appSettings = appSettings;
        _logger = logger;

        _fileTransferUtility = new TransferUtility
        (
            new AmazonS3Client
            (
                RegionEndpoint.GetBySystemName(_appSettings.AwsRegion ?? "eu-west-1")
            )
        );
    }

    public async Task DownloadFile(string bucketName, string keyName)
    {
        try
        {
            _logger.LogInformation($"Downloading file from S3 bucket: {bucketName} with key: {keyName}");

            await _fileTransferUtility.DownloadAsync
            (
                $"{_appSettings.TempFolder}/{keyName}",
                bucketName,
                keyName
            );

            _logger.LogInformation($"File downloaded to: {_appSettings.TempFolder}/{keyName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while downloading: {ex.Message}");
        }
    }
}