namespace BasicDotnetTemplate.MainProject.Enum;
public static class PasswordValidationEnum
{
    public const string MIN_LENGTH = "Password must be at least 8 characters long";
    public const string MIN_UPPER = "Password must have at least 2 uppercase letters";
    public const string MIN_LOWER = "Password must have at least 2 lowercase letters";
    public const string MIN_NUMBER = "Password must be at least 2 numbers";
    public const string MIN_SPECIAL = "Password must be at least 2 special characters";
    public const string IDENTICAL_CHARS = "Password cannot have 3 or more consecutive identical characters";
}
