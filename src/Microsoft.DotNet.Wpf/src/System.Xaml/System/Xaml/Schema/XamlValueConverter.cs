// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel;
using System.Threading;

namespace System.Xaml.Schema
{
    public class XamlValueConverter<TConverterBase> : IEquatable<XamlValueConverter<TConverterBase>>
        where TConverterBase : class
    {
        // Assignment should be idempotent
        private TConverterBase _instance;
        private ThreeValuedBool _isPublic;

        private volatile bool _instanceIsSet; // volatile for the same reason as valid flags in TypeReflector/MemberReflector

        public string Name { get; }
        public Type ConverterType { get; }
        public XamlType TargetType { get; }

        public XamlValueConverter(Type converterType, XamlType targetType)
            :this(converterType, targetType, null)
        {
        }

        public XamlValueConverter(Type converterType, XamlType targetType, string name)
        {
            if (converterType is null && targetType is null && name is null)
            {
                throw new ArgumentException(SR.Format(SR.ArgumentRequired, $"{nameof(converterType)}, {nameof(targetType)}, {nameof(name)}"));
            }

            ConverterType = converterType;
            TargetType = targetType;
            Name = name ?? GetDefaultName();
        }

        public TConverterBase ConverterInstance
        {
            get
            {
                if (!_instanceIsSet)
                {
                    Interlocked.CompareExchange(ref _instance, CreateInstance(), null);
                    _instanceIsSet = true;
                }

                return _instance;
            }
        }

        public override string ToString() => Name;

        internal virtual bool IsPublic
        {
            get
            {
                if (_isPublic == ThreeValuedBool.NotSet)
                {
                    _isPublic = (ConverterType is null || ConverterType.IsVisible) ? ThreeValuedBool.True : ThreeValuedBool.False;
                }

                return _isPublic == ThreeValuedBool.True;
            }
        }

        protected virtual TConverterBase CreateInstance()
        {
            if (ConverterType == typeof(EnumConverter) &&
                TargetType.UnderlyingType is not null && TargetType.UnderlyingType.IsEnum)
            {
                return (TConverterBase)(object)new EnumConverter(TargetType.UnderlyingType);
            }
            else if (ConverterType is not null)
            {
                if (!typeof(TConverterBase).IsAssignableFrom(ConverterType))
                {
                    throw new XamlSchemaException(SR.Format(SR.ConverterMustDeriveFromBase,
                       ConverterType, typeof(TConverterBase)));
                }

                return (TConverterBase)Activator.CreateInstance(ConverterType, null);
            }

            return null;
        }

        private string GetDefaultName()
        {
            if (ConverterType is not null)
            {
                if (TargetType is not null)
                {
                    return $"{ConverterType.Name}({TargetType.Name})";
                }

                return ConverterType.Name;
            }

            return TargetType.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is not XamlValueConverter<TConverterBase> other)
            {
                return false;
            }

            return this == other;
        }

        public override int GetHashCode()
        {
            int result = Name.GetHashCode();
            if (ConverterType is not null)
            {
                result ^= ConverterType.GetHashCode();
            }

            if (TargetType is not null)
            {
                result ^= TargetType.GetHashCode();
            }

            return result;
        }

        public bool Equals(XamlValueConverter<TConverterBase> other) => this == other;

        public static bool operator ==(XamlValueConverter<TConverterBase> converter1, XamlValueConverter<TConverterBase> converter2)
        {
            if (converter1 is null)
            {
                return converter2 is null;
            }

            if (converter2 is null)
            {
                return false;
            }

            return converter1.ConverterType == converter2.ConverterType &&
                converter1.TargetType == converter2.TargetType &&
                converter1.Name == converter2.Name;
        }

        public static bool operator !=(XamlValueConverter<TConverterBase> converter1, XamlValueConverter<TConverterBase> converter2)
            => !(converter1 == converter2);
    }
}
