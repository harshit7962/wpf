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
            private static object _winRtActivationFactory;
            private UISettingsRCW.IUISettings3 _uisettings;

            static UISettings()
            {
                try
                {
                    if (GetWinRtActivationFactory(forceInitialization: true) == null)
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
                        uisettings = GetWinRtActivationFactory() as UISettingsRCW.IUISettings3;
                    }
                    catch (COMException)
                    {
                        uisettings = GetWinRtActivationFactory(forceInitialization: true) as UISettingsRCW.IUISettings3;
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
                    color = Colors.Transparent;
                }

                return color;
            }

        
            private static object GetWinRtActivationFactory(bool forceInitialization = false)
            {
                if (_winRtActivationFactory == null || forceInitialization)
                {
                    try
                    {
                        _winRtActivationFactory = UISettingsRCW.GetUISettingsActivationFactory();
                    }
                    catch (Exception e) when (e is TypeLoadException
                                             || e is FileNotFoundException
                                             || e is EntryPointNotFoundException
                                             || e is DllNotFoundException
                                             || e.HResult == NativeMethods.E_NOINTERFACE
                                             || e.HResult == NativeMethods.REGDB_E_CLASSNOTREG)
                    {
                        _winRtActivationFactory = null;
                    }
                }

                return _winRtActivationFactory;
            }
        }
    }
}