using System.Collections;
using System.ComponentModel;
using Newtonsoft.Json;

namespace aws_s3_file_validator.Models;

public class ValidationVariable
{
    public string Name { get; private set; }
    public int ColumnIndex { get; private set; }

    #region Validation variables

    private Type _type { get; set; }
    private bool _required { get; set; }

    private TypeConverter _typeConverter { get; set; }

    private object _minimum { get; set; }
    private object _maximum { get; set; }
    
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
    public ValidationVariable(string name, int columnIndex, string type, bool required, string minimum = null, string maximum = null)
    {
        try
        {
            Name = name;
            ColumnIndex = columnIndex;
            _type = _typeMap[type];
            _required = required;

            _typeConverter = TypeDescriptor.GetConverter(_type);

            if (minimum != null)
            {
                _minimum = _typeConverter.ConvertFromInvariantString(minimum);
            }

            if (maximum != null)
            {
                _maximum = _typeConverter.ConvertFromInvariantString(maximum);
            }
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

        if (IsTypeValid(value))
        {
            return true;
        }

        return false;
    }

    private bool IsTypeValid(string value)
    {
        try
        {
            var convertedValue = _typeConverter.ConvertFromInvariantString(value);

            if (_minimum != null && Comparer.Default.Compare(convertedValue, _minimum) < 0)
            {
                return false;
            }

            if (_maximum != null && Comparer.Default.Compare(convertedValue, _maximum) > 0)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}