using System.Threading.Channels;
using aws_s3_file_validator.Models;

namespace aws_s3_file_validator.Validators;

public static class DogValidator
{
    public static bool IsValid(this Dog model)
    {
        bool dateOfBirthValid = model.IsDateOfBirthValid();
        bool dateOfDeathValid = model.IsDateOfDeathValid();
        bool ageValid = model.IsAgeValid();

        return dateOfBirthValid && dateOfDeathValid && ageValid;
    }

    private static bool IsDateOfBirthValid(this Dog model)
    {
        return
            model.DateOfBirth < model.DateOfDeath &&
            model.DateOfBirth < DateOnly.FromDateTime(DateTime.Now);
    }

    private static bool IsDateOfDeathValid(this Dog model)
    {
        return
            model.DateOfDeath < DateOnly.FromDateTime(DateTime.Now);
    }

    private static bool IsAgeValid(this Dog model)
    {
        return
            model.Age > 0 &&
            model.Age < 35;
    }

    private static bool IsDeathFlagValid(this Dog model)
    {
        return
            model.IsDead == (model.DateOfDeath != null);
    }
}