using System;

namespace FCG.Domain.Shared;

public static class AssertionConcern
{
    public static void AssertTrue(bool condition, string message)
    {
        if (!condition)
            throw new ApplicationException(message);
    }

    public static void AssertNotEmpty(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ApplicationException(message);
    }

    public static void AssertLength(string value, int min, int max, string message)
    {
        if (value.Length < min || value.Length > max)
            throw new ApplicationException(message);
    }
}
