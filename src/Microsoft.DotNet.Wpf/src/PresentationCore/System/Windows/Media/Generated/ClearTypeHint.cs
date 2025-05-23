// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
// This file was generated, please do not edit it directly.
//
// Please see MilCodeGen.html for more information.
//


namespace System.Windows.Media
{
    /// <summary>
    ///     ClearTypeHint - Enum used for hinting the rendering engine that text can be 
    ///     rendered with ClearType.
    /// </summary>
    public enum ClearTypeHint
    {
        /// <summary>
        ///     Auto - Rendering engine will use ClearType when it is determined possible.  If an 
        ///     intermediate render target has been introduced in the ancestor tree, ClearType will 
        ///     be disabled.
        /// </summary>
        Auto = 0,

        /// <summary>
        ///     Enabled - Rendering engine will enable ClearType for this element subtree.  Where 
        ///     an intermediate render target is introduced in this subtree, ClearType will once 
        ///     again be disabled.
        /// </summary>
        Enabled = 1,
    }
}
