// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if RIBBON_IN_FRAMEWORK
namespace System.Windows.Controls.Ribbon.Primitives
#else
namespace Microsoft.Windows.Controls.Ribbon.Primitives
#endif
{
    internal interface IContainsStarLayoutManager
    {
        ISupportStarLayout StarLayoutManager
        {
            get;
            set;
        }
    }
}
