using Monq.Tools.MvcExtensions.Extensions;
using Monq.Tools.MvcExtensions.Filters;
using Monq.Tools.TestExtensions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Xunit
{
    public partial class Assert
    {
        /// <summary>
        /// Проверить класс фильтра на валидность.
        /// </summary>
        /// <typeparam name="TFilter">Тип фильтра.</typeparam>
        /// <typeparam name="TModel">Тип фильтруемого элемента.</typeparam>
        public static void FilterIsValid<TFilter, TModel>()
        {
            var filterType = typeof(TFilter);
            var modelType = typeof(TModel);
            var filteredProperties = filterType.GetFilteredProperties().ToList();

            var reqModelProps = filteredProperties.Select(x => x.GetCustomAttributes<FilteredByAttribute>()).SelectMany(x => x).Select(x => x.FilteredProperty).ToList();
            var badProps = reqModelProps.Where(x => modelType.GetPropertyType(x) == null).ToList();

            var badTypes = new List<(Type, Type, string)>();
            foreach (var property in filteredProperties)
            {
                var modelPropertyName = property.GetCustomAttributes<FilteredByAttribute>().Select(x => x.FilteredProperty).FirstOrDefault();
                if (badProps.Contains(modelPropertyName)) continue;

                var filterPropType = property.PropertyType;
                if (filterPropType.IsGenericType)
                    filterPropType = filterPropType.GetGenericArguments().FirstOrDefault();
                var modelPropType = modelType.GetPropertyType(modelPropertyName);

                if (!filterPropType.Equals(modelPropType))
                    badTypes.Add((filterPropType, modelPropType, modelPropertyName));
            }
            if (badProps.Count > 0 || badTypes.Count > 0)
                throw new FilterValidationException(badProps, badTypes);
        }

        /// <summary>
        /// Провести над всеми элементами коллекции действия.
        /// </summary>
        /// <typeparam name="TExpected">Тип ожидаемого элемента.</typeparam>
        /// <typeparam name="TActual">Тип .</typeparam>
        /// <typeparam name="TKey">Тип поля по которому будет вести сопоставление.</typeparam>
        /// <param name="expectedCollection">Ожидаемый.</param>
        /// <param name="actualCollection">Действительный.</param>
        /// <param name="expectedKeySelector">Поле по которому будет вестись сопоставление в ожидаемом.</param>
        /// <param name="actualKeySelector">Поле по которому будет вестись сопоставление в действительном.</param>
        /// <param name="action">Действие.</param>
        public static void Collection<TExpected, TActual, TKey>(IEnumerable<TExpected> expectedCollection, IEnumerable<TActual> actualCollection,
            Func<TExpected, TKey> expectedKeySelector, Func<TActual, TKey> actualKeySelector, Action<TExpected, TActual> action)
        {
            expectedCollection.GroupJoin(actualCollection, expectedKeySelector, actualKeySelector,
                  (Expected, Actual) => (Expected, Actual))
                    .SelectMany(x => x.Actual.DefaultIfEmpty(),
                    (item, Actual) => (item.Expected, Actual))
                    .ToList().ForEach(x =>
                    {
                        try
                        {
                            action(x.Expected, x.Actual);
                        }
                        catch (Exception ex)
                        {
                            throw new MultipleCollectionException(expectedCollection, x.Expected, x.Actual, expectedKeySelector(x.Expected).ToString(), ex);
                        }
                    });
        }
    }
}