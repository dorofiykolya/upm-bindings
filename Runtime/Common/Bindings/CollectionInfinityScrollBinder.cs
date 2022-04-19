using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Bindings
{
    [BindTo(typeof(Collection))]
    public class CollectionInfinityScrollBinder : ABinder
    {
#pragma warning disable 649
        [Tooltip("Container for items")] [SerializeField]
        private Transform _container;

        [Tooltip("Prefab to instantiate in collection")] [SerializeField]
        private GameObject _prefab;

        [Tooltip("Parent prefab for items")] [SerializeField]
        private GameObject _parentPrefab;

        [Tooltip("Items count in parent")] [SerializeField]
        private int _elementsCountInParentPrefab;

        [SerializeField] private bool _byFrame;
#pragma warning restore 649

        [Tooltip("Method to setup data to component from item (Item, Data, IsNew)")]
        public SetupEvent OnSetup = new SetupEvent();

        public UnityEvent OnBegin = new UnityEvent();
        public UnityEvent OnComplete = new UnityEvent();

        private Func<Collection> _getter;
        private Dictionary<object, GameObject> _items;
        private Dictionary<GameObject, List<object>> _groupedItems;

        private Coroutine _coroutine;

        [Serializable]
        public class SetupEvent : UnityEvent<GameObject, List<object>>
        {
        }

        protected override void Bind(bool init)
        {
            if (_container == null || _prefab == null || _getter == null)
                return;

            _prefab.SetActive(false);

            FillItems();
        }

        private void FillItems()
        {
            OnBegin?.Invoke();

            var collection = _getter.Invoke();

            if (collection.IsEmpty) //clear all items
            {
                foreach (var item in _items)
                    Destroy(item.Value);
                _items.Clear();
            }
            else //fill items
            {
                if (_byFrame)
                    foreach (var item in _items)
                        item.Value.SetActive(false);

                if (_coroutine != null) StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(FillCoroutine(collection));

                //clear items, who doesn't exists in collection
            }
        }

        private IEnumerator FillCoroutine(Collection collection)
        {
            if (collection.Comparer != null && collection.Comparer != _items.Comparer)
                _items = new Dictionary<object, GameObject>(_items, collection.Comparer);

            var index = 0;
            var itemsKeys = new HashSet<object>(_items.Comparer);
            GameObject parentObj = null;
            var _itemsGroup = new List<object>();

            foreach (var data in collection)
            {
                GameObject itemObj;

                //bool isNew = false;
                if (index % _elementsCountInParentPrefab == 0)
                {
                    parentObj = Instantiate(_parentPrefab);
                    parentObj.transform.SetParent(_container, false);
                    parentObj.transform.localScale = Vector3.one;
                }

                //doesn't have -> create new one
                var key = _items.Keys.FirstOrDefault(k => _items.Comparer.Equals(k, data));

                if (key == null)
                {
                    itemObj = Instantiate(_prefab);
                    itemObj.transform.SetParent(parentObj.transform, false);
                    itemObj.transform.localScale = Vector3.one;
                    _itemsGroup.Add(data);
                    _items.Add(data, itemObj);
                    //isNew = true;
                }
                else
                {
                    itemObj = _items[key];
                }

                if (index > 0 && index % _elementsCountInParentPrefab == 0)
                {
                    _groupedItems.Add(parentObj, _itemsGroup);

                    if (OnSetup != null)
                        OnSetup.Invoke(parentObj, _itemsGroup);
                    _itemsGroup.Clear();
                }


                itemObj.transform.SetSiblingIndex(index);


                if (!itemObj.activeSelf)
                    itemObj.SetActive(true);

                itemsKeys.Add(data);
                index++;
                if (_byFrame)
                    yield return null;
            }

            var itemsToRemove = _items.Keys.Where(key => !itemsKeys.Any(k => _items.Comparer.Equals(k, key))).ToArray();
            foreach (var itemKey in itemsToRemove)
            {
                Destroy(_items[itemKey]);
                _items.Remove(itemKey);
            }

            OnComplete?.Invoke();
            _coroutine = null;
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }

        private void Awake()
        {
            Init(ref _getter);
            _items = new Dictionary<object, GameObject>();
            _groupedItems = new Dictionary<GameObject, List<object>>();
        }
    }
}