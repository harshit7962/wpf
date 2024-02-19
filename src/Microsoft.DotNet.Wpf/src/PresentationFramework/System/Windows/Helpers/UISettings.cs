using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Appearance;
using System.Windows.Interop;
using Microsoft.Win32;
using System.Windows.Media.ColorPalette;

namespace System.Windows;
public class UISettings
{
    public Action<UISettings, object> ColorValuesChanged { get; internal set; }
    public Action<object, object> AdvancedEffectsEnabledChanged { get; internal set; }
    public Action<object, object> AutoHideScrollBarsChanged { get; internal set; }
    public bool AdvancedEffectsEnabled { get; internal set; }
    public object AutoHideScrollBars { get; internal set; }

    internal static Color GetColorValue(UIColorType accent)
    {
        var regKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\DWM";
        var regValue = (Int32)Registry.GetValue(
        regKey,
        "AccentColor",
        null);
    
        ByteColor currentAccent = new ByteColor(0xff, 0x00, 0x78, 0xd4);

        if(regValue is Int32 x)
        {
            currentAccent = ParseDWordColor(x);
        }
        else
        {
            // Implement a method that returns fallback color
            throw new NotImplementedException();
        }

        Color currentAccentColor = Color.FromArgb(currentAccent.A, currentAccent.R, currentAccent.G, currentAccent.B);

        switch(accent)
        {
            case UIColorType.Accent:
                break;
            case UIColorType.AccentDark1:
                currentAccentColor = currentAccentColor.Update(15f, -12f);
                break;
            case UIColorType.AccentDark2:
                currentAccentColor = currentAccentColor.Update(30f, -24f);
                break;
            case UIColorType.AccentDark3:
                currentAccentColor = currentAccentColor.Update(45f, -36f);
                break;
            case UIColorType.AccentLight1:
                currentAccentColor = currentAccentColor.UpdateBrightness(-5f);
                break;
            case UIColorType.AccentLight2:
                currentAccentColor = currentAccentColor.UpdateBrightness(-10f);
                break;
            case UIColorType.AccentLight3:
                currentAccentColor = currentAccentColor.UpdateBrightness(-15f);
                break;
            default:
                // Implement a method that returns a Fallback color
                throw new NotImplementedException();
        }

        return currentAccentColor;
    }
    
    private static ByteColor ParseDWordColor(Int32 color)
    {
        Byte
            a = (byte)((color >> 24) & 0xFF),
            b = (byte)((color >> 16) & 0xFF),
            g = (byte)((color >> 8) & 0xFF),
            r = (byte)((color >> 0) & 0xFF);

        ByteColor current = new ByteColor(a, r, g, b);

        return current;
    }

    private static ApplicationTheme GetCurrentSystemTheme()
    {
        var currentTheme =
        Registry.GetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes",
            "CurrentTheme",
            "aero.theme"
        ) as string
        ?? string.Empty;

        if (!string.IsNullOrEmpty(currentTheme))
        {
        currentTheme = currentTheme.ToLower().Trim();

        // This may be changed in the next versions, check the Insider previews
        if (currentTheme.Contains("basic.theme"))
            {
                return ApplicationTheme.Light;
            }

            if (currentTheme.Contains("aero.theme"))
            {
                return ApplicationTheme.Light;
            }

            if (currentTheme.Contains("dark.theme"))
            {
                return ApplicationTheme.Dark;
            }

            if (currentTheme.Contains("hcblack.theme"))
            {
                return ApplicationTheme.Dark;
            }

            if (currentTheme.Contains("hcwhite.theme"))
            {
                return ApplicationTheme.Light;
            }

            if (currentTheme.Contains("hc1.theme"))
            {
                return ApplicationTheme.Dark;
            }

            if (currentTheme.Contains("hc2.theme"))
            {
                return ApplicationTheme.Dark;
            }
        }

        //if (currentTheme.Contains("custom.theme"))
        //    return ; custom can be light or dark
        var rawAppsUseLightTheme = Registry.GetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
            "AppsUseLightTheme",
            1
        );

        if (rawAppsUseLightTheme is 0)
        {
            return ApplicationTheme.Dark;
        }
        else if (rawAppsUseLightTheme is 1)
        {
            return ApplicationTheme.Light;
        }

        var rawSystemUsesLightTheme =
            Registry.GetValue(
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
                "SystemUsesLightTheme",
                1
            ) ?? 1;

        return rawSystemUsesLightTheme is 0 ? ApplicationTheme.Dark : ApplicationTheme.Light;
    }

    public static Color SystemBackground
    {
        get
        {
        ApplicationTheme currentTheme = GetCurrentSystemTheme();
        if(currentTheme == ApplicationTheme.Light)
        {
            return Color.FromArgb(255, 255, 255, 255);
        }

        return Color.FromArgb(255, 0, 0, 0);
        }
    }

    private static Color GetDefaultWindowsAccentColor()
    {
        return Color.FromArgb(0xff, 0x00, 0x78, 0xd7);
    }

    public static Color SystemAccent
    {
        get
        {
        try
        {
            Dwmapi.DwmGetColorizationParameters(out var dwmParams);
            var values = BitConverter.GetBytes(dwmParams.clrColor);

            return Color.FromArgb(255, values[2], values[1], values[0]);
        }
        catch
        {
            var colorizationColorValue = Registry.GetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM",
                "ColorizationColor",
                null
            );

            if (colorizationColorValue is not null)
            {
                try
                {
                    var colorizationColor = (uint)(int)colorizationColorValue;
                    var values = BitConverter.GetBytes(colorizationColor);

                    return Color.FromArgb(255, values[2], values[1], values[0]);
                }
                catch { }
            }
        }

        return GetDefaultWindowsAccentColor();
        }
    }
}

public static class UISettingsExt {
    /// <summary>
    /// Maximum <see cref="Byte"/> size with the current <see cref="Single"/> precision.
    /// </summary>
    private static readonly float _byteMax = (float)Byte.MaxValue;

    // public static Color ToColor(this object color)
    // {
    //     return Color.FromRgb(240, 242, 255);
    // }

    /// <summary>
    /// Absolute byte.
    /// </summary>
    private static byte ToColorByte(int value)
    {
        if (value > Byte.MaxValue)
        {
            value = Byte.MaxValue;
        }
        else if (value < Byte.MinValue)
        {
            value = Byte.MinValue;
        }

        return Convert.ToByte(value);
    }

    public static Color UpdateBrightness(this Color color, float factor)
    {
        if (factor > 100f || factor < -100f)
        {
            throw new ArgumentOutOfRangeException(nameof(factor));
        }

        (float hue, float saturation, float rawBrightness) = color.ToHsv();

        (int red, int green, int blue) = FromHsvToRgb(hue, saturation, ToPercentage(rawBrightness + factor));

        return Color.FromArgb(color.A, ToColorByte(red), ToColorByte(green), ToColorByte(blue));
    }

    public static Color Update(
        this Color color, 
        float brightnessFactor, 
        float saturationFactor = 0,
        float luminanceFactor = 0
    )
    {
        if (brightnessFactor > 100f || brightnessFactor < -100f)
        {
            throw new ArgumentOutOfRangeException(nameof(brightnessFactor));
        }

        if (saturationFactor > 100f || saturationFactor < -100f)
        {
            throw new ArgumentOutOfRangeException(nameof(saturationFactor));
        }

        if (luminanceFactor > 100f || luminanceFactor < -100f)
        {
            throw new ArgumentOutOfRangeException(nameof(luminanceFactor));
        }

        (float hue, float rawSaturation, float rawBrightness) = color.ToHsv();

        (int red, int green, int blue) = FromHsvToRgb(
            hue,
            ToPercentage(rawSaturation + saturationFactor),
            ToPercentage(rawBrightness + brightnessFactor)
        );

        if (luminanceFactor == 0)
        {
            return Color.FromArgb(color.A, ToColorByte(red), ToColorByte(green), ToColorByte(blue));
        }

        (hue, float saturation, float rawLuminance) = Color
            .FromArgb(color.A, ToColorByte(red), ToColorByte(green), ToColorByte(blue))
            .ToHsl();

        (red, green, blue) = FromHslToRgb(hue, saturation, ToPercentage(rawLuminance + luminanceFactor));

        return Color.FromArgb(color.A, ToColorByte(red), ToColorByte(green), ToColorByte(blue));
    }

    /// <summary>
    /// Converts the color values stored as HSV (HSB) to RGB.
    /// </summary>
    private static (int R, int G, int B) FromHsvToRgb(float hue, float saturation, float brightness)
    {
        var red = 0;
        var green = 0;
        var blue = 0;

        if (AlmostEquals(saturation, 0, 0.01f))
        {
            red = green = blue = (int)(((brightness / 100f) * _byteMax) + 0.5f);

            return (red, green, blue);
        }

        hue /= 360f;
        brightness /= 100f;
        saturation /= 100f;

        var hueAngle = (hue - (float)Math.Floor(hue)) * 6.0f;
        var f = hueAngle - (float)Math.Floor(hueAngle);

        var p = brightness * (1.0f - saturation);
        var q = brightness * (1.0f - saturation * f);
        var t = brightness * (1.0f - (saturation * (1.0f - f)));

        switch ((int)hueAngle)
        {
            case 0:
                red = (int)(brightness * 255.0f + 0.5f);
                green = (int)(t * 255.0f + 0.5f);
                blue = (int)(p * 255.0f + 0.5f);

                break;
            case 1:
                red = (int)(q * 255.0f + 0.5f);
                green = (int)(brightness * 255.0f + 0.5f);
                blue = (int)(p * 255.0f + 0.5f);

                break;
            case 2:
                red = (int)(p * 255.0f + 0.5f);
                green = (int)(brightness * 255.0f + 0.5f);
                blue = (int)(t * 255.0f + 0.5f);

                break;
            case 3:
                red = (int)(p * 255.0f + 0.5f);
                green = (int)(q * 255.0f + 0.5f);
                blue = (int)(brightness * 255.0f + 0.5f);

                break;
            case 4:
                red = (int)(t * 255.0f + 0.5f);
                green = (int)(p * 255.0f + 0.5f);
                blue = (int)(brightness * 255.0f + 0.5f);

                break;
            case 5:
                red = (int)(brightness * 255.0f + 0.5f);
                green = (int)(p * 255.0f + 0.5f);
                blue = (int)(q * 255.0f + 0.5f);

                break;
        }

        return (red, green, blue);
    }

    /// <summary>
    /// HSV representation models how colors appear under light.
    /// </summary>
    /// <returns><see langword="float"/> hue, <see langword="float"/> saturation, <see langword="float"/> brightness</returns>
    private static (float Hue, float Saturation, float Value) ToHsv(this Color color)
    {
        int red = color.R;
        int green = color.G;
        int blue = color.B;

        var max = Math.Max(red, Math.Max(green, blue));
        var min = Math.Min(red, Math.Min(green, blue));

        var fDelta = (max - min) / _byteMax;

        float hue;
        float saturation;
        float value;

        if (max <= 0)
        {
            return (0f, 0f, 0f);
        }

        saturation = fDelta / (max / _byteMax);
        value = max / _byteMax;

        if (fDelta <= 0.0)
        {
            return (0f, saturation * 100f, value * 100f);
        }

        if (max == red)
        {
            hue = ((green - blue) / _byteMax) / fDelta;
        }
        else if (max == green)
        {
            hue = 2f + (((blue - red) / _byteMax) / fDelta);
        }
        else
        {
            hue = 4f + (((red - green) / _byteMax) / fDelta);
        }

        if (hue < 0)
        {
            hue += 360;
        }

        return (hue * 60f, saturation * 100f, value * 100f);
    }

    /// <summary>
    /// Converts the color values stored as HSL to RGB.
    /// </summary>
    private static (int R, int G, int B) FromHslToRgb(float hue, float saturation, float lightness)
    {
        if (AlmostEquals(saturation, 0, 0.01f))
        {
            var color = (int)(lightness * _byteMax);

            return (color, color, color);
        }

        lightness /= 100f;
        saturation /= 100f;

        var hueAngle = hue / 360f;

        return (
            CalcHslChannel(hueAngle + 0.333333333f, saturation, lightness),
            CalcHslChannel(hueAngle, saturation, lightness),
            CalcHslChannel(hueAngle - 0.333333333f, saturation, lightness)
        );
    }

    /// <summary>
    /// Whether the floating point number is about the same.
    /// </summary>
    private static bool AlmostEquals(float numberOne, float numberTwo, float precision = 0)
    {
        if (precision <= 0)
        {
            precision = Single.Epsilon;
        }

        return numberOne >= (numberTwo - precision) && numberOne <= (numberTwo + precision);
    }

    /// <summary>
    /// Calculates the color component for HSL.
    /// </summary>
    private static int CalcHslChannel(float color, float saturation, float lightness)
    {
        float num1,
            num2;

        if (color > 1)
        {
            color -= 1f;
        }

        if (color < 0)
        {
            color += 1f;
        }

        if (lightness < 0.5f)
        {
            num1 = lightness * (1f + saturation);
        }
        else
        {
            num1 = lightness + saturation - lightness * saturation;
        }

        num2 = (2f * lightness) - num1;

        if (color * 6f < 1)
        {
            return (int)((num2 + (num1 - num2) * 6f * color) * _byteMax);
        }

        if (color * 2f < 1)
        {
            return (int)(num1 * _byteMax);
        }

        if (color * 3f < 2)
        {
            return (int)((num2 + (num1 - num2) * (0.666666666f - color) * 6f) * _byteMax);
        }

        return (int)(num2 * _byteMax);
    }

    /// <summary>
    /// HSL representation models the way different paints mix together to create colour in the real world,
    /// with the lightness dimension resembling the varying amounts of black or white paint in the mixture.
    /// </summary>
    /// <returns><see langword="float"/> hue, <see langword="float"/> saturation, <see langword="float"/> lightness</returns>
    private static (float Hue, float Saturation, float Lightness) ToHsl(this Color color)
    {
        int red = color.R;
        int green = color.G;
        int blue = color.B;

        var max = Math.Max(red, Math.Max(green, blue));
        var min = Math.Min(red, Math.Min(green, blue));

        var fDelta = (max - min) / _byteMax;

        float hue;
        float saturation;
        float lightness;

        if (max <= 0)
        {
            return (0f, 0f, 0f);
        }

        saturation = 0.0f;
        lightness = ((max + min) / _byteMax) / 2.0f;

        if (fDelta <= 0.0)
        {
            return (0f, saturation * 100f, lightness * 100f);
        }

        saturation = fDelta / (max / _byteMax);

        if (max == red)
        {
            hue = ((green - blue) / _byteMax) / fDelta;
        }
        else if (max == green)
        {
            hue = 2f + (((blue - red) / _byteMax) / fDelta);
        }
        else
        {
            hue = 4f + (((red - green) / _byteMax) / fDelta);
        }

        if (hue < 0)
        {
            hue += 360;
        }

        return (hue * 60f, saturation * 100f, lightness * 100f);
    }

    /// <summary>
    /// Absolute percentage.
    /// </summary>
    private static float ToPercentage(float value)
    {
        return value switch
        {
            > 100f => 100f,
            < 0f => 0f,
            _ => value
        };
    }
}

public class ApiInformation
{
    internal static bool IsApiContractPresent(string universalApiContractName, int v)
    {
        return true;
    }
}

public enum UIColorType {
    Accent,
    AccentDark1,
    AccentDark2,
    AccentDark3,
    AccentLight1,
    AccentLight2,
    AccentLight3,
    Background,
    Complement,
    Foreground
}

internal struct ByteColor
{
    internal byte A { get; set; }
    internal byte R { get; set; }
    internal byte G { get; set; }
    internal byte B { get; set; }

    internal ByteColor(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }
}
