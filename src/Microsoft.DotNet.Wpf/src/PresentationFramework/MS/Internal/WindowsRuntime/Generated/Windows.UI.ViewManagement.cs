using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;

#pragma warning disable CS0649


namespace MS.Internal.WindowsRuntime
{
    namespace Windows.UI.ViewManagement
    {
        internal static unsafe partial class WinRT
        {
            [DllImport("combase.dll", CallingConvention = CallingConvention.StdCall)]
            internal static extern unsafe int WindowsCreateString([MarshalAs(UnmanagedType.LPWStr)] string sourceString,
                                                  int length,
                                                  out IntPtr hstring);

            [DllImport("combase.dll", CallingConvention = CallingConvention.StdCall)]
            internal static extern int WindowsDeleteString(IntPtr hstring);


            /// <include file='WinRT.xml' path='doc/member[@name="WinRT.RoActivateInstance"]/*' />
            [DllImport("combase.dll", CallingConvention = CallingConvention.StdCall)]
            public static extern int RoActivateInstance(IntPtr activatableClassId, IInspectable** instance);
            // public static extern int RoActivateInstance(IntPtr activatableClassId, out IntPtr instance);
        }
        
        internal unsafe class Win32WindowSettings
        {
            private readonly IUISettings3* _uiSettings3;

            // private static readonly Guid IID_IUISettings3 = Guid.Parse("03021BE4-5254-4781-8194-5168F7D06D7B");

            public Win32WindowSettings()
            {
                const string uiSettingsClassName = "Windows.UI.ViewManagement.UISettings";
                IntPtr hstring;
                Marshal.ThrowExceptionForHR(WinRT.WindowsCreateString(uiSettingsClassName, uiSettingsClassName.Length, out hstring));

                IInspectable* inspectable;
                Marshal.ThrowExceptionForHR(WinRT.RoActivateInstance(hstring, &inspectable));
                WinRT.WindowsDeleteString(hstring);

                var guid = new Guid("03021BE4-5254-4781-8194-5168F7D06D7B");
                IUISettings3* uiSettings3;
                Marshal.ThrowExceptionForHR(inspectable->QueryInterface(&guid, (void**)&uiSettings3));
                _uiSettings3 = uiSettings3;

                // Marshal.ThrowExceptionForHR(WinRT.RoActivateInstance(hstring, out IntPtr punk));

                // WinRT.WindowsDeleteString(hstring);

                // Guid guid = IID_IUISettings3;
                // Marshal.ThrowExceptionForHR(Marshal.QueryInterface(punk, in guid, out IntPtr ppUnk));
                // IUISettings3 uiSettings3 = (IUISettings3)Marshal.PtrToStructure(ppUnk, typeof(IUISettings3));
                
                // _uiSettings3 = &uiSettings3;

            }

            public Color AccentColor => _uiSettings3->GetColorValue(UIColorType.Accent);
            public Color AccentColor1 => _uiSettings3->GetColorValue(UIColorType.AccentDark1);
            public Color AccentColor2 => _uiSettings3->GetColorValue(UIColorType.AccentDark2);
            public Color AccentColor3 => _uiSettings3->GetColorValue(UIColorType.AccentDark3);
            public Color AccentColor4 => _uiSettings3->GetColorValue(UIColorType.AccentLight1);
            public Color AccentColor5 => _uiSettings3->GetColorValue(UIColorType.AccentLight2);
            public Color AccentColor6 => _uiSettings3->GetColorValue(UIColorType.AccentLight3);

            // Extract of the IUISettings3 from windows.ui.viewmanagement.idl
            private struct IUISettings3
            {
                public void** lpVtbl;

                public Color GetColorValue(UIColorType desiredColor)
                {
                    UIColor value;
                    // The GetColorValue method comes right after IInspectable and is at VTBL slot 6
                    ((delegate* unmanaged<IUISettings3*, UIColorType, UIColor*, int>)(lpVtbl[6]))((IUISettings3*)Unsafe.AsPointer(ref this), desiredColor, &value);

                    return Color.FromUInt32((uint)*(int*)&value);
                }
            }

            private enum UIColorType
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
            };

            private readonly record struct UIColor(byte A, byte R, byte G, byte B);

        }

        [Guid("AF86E2E0-B12D-4C6A-9C5A-D7AA65101E90")]
        internal unsafe partial struct IInspectable
        {

            public void** lpVtbl;

            /// <inheritdoc cref="IUnknown.QueryInterface" />
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int QueryInterface(Guid* riid, void** ppvObject)
            {
                return ((delegate* unmanaged<IInspectable*, Guid*, void**, int>)(lpVtbl[0]))((IInspectable*)Unsafe.AsPointer(ref this), riid, ppvObject);
            }
        }
    }
}