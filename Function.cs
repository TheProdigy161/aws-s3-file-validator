using System.Diagnostics;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using aws_s3_file_validator.Models;
using aws_s3_file_validator.Validators;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace aws_s3_file_validator;

public class Function
{
    public async Task Run(S3Event s3Event)
    {
        int totalLineCount = 0;
        int validLineCount = 0;
        int invalidLineCount = 0;

        Stopwatch sw = Stopwatch.StartNew();

        try
        {
            Console.WriteLine("Processing S3 Event");

            if (s3Event == null || s3Event?.Records?.Count == 0)
            {
                Console.WriteLine("No records found in the S3 event");
                return;
            }

            string bucketName = s3Event.Records[0].S3.Bucket.Name;
            string keyName = s3Event.Records[0].S3.Object.Key;

            Console.WriteLine("Bucket Name: " + bucketName);
            Console.WriteLine("Object Key: " + keyName);

            AmazonS3Client client = new AmazonS3Client();

            GetObjectResponse objectResponse = await client.GetObjectAsync(bucketName, keyName);

            using (Stream responseStream = objectResponse.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {   
                while (!reader.EndOfStream)
                {
                    string? content = await reader.ReadLineAsync();

                    Dog dog = new Dog(content);

                    if (dog == null)
                    {
                        break;
                    }

                    totalLineCount++;

                    if (dog.IsValid())
                    {
                        validLineCount++;
                    }
                    else
                    {
                        invalidLineCount++;
                    }

                    if (totalLineCount % 50000 == 0)
                    {
                        Console.WriteLine($"Processed {totalLineCount:n0} lines");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            sw.Stop();
            Console.WriteLine($"Execution Time: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Total Lines: {totalLineCount:n0}");
            Console.WriteLine($"Valid Lines: {validLineCount:n0}");
            Console.WriteLine($"Invalid Lines: {invalidLineCount:n0}");
        }
    }
}
