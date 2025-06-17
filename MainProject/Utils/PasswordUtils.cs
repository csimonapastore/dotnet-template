using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BasicDotnetTemplate.MainProject.Enum;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Utils;
public partial class PasswordUtils
{
    protected PasswordUtils() { }

    private const int MIN_LENGTH = 8;
    private const int MIN_UPPER = 2;
    private const int MIN_LOWER = 2;
    private const int MIN_NUMBER = 2;
    private const int MIN_SPECIAL = 2;

    [GeneratedRegex("[A-Z]")]
    private static partial Regex RegexUpper();

    [GeneratedRegex("[a-z]")]
    private static partial Regex RegexLower();

    [GeneratedRegex("[0-9]")]
    private static partial Regex RegexNumber();

    [GeneratedRegex("[^a-zA-Z0-9]")]
    private static partial Regex RegexSpecial();

    private static readonly Regex RegexIdenticalChars = new(
        @"(\S)\1{2,}", 
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100)
    );

    public static List<string> ValidatePassword(string password)
    {
        List<string> errors = [];

        if (password.Length < MIN_LENGTH)
            errors.Add(PasswordValidationEnum.MIN_LENGTH);

        if (RegexUpper().Matches(password).Count < MIN_UPPER)
            errors.Add(PasswordValidationEnum.MIN_UPPER);

        if (RegexLower().Matches(password).Count < MIN_LOWER)
            errors.Add(PasswordValidationEnum.MIN_LOWER);

        if (RegexNumber().Matches(password).Count < MIN_NUMBER)
            errors.Add(PasswordValidationEnum.MIN_NUMBER);

        if (RegexSpecial().Matches(password).Count < MIN_SPECIAL)
            errors.Add(PasswordValidationEnum.MIN_SPECIAL);

        if (RegexIdenticalChars.IsMatch(password))
            errors.Add(PasswordValidationEnum.IDENTICAL_CHARS);

        return errors;
    }


}

