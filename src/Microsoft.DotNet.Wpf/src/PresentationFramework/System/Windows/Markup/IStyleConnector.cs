// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
// Description:
//   Provides methods used internally by the StyleBamlReader
//
namespace System.Windows.Markup
{
    /// <summary>
    /// Provides methods used internally by the StyleBamlReader
    /// on compiled content.
    /// </summary>
    public interface IStyleConnector
    {
        /// <summary>
        /// Called by the StyleBamlReader to attach events on EventSetters and
        /// Templates in compiled content.
        /// </summary>
        void Connect(int connectionId, object target);
    }
}
