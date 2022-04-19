using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Bindings
{
    [BindTo(typeof(Collection))]
    public class CollectionBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] [Tooltip("Container for items")]
        private Transform _container;

        [SerializeField] [Tooltip("Prefab to instantiate in collection")]
        private GameObject _prefab;

        [SerializeField] [Tooltip("Methot to setup data to component from item (Item, Data, IsNew)")]
        private SetupEvent _setupEvent = new SetupEvent();

        [SerializeField] private UnityEvent _onBegin = new UnityEvent();
        [SerializeField] private UnityEvent _onComplete = new UnityEvent();
        [SerializeField] private bool _useInvertSibling = true;
        [SerializeField] private bool _byFrame;
#pragma warning restore 649

        private Func<Collection> _getter;
        private List<CollectionItemInfo> _items;
        private Coroutine _coroutine;
        private List<CollectionItemInfo> _pooledItems;
        private GameObject _pooledItemContainer;

        [Serializable]
        public class SetupEvent : UnityEvent<GameObject, object, bool>
        {
        }

        private void Start()
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
            _onBegin?.Invoke();

            var collection = _getter.Invoke();
            if (collection.IsEmpty)
            {
                ReturnAllToPool();
                _onComplete?.Invoke();
                return;
            }

            if (gameObject.activeSelf && _byFrame)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                ReturnAllToPool();
                _coroutine = StartCoroutine(FillCoroutine(collection));
            }
            else
            {
                var index = 0;
                var tempList = new List<CollectionItemInfo>();
                foreach (var data in collection)
                {
                    var itemObj =
                        _items.FirstOrDefault(info => info.Data.Equals(data)) ?? _items.FirstOrDefault();

                    if (itemObj == null)
                        itemObj = GetPooledObj(data);
                    else
                        _items.Remove(itemObj);

                    tempList.Add(itemObj);

                    _setupEvent?.Invoke(itemObj.Object, data, itemObj.IsNew);
                    if (_useInvertSibling) itemObj.Object.transform.SetSiblingIndex(index++);
                }

                ReturnAllToPool();

                _items = tempList;
                _onComplete?.Invoke();
            }

            ResetMonoPoolItems();
        }

        private void ResetMonoPoolItems()
        {
            foreach (var item in _items.Where(info => !info.Object.activeSelf))
                if (item.MonoPoolItem != null)
                    item.MonoPoolItem.Reset();
        }

        private void ReturnAllToPool()
        {
            while (_items.Count > 0)
            {
                var item = _items.FirstOrDefault();
                if (item != null)
                    ReturnToPool(item);
            }
        }

        private IEnumerator FillCoroutine(Collection collection)
        {
            var index = 0;
            foreach (var data in collection)
            {
                var itemObj = GetPooledObj(data);
                _items.Add(itemObj);
                _setupEvent?.Invoke(itemObj.Object, itemObj.Data, itemObj.IsNew);
                itemObj.Object.transform.SetSiblingIndex(index);

                index++;

                if (_byFrame)
                    yield return null;
            }

            _onComplete?.Invoke();
            _coroutine = null;
        }

        private CollectionItemInfo GetPooledObj(object data)
        {
            var item = _pooledItems.FirstOrDefault(info => info.Data.Equals(data)) ??
                       _pooledItems.LastOrDefault();

            if (item == null)
            {
                var instance = Instantiate(_prefab, _container);
                item = new CollectionItemInfo()
                {
                    Object = instance,
                    MonoPoolItem = instance.GetComponent<IMonoPoolItem>(),
                    Data = data,
                    IsNew = true
                };
                item.Object.transform.localScale = Vector3.one;
            }
            else
            {
                _pooledItems.Remove(item);
                item.Data = data;
                item.IsNew = false;
            }

            item.Object.transform.SetParent(_container, false);
            item.Object.SetActive(true);

            return item;
        }

        private void ReturnToPool(CollectionItemInfo item)
        {
            item.Object.SetActive(false);
            item.Object.transform.SetParent(_pooledItemContainer.transform, false);
            _items.Remove(item);
            _pooledItems.Add(item);
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }

        private void Awake()
        {
            _pooledItemContainer = new GameObject("PooledItems");
            _pooledItemContainer.SetActive(false);
            _pooledItemContainer.transform.SetParent(transform);
            _items = new List<CollectionItemInfo>();
            _pooledItems = new List<CollectionItemInfo>();

            Init(ref _getter);
        }

        private class CollectionItemInfo
        {
            public object Data;
            public GameObject Object;
            public IMonoPoolItem MonoPoolItem;
            public bool IsNew;
        }
    }

    public interface IMonoPoolItem
    {
        void Reset();
    }
}