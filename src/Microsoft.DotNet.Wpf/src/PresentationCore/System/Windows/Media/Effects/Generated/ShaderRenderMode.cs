// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//
// This file was generated, please do not edit it directly.
//
// Please see MilCodeGen.html for more information.
//

#if PRESENTATION_CORE
#else
using SR=System.Windows.SR;
#endif

namespace System.Windows.Media.Effects
{
    /// <summary>
    ///     ShaderRenderMode - Policy for rendering the shader in software.
    /// </summary>
    public enum ShaderRenderMode
    {
        /// <summary>
        ///     Auto - Allow hardware and software
        /// </summary>
        Auto = 0,

        /// <summary>
        ///     SoftwareOnly - Force software rendering
        /// </summary>
        SoftwareOnly = 1,

        /// <summary>
        ///     HardwareOnly - Require hardware rendering, ignore otherwise
        /// </summary>
        HardwareOnly = 2,
    }   
}
