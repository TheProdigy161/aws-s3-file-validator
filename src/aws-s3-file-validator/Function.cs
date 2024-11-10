using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace aws_s3_file_validator;

public class Function
{
    public string Run(string input, ILambdaContext context)
    {
        return input.ToUpper();
    }
}
