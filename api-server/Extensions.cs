using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Sheep.IHeartFiction.ApiServer
{
    public static class Extensions
    {
        public static Guid GetNextGuid<TValue>(this IEnumerable<TValue> collection) where TValue : IDualKeyEntity
        {
            var newGuid = Guid.NewGuid();

            while (collection.Any(e => e.UUID == newGuid))
                newGuid = Guid.NewGuid();

            return newGuid;
        }

        public static int GetNextId<TValue>(this IEnumerable<TValue> collection) where TValue : IDualKeyEntity => collection.Count() > 0 ? collection.Max(e => e.Id) + 1 : 1;

        public static void AssignTo(this object source, object destination, IEnumerable<string>? exceptProperties)
        {
            exceptProperties = exceptProperties ?? new List<string>();
            var destType = destination.GetType();
            var srcType = source.GetType();

            var validMatches = from srcProp in srcType.GetProperties().Where(e => !exceptProperties.Contains(e.Name))
                               let targetProperty = destType.GetProperty(srcProp.Name)
                               where srcProp.CanRead
                               && targetProperty != null
                               && targetProperty.GetSetMethod(true) != null
                               && !(targetProperty.GetSetMethod(true)?.IsPrivate ?? true)
                               && (targetProperty.GetSetMethod()?.Attributes & MethodAttributes.Static) == 0
                               && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                               select new { sourceProperty = srcProp, targetProperty };

            foreach (var match in validMatches)
                match.targetProperty.SetValue(destination, match.sourceProperty.GetValue(source));

        }

        public static TValue? FirstOrNull<TValue>(this IEnumerable<TValue> collection, Func<TValue, bool> selector) where TValue : class
        {
            try
            {
                return collection.First(selector);
            }
            catch (InvalidOperationException)
            {
                // Only swallow the IO since it indicates value not found
            }

            return null;
        }

        public static bool TryGetSome<TValue>(this ICollection<TValue> collection, [MaybeNullWhen(false)] out ICollection<TValue> results, Func<TValue, bool> selector)
        {
            var filtered = new List<TValue>();

            foreach(var item in collection)
                if (selector.Invoke(item))
                    filtered.Add(item);

            if (filtered.Count() > 0)
            {
                results = filtered;
                return true;
            }

            results = null;
            return false;
        }
    }
}
