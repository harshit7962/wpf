using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Media;

namespace MS.Internal.WindowsRuntime
{
    namespace Windows.UI.ViewManagement
    {
        /// <summary>
        /// Contains internal RCWs for invoking the UISettings
        /// </summary>
        internal static class UISettingsRCW
        {
            private static readonly Guid IID_IActivationFactory = Guid.Parse("00000035-0000-0000-C000-000000000046");

            public static object GetUISettingsActivationFactory()
            {
                const string typeName = "Windows.UI.ViewManagement.UISettings";
                IntPtr hstring = IntPtr.Zero;
                IntPtr instance = IntPtr.Zero;
                Marshal.ThrowExceptionForHR(NativeMethods.WindowsCreateString(typeName, typeName.Length, out hstring));
                try
                {
                    Marshal.ThrowExceptionForHR(NativeMethods.RoActivateInstance(hstring, out IntPtr instance));
                    return Marshal.GetObjectForIUnknown(instance);
                }
                finally
                {
                    Marshal.Release(instance);
                    Marshal.ThrowExceptionForHR(NativeMethods.WindowsDeleteString(hstring));
                }
            }

            internal enum TrustLevel
            {
                BaseTrust,
                PartialTrust,
                FullTrust
            }

            [Guid("03021BE4-5254-4781-8194-5168F7D06D7B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            [ComImport]
            internal interface IUISettings3
            {
                [MethodImpl(MethodImplOptions.InternalCall)]
                UIColor GetColorValue([In] UIColorType desiredColor);
            }

            public enum UIColorType
            {
                Background = 0,
                Foreground = 1,
                AccentDark3 = 2,
                AccentDark2 = 3,
                AccentDark1 = 4,
                Accent = 5,
                AccentLight1 = 6,
                AccentLight2 = 7,
                AccentLight3 = 8,
                Complement = 9
            }

            internal readonly record struct UIColor(byte A, byte R, byte G, byte B);
        }
    }
}