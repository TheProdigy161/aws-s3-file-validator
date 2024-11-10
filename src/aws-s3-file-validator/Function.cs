using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace aws_s3_file_validator;

public class Function
{
    public void Run(S3Event s3Event)
    {
        Console.WriteLine("Processing S3 Event");

        Console.WriteLine("Bucket Name: " + s3Event.Records[0].S3.Bucket.Name);

        Console.WriteLine("Object Key: " + s3Event.Records[0].S3.Object.Key);
    }
}
