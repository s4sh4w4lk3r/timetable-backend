using System.Text.RegularExpressions;

namespace Validation;

public static partial class StaticValidator
{
    /// <summary>
    /// Проверяет строку с Email адресом с помощью регулярного выражения.
    /// </summary>
    /// <param name="email"></param>
    /// <returns>True если адрес имеет валидный формат, иначе False.</returns>
    public static bool ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email) is true)
        {
            return false;
        }

        var regex = EmailRegex();
        if (regex.IsMatch(email) is false)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Проверяет строку с паролем с помощью регулярного выражения на наличие больших, маленьких букв, цифр, спецсимволов и мин. длины 8 симв. Также на IsNullOrWhiteSpace.
    /// </summary>
    /// <param name="password"></param>
    /// <returns>True если адрес имеет валидный формат, иначе False.</returns>
    public static bool ValidatePassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) is true)
        {
            return false;
        }

        var regex = PasswordRegex();
        if (regex.IsMatch(password) is false)
        {
            return false;
        }

        return true;
    }





    [GeneratedRegex("^\\S+@\\S+\\.\\S+$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
    private static partial Regex PasswordRegex();
}