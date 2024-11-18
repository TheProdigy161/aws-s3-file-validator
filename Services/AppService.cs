using aws_s3_file_validator.Utils;
using Amazon.S3;
using Amazon.S3.Model;
using aws_s3_file_validator.Models;

namespace aws_s3_file_validator;

public class AppService
{
    private ValidationModel _validationModel { get; set; }

    public AppService(ValidationModel validationModel)
    {
        _validationModel = validationModel;
    }

    [Performance]
    public async Task Run(string bucketName, string keyName)
    {
        AmazonS3Client client = new AmazonS3Client();

        GetObjectResponse objectResponse = await client.GetObjectAsync(bucketName, keyName);

        using (Stream responseStream = objectResponse.ResponseStream)
        using (StreamReader reader = new StreamReader(responseStream))
        {   
            while (!reader.EndOfStream)
            {
                string? content = await reader.ReadLineAsync();

                if (content == null)
                {
                    break;
                }

                StatsService.IncrementTotalLineCount();

                if (_validationModel.IsValid(content))
                {
                    StatsService.IncrementValidLineCount();
                }
                else
                {
                    StatsService.IncrementInvalidLineCount();
                }

                if (StatsService.TotalLineCount % 50000 == 0)
                {
                    Console.WriteLine($"Processed {StatsService.TotalLineCount:n0} lines");
                }
            }
        }
    }
}