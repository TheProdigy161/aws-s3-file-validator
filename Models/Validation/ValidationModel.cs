using Newtonsoft.Json;

namespace aws_s3_file_validator.Models;

public class ValidationModel
{
    private List<ValidationVariable> _validationVariables { get; set; }

    // Build the validation model from the JSON string.
    public ValidationModel(string jsonString)
    {
        string formattedString = jsonString.Replace(" ", string.Empty).Replace("\n", string.Empty).Replace("\\", string.Empty);
        _validationVariables = JsonConvert.DeserializeObject<List<ValidationVariable>>(formattedString);
    }

    // Check if the values are valid.
    public bool IsValid(string rawValues)
    {
        List<string> values = rawValues.Split(",").ToList();

        foreach (var v in _validationVariables)
        {
            if (!v.IsValid(values[v.ColumnIndex]))
            {
                return false;
            }
        }

        return true;
    }
}