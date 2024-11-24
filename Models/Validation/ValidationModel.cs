using Newtonsoft.Json;

namespace aws_s3_file_validator.Models;

public class ValidationModel
{
    private List<ValidationVariable> _validationVariables { get; set; }

    // Build the validation model from the JSON string.
    public ValidationModel(string validationVariablesJson)
    {   
        string validationVariablesFormattedString = validationVariablesJson.Replace(" ", string.Empty).Replace("\n", string.Empty).Replace("\\", string.Empty);

        _validationVariables = JsonConvert.DeserializeObject<List<ValidationVariable>>(validationVariablesFormattedString);
    }

    // Check if the values are valid.
    public bool IsValid(string rawValues)
    {
        string[] values = rawValues.Split(",");

        for (int i = 0; i < _validationVariables.Count; i++)
        {
            if (!_validationVariables[i].IsValid(values[i]))
            {
                return false;
            }
        }

        return true;
    }
}