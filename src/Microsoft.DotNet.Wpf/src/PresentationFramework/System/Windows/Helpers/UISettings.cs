using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Appearance;
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

  internal object GetColorValue(UIColorType accent)
  {
    throw new NotImplementedException();
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