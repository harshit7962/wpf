﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//
// This file was generated, please do not edit it directly.
//
// Please see MilCodeGen.html for more information.
//

using System.Windows.Markup;

#pragma warning disable 1634, 1691  // suppressing PreSharp warnings

namespace System.Windows.Media.Media3D.Converters
{
    /// <summary>
    /// Point3DCollectionValueSerializer - ValueSerializer class for converting instances of strings to and from Point3DCollection instances
    /// This is used by the MarkupWriter class.
    /// </summary>
    public class Point3DCollectionValueSerializer : ValueSerializer 
    {
        /// <summary>
        /// Returns true.
        /// </summary>
        public override bool CanConvertFromString(string value, IValueSerializerContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns true if the given value can be converted into a string
        /// </summary>
        public override bool CanConvertToString(object value, IValueSerializerContext context)
        {
            // Validate the input type
            if (!(value is Point3DCollection))
            {
                return false;
            }

            return true;
}

        /// <summary>
        /// Converts a string into a Point3DCollection.
        /// </summary>
        public override object ConvertFromString(string value, IValueSerializerContext context)
        {
            if (value != null)
            {
                return Point3DCollection.Parse(value );
            }
            else
            {
                return base.ConvertFromString( value, context );
            }
}

        /// <summary>
        /// Converts the value into a string.
        /// </summary>
        public override string ConvertToString(object value, IValueSerializerContext context)
        {
            if (value is Point3DCollection instance)
            {


#pragma warning suppress 6506 // instance is obviously not null
                return instance.ConvertToString(null, System.Windows.Markup.TypeConverterHelper.InvariantEnglishUS);
            }

            return base.ConvertToString(value, context);
        }
    }
}
