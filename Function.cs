using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using aws_s3_file_validator.Models;
using aws_s3_file_validator.Services;
using aws_s3_file_validator.Utils;
using DotNetEnv.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace aws_s3_file_validator;

public class Function
{   
    private IServiceCollection _services = new ServiceCollection();
    private IServiceProvider _serviceProvider;

    public Function()
    {
        ConfigureServices();

        if (_serviceProvider == null)
        {
            Console.WriteLine("Service Provider is null");
            Environment.Exit(1);
        }
    }

    [Performance]
    public async Task Run(S3Event s3Event)
    {
        try
        {
            // Check if the S3 event is null or if there are no records in the event
            if (s3Event == null || s3Event.Records.Count == 0)
            {
                Console.WriteLine("No records found in the S3 event");
                return;
            }

            string bucketName = s3Event.Records[0].S3.Bucket.Name;
            string keyName = s3Event.Records[0].S3.Object.Key;

            Console.WriteLine($"Bucket Name: {bucketName}, Object Key: {keyName}");

            AppService appService = _serviceProvider.GetRequiredService<AppService>();

            if (appService is null)
            {
                Console.WriteLine("AppService is null");
                return; 
            }
            
            await appService.Run(bucketName, keyName);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            StatsService.PrintStats();
        }
    }

    private void ConfigureServices()
    {
        DotNetEnv.Env.Load();
        
        string validationModelJson = Environment.GetEnvironmentVariable("ValidationModel");
        
        ValidationModel validationModel = new ValidationModel(validationModelJson);

        _services.AddSingleton(validationModel);
        _services.AddTransient<AppService>();
        _services.AddTransient<ValidationService>();

        _serviceProvider = _services.BuildServiceProvider();
    }
}
