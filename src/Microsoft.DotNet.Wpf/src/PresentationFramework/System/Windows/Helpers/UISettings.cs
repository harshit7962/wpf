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

  internal static object GetColorValue(UIColorType accent)
  {
    throw new NotImplementedException();
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
  public static Color ToColor(this object color)
  {
    return Color.FromRgb(240, 242, 255);
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