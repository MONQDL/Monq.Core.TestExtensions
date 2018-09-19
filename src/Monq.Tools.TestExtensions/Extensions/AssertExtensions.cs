﻿using Monq.Tools.MvcExtensions.Extensions;
using Monq.Tools.MvcExtensions.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;
using System.Reflection;

namespace Monq.Tools.TestExtensions.Extensions
{
    public static class AssertExtensions
    {
        public static void AssertFilterIsValid<TFilter, TModel>()
        {
            var ag = new ExceptionAggregator();
            var filterType = typeof(TFilter);
            var modelType = typeof(TModel);
            var filteredProperties = filterType.GetFilteredProperties().ToList();

            var reqModelProps = filteredProperties.Select(x => x.GetCustomAttributes<FilteredByAttribute>()).SelectMany(x => x).Select(x => x.FilteredProperty).ToList();
            var badProps = reqModelProps.Except(modelType.GetProperties().Select(x => x.Name)).ToList();
            if (badProps.Count > 0)
                ag.Add(new XunitException($"В конечной модели отсутствуют поля {string.Join(",", badProps)}"));

            foreach (var property in filteredProperties)
            {
                var modelPropertyName = property.GetCustomAttributes<FilteredByAttribute>().Select(x => x.FilteredProperty).FirstOrDefault();
                if (badProps.Contains(modelPropertyName)) continue;

                var filterPropType = property.PropertyType;
                if (filterPropType.IsGenericType)
                    filterPropType = filterPropType.GetGenericArguments().FirstOrDefault();
                var modelPropType = modelType.GetProperty(modelPropertyName).PropertyType;

                if (!filterPropType.Equals(modelPropType))
                    ag.Add(new EqualException($"Свойство {modelPropertyName} должно быть типа {filterPropType.Name}", modelPropType.Name));
            }
            if (ag.HasExceptions)
                throw ag.ToException();
        }
    }
}