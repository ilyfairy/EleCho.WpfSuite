﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace EleCho.WpfSuite
{
    /// <summary>
    /// Value converter that returns a boolean value that indicates that the collection is not null or empty
    /// </summary>
    public class CollectionIsNotNullOrEmptyConverter : SingletonValueConverterBase<CollectionIsNotNullOrEmptyConverter>
    {
        static readonly Type _typeGenericCollection = typeof(ICollection<>);
        static readonly PropertyInfo _genericCollectionCountProperty = _typeGenericCollection.GetProperty(nameof(ICollection<object>.Count))!;

        /// <inheritdoc/>
        public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return false;

            if (value is ICollection nonGenericCollection)
            {
                return nonGenericCollection.Count != 0;
            }
            else
            {
                var valueType = value.GetType();
                var interfaceTypes = valueType.GetInterfaces();

                if (interfaceTypes.FirstOrDefault(type => type.GetGenericTypeDefinition() == _typeGenericCollection) is Type interfaceType)
                {
                    var property = interfaceType.GetProperty(nameof(ICollection<object>.Count));

                    return (int)(property.GetValue(value)!) != 0;
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
