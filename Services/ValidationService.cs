using aws_s3_file_validator.Models;

namespace aws_s3_file_validator.Services;

public class ValidationService
{
    private ValidationModel _validationModel { get; set; }

    public ValidationService(ValidationModel validationModel)
    {
        _validationModel = validationModel;
    }

    public bool IsValid(string content)
    {
        return _validationModel.IsValid(content);
    }
}