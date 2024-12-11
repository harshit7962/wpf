﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Windows.Interop
{
    /// <summary>
    ///     Defines the contract for Win32 window handles.
    /// </summary>
    public interface IWin32Window
    {
        /// <summary>
        ///     Handle to the window.
        /// </summary>
        IntPtr Handle
        {
            get;
        }
    }
}

