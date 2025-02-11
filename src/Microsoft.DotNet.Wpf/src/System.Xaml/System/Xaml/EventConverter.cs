﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Globalization;

namespace System.Xaml
{
    internal class EventConverter : TypeConverter
    {
        // CanConvertTo and ConvertTo are not implemented here because it is not possible to convert
        // an event/delegate to string in the general case, because
        // 1. an event's getter is private.
        // 2. we currently do not have a syntax for writing down a multi-cast delegate
        // 3. we currently do not have a syntax to write down a method not on the root object.

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string valueString)
            {
                GetRootObjectAndDelegateType(context, out object? rootObject, out Type? delegateType);

                if (rootObject is not null && delegateType is not null)
                {
                    return Delegate.CreateDelegate(delegateType, rootObject, valueString);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        internal static void GetRootObjectAndDelegateType(ITypeDescriptorContext? context, out object? rootObject, out Type? delegateType)
        {
            rootObject = null;
            delegateType = null;

            if (context is null)
            {
                return;
            }

            IRootObjectProvider? rootObjectService = context.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            if (rootObjectService is null)
            {
                return;
            }
            rootObject = rootObjectService.RootObject;

            IDestinationTypeProvider? targetService = context.GetService(typeof(IDestinationTypeProvider)) as IDestinationTypeProvider;
            if (targetService is null)
            {
                return;
            }
            delegateType = targetService.GetDestinationType();
        }
    }
}
