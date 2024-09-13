// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// 
//
// Description: TextDecorationCollectionConverter class
//
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;

using SR=MS.Internal.PresentationCore.SR;

namespace System.Windows
{
    /// <summary>
    /// TypeConverter for TextDecorationCollection 
    /// </summary>     
    public sealed class TextDecorationCollectionConverter : TypeConverter
    {
        /// <summary>
        /// CanConvertTo method
        /// </summary>
        /// <param name="context"> ITypeDescriptorContext </param>
        /// <param name="destinationType"> Type to convert to </param>
        /// <returns> false will always be returned because TextDecorations cannot be converted to any other type. </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            // Return false for any other target type. Don't call base.CanConvertTo() because it would be confusing 
            // in some cases. For example, for destination typeof(string), base TypeConveter just converts the
            // ITypeDescriptorContext to the full name string of the given type.
            return destinationType == typeof(InstanceDescriptor);
        }

        /// <summary>
        /// CanConvertFrom
        /// </summary>
        /// <param name="context"> ITypeDescriptorContext </param>
        /// <param name="sourceType">Type to convert to </param>
        /// <returns> true if it can convert from sourceType to TextDecorations, false otherwise </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// ConvertFrom
        /// </summary>
        /// <param name="context"> ITypeDescriptorContext </param>
        /// <param name="culture"> CultureInfo </param>        
        /// <param name="input"> The input object to be converted to TextDecorations </param>
        /// <returns> the converted value of the input object </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object input)
        {
            if (input is null)
                throw GetConvertFromException(input);

            if (input is not string value)
                throw new ArgumentException(SR.Format(SR.General_BadType, "ConvertFrom"), nameof(input));                     
                        
            return ConvertFromString(value);            
        }

        /// <summary>
        /// ConvertFromString
        /// </summary>
        /// <param name="text"> The string to be converted into TextDecorationCollection object </param>
        /// <returns> the converted value of the string flag </returns>
        /// <remarks>
        /// The text parameter can be either string "None" or a combination of the predefined 
        /// TextDecoration names delimited by commas (,). One or more blanks spaces can precede 
        /// or follow each text decoration name or comma. There can't be duplicate TextDecoration names in the 
        /// string. The operation is case-insensitive. 
        /// </remarks>
        public static new TextDecorationCollection ConvertFromString(string text)
        {
            if (text is null)
                return null;

            // Define constants that will make sure the match has been unique
            const byte OverlineMatch = 1 << 0;
            const byte BaselineMatch = 1 << 1;
            const byte UnderlineMatch = 1 << 2;
            const byte StrikethroughMatch = 1 << 3;

            // Flags indicating which pre-defined TextDecoration have been matched
            byte matchedDecorations = 0;

            // Sanitize the input
            ReadOnlySpan<char> decorationsSpan = text.AsSpan().Trim();

            // Test for "None", which equals to empty collection and needs to be specified alone
            if (decorationsSpan.IsEmpty || decorationsSpan.Equals("None", StringComparison.OrdinalIgnoreCase))
                return new TextDecorationCollection();

            // Create new collection, save allocations
            TextDecorationCollection textDecorations = new(1 + decorationsSpan.Count(','));

            // Go through each item in the input and match accordingly
            foreach (Range segment in decorationsSpan.Split(','))
            {
                ReadOnlySpan<char> decoration = decorationsSpan[segment].Trim();

                if (decoration.Equals("Overline", StringComparison.OrdinalIgnoreCase) && (matchedDecorations & OverlineMatch) == 0)
                {
                    textDecorations.Add(TextDecorations.OverLine[0]);
                    matchedDecorations |= OverlineMatch;
                }
                else if (decoration.Equals("Baseline", StringComparison.OrdinalIgnoreCase) && (matchedDecorations & BaselineMatch) == 0)
                {
                    textDecorations.Add(TextDecorations.Baseline[0]);
                    matchedDecorations |= BaselineMatch;
                }
                else if (decoration.Equals("Underline", StringComparison.OrdinalIgnoreCase) && (matchedDecorations & UnderlineMatch) == 0)
                {
                    textDecorations.Add(TextDecorations.Underline[0]);
                    matchedDecorations |= UnderlineMatch;
                }
                else if (decoration.Equals("Strikethrough", StringComparison.OrdinalIgnoreCase) && (matchedDecorations & StrikethroughMatch) == 0)
                {
                    textDecorations.Add(TextDecorations.Strikethrough[0]);
                    matchedDecorations |= StrikethroughMatch;
                }
                else
                {
                    throw new ArgumentException(SR.Format(SR.InvalidTextDecorationCollectionString, text));
                }
            }

            return textDecorations;
        }        

        /// <summary>
        /// ConvertTo
        /// </summary>
        /// <param name="context"> ITypeDescriptorContext </param>
        /// <param name="culture"> CultureInfo </param>        
        /// <param name="value"> the object to be converted to another type </param>
        /// <param name="destinationType"> The destination type of the conversion </param>
        /// <returns> null will always be returned because TextDecorations cannot be converted to any other type. </returns>        
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) && value is IEnumerable<TextDecoration>)
            {
                ConstructorInfo ci = typeof(TextDecorationCollection).GetConstructor(new Type[] { typeof(IEnumerable<TextDecoration>) });

                return new InstanceDescriptor(ci, new object[] { value });
            }

            // Pass unhandled cases to base class (which will throw exceptions for null value or destinationType.)
            return base.ConvertTo(context, culture, value, destinationType);
        }     
    }             
}
