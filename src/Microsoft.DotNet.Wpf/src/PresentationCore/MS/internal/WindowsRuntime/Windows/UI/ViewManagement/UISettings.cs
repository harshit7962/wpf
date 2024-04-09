using MS.Internal.PresentationCore.WindowsRuntime;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace MS.Internal.WindowsRuntime
{
    namespace Windows.UI.ViewManagement
    {
        internal class UISettings
        {
            private static readonly bool _isSupported;
            private static object _winRtInstance;
            private UISettingsRCW.IUISettings3 _uisettings;

            static UISettings()
            {
                try
                {
                    if (GetWinRTInstance(forceInitialization: true) == null)
                    {
                        _isSupported = false;
                    }
                    _isSupported = true;
                }
                catch
                {
                    _isSupported = false;
                }
            }

            internal UISettings()
            {
                if (!_isSupported)
                {
                    throw new PlatformNotSupportedException();
                }

                try
                {
                    UISettingsRCW.IUISettings3 uisettings;

                    try
                    {
                        uisettings = GetWinRTInstance() as UISettingsRCW.IUISettings3;
                    }
                    catch (COMException)
                    {
                        uisettings = GetWinRTInstance(forceInitialization: true) as UISettingsRCW.IUISettings3;
                    }

                    _uisettings = uisettings;
                }
                catch (COMException)
                {
                }


                if (_uisettings == null)
                {
                    throw new PlatformNotSupportedException();
                }
            }

            internal Color GetColorValue(UISettingsRCW.UIColorType desiredColor)
            {
                Color color;
                try
                {
                    var uiColor = _uisettings.GetColorValue(desiredColor);
                    color = Color.FromArgb(uiColor.A, uiColor.R, uiColor.G, uiColor.B);
                }
                catch (COMException)
                {
                    // Fallback Accent color
                    color = Color.FromRgb(0x33, 0x79, 0xd9);
                }

                return color;
            }

        
            private static object GetWinRTInstance(bool forceInitialization = false)
            {
                if (_winRtInstance == null || forceInitialization)
                {
                    try
                    {
                        _winRtInstance = UISettingsRCW.GetUISettingsInstance();
                    }
                    catch (Exception e) when (e is TypeLoadException
                                             || e is FileNotFoundException
                                             || e is EntryPointNotFoundException
                                             || e is DllNotFoundException
                                             || e.HResult == NativeMethods.E_NOINTERFACE
                                             || e.HResult == NativeMethods.REGDB_E_CLASSNOTREG)
                    {
                        _winRtInstance = null;
                    }
                }

                return _winRtInstance;
            }
        }
    }
}