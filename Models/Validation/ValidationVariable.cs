using Newtonsoft.Json;

namespace aws_s3_file_validator.Models;

public class ValidationVariable
{
    public int ColumnIndex { get; private set; }

    #region Validation variables

    private Type _type { get; set; }
    private bool _required { get; set; }
    
    #endregion

    private Dictionary<string, Type> _typeMap = new Dictionary<string, Type>
    {
        { "int", typeof(int) },
        { "number", typeof(int) },
        { "string", typeof(string) },
        { "bool", typeof(bool)},
        { "boolean", typeof(bool)},
        { "DateTime", typeof(DateTime) },
        { "DateOnly", typeof(DateOnly) },
        { "Guid", typeof(Guid) }
    };

    [JsonConstructor]
    public ValidationVariable(int columnIndex, string type, bool required)
    {
        try
        {
            ColumnIndex = columnIndex;
            _type = _typeMap[type];
            _required = required;
        }
        catch
        {
            throw new Exception("Invalid validation variable.");
        }
    }

    // Build the validation variable from the JSON string.
    public ValidationVariable(object value)
    {
        try
        {
            ColumnIndex = int.Parse(value.GetType().GetProperty("columnIndex").GetValue(value).ToString());
            _type = Type.GetType(value.GetType().GetProperty("type").GetValue(value).ToString());
            _required = bool.Parse(value.GetType().GetProperty("required").GetValue(value).ToString());
        }
        catch
        {
            throw new Exception("Invalid validation variable.");
        }
    }

    // Check if the value is valid.
    public bool IsValid(string value)
    {
        if (_required && string.IsNullOrEmpty(value))
        {
            return false;
        }

        string valueType = value.Split(",")[ColumnIndex];

        switch (_typeMap[valueType])
        {
            case Type v when v == typeof(int): // Int
                return int.TryParse(value, out _);
            case Type v when v == typeof(bool): // Bool
                return float.TryParse(value, out _);
            case Type v when v == typeof(DateTime): // DateTime
                return DateTime.TryParse(value, out _);
            case Type v when v == typeof(DateOnly): // DateOnly
                return DateOnly.TryParse(value, out _);
            case Type v when v == typeof(Guid): // Guid
                return Guid.TryParse(value, out _);
            default:
                return false;
        }
    }
}