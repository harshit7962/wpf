// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace System.Windows;

internal enum WindowBackdropType
{
    /// <summary>
    /// No backdrop effect.
    /// </summary>
    Auto,

    /// <summary>
    /// Sets <c>DWMWA_SYSTEMBACKDROP_TYPE</c> to <see langword="0"></see>.
    /// </summary>
    None,

    /// <summary>
    /// Windows 11 Mica effect.
    /// </summary>
    MainWindow,

    /// <summary>
    /// Windows Acrylic effect.
    /// </summary>
    TransientWindow,

    /// <summary>
    /// Windows 11 wallpaper blur effect.
    /// </summary>
    TabbedWindow
}
