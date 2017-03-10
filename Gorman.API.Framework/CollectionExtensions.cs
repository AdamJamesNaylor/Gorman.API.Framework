namespace Gorman.API.Framework {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class CollectionExtensions {
        public static void AddRange<T>(this Collection<T> operand, IEnumerable<T> items) {
            if (items == null)
                return;

            foreach (var item in items) {
                operand.Add(item);
            }
        }
    }
}