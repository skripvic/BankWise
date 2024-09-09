using System.Windows.Controls;

namespace View;

public class DepositValidation : ValidationRule
{
    public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
    {
        if (!int.TryParse(value.ToString(), out var intValue))
        {
            return new ValidationResult(false, "Введите целое число.");
        }

        if (intValue < 0 || intValue > 200)
        {
            return new ValidationResult(false, "Число банкнот должно быть от 0 до 200.");
        }

        return ValidationResult.ValidResult;
    }
}