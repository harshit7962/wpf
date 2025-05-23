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
    ///     BrushMappingMode - Enum which describes whether certain values should be considered 
    ///     as absolute local coordinates or whether they should be considered multiples of a 
    ///     bounding box's size.
    /// </summary>
    public enum BrushMappingMode
    {
        /// <summary>
        ///     Absolute - Absolute means that the values in question will be interpreted directly 
        ///     in local space.
        /// </summary>
        Absolute = 0,

        /// <summary>
        ///     RelativeToBoundingBox - RelativeToBoundingBox means that the values will be 
        ///     interpreted as a multiples of a bounding box, where 1.0 is considered 100% of the 
        ///     bounding box measure.
        /// </summary>
        RelativeToBoundingBox = 1,
    }
}
