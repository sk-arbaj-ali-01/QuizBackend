namespace Quiz.Shared.Helpers;
public static class Utility
{
    public static bool IsValidString(string str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }
}
