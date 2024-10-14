using System;

namespace System.Windows;

public readonly struct BackdropType : IEquatable<BackdropType>
{
    public static BackdropType None => new BackdropType();
    public static BackdropType Mica => new BackdropType("Mica");
    public static BackdropType Acrylic => new BackdropType("Acrylic");
    public static BackdropType MicaAlt => new BackdropType("MicaAlt");

    public string Value => _value ?? "None";

    public BackdropType(string value)
    {
        _value = value;
    }

    public bool Equals(BackdropType givenBackdropType)
    {
        return string.Equals(Value, givenBackdropType.Value, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
        return obj is BackdropType backdropType && Equals(backdropType);
    }

    public override int GetHashCode()
    {
        return _value != null ? StringComparer.Ordinal.GetHashCode(_value) : 0;
    }

    public static bool operator ==(BackdropType left, BackdropType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BackdropType left, BackdropType right)
    {
        return !left.Equals(right);
    }

    // Shouldn't be here, will be moved to another class
    internal bool IsValidBackdropType(BackdropType backdropType) {
        return backdropType  == BackdropType.None || 
            backdropType == BackdropType.Mica || 
            backdropType == BackdropType.Acrylic || 
            backdropType == BackdropType.MicaAlt;
    }

    public override string ToString() => Value;

    private readonly string _value;
}