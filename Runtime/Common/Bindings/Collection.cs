using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.Bindings
{
    public struct Collection : IEnumerable<object>
    {
        public static Collection Default = new Collection();

        private readonly IEnumerable<object> _collection;
        private readonly IEqualityComparer<object> _comparer;

        public IEqualityComparer<object> Comparer => _comparer;

        public bool IsEmpty => _collection == null || !_collection.GetEnumerator().MoveNext();

        public Collection(IEnumerable collection, IEqualityComparer<object> comparer = null)
        {
            _collection = collection.Cast<object>();
            _comparer = comparer;
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            if (_collection == null)
            {
                Debug.LogError($"{nameof(Collection)}.{nameof(_collection)} is null");
                yield break;
            }

            foreach (var item in _collection) yield return item;
        }

        public IEnumerator GetEnumerator()
        {
            if (_collection == null)
            {
                Debug.LogError($"{nameof(Collection)}.{nameof(_collection)} is null");
                yield break;
            }

            foreach (var item in _collection) yield return item;
        }
    }
}