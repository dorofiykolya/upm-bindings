using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

namespace Common.Bindings
{
    public class GenericBindableWidget : APropertyBindableBehaviour
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

#pragma warning disable 649
        [SerializeField] private string[] _keys;
#pragma warning restore 649

        [Bindable]
        public void SetValue(string key, object value)
        {
            if (_keys != null && _keys.Length > 0 && _keys.All(k => k != key))
                throw new ArgumentException("GenericBindableWidget does not contain key: " + key);

            _data[key] = value;
        }

        [Bindable]
        private int GetInt(string key)
        {
            return GetVal<int>(key);
        }

        [Bindable]
        private float GetSingle(string key)
        {
            return GetVal<float>(key);
        }

        [Bindable]
        private Sprite GetSprite(string key)
        {
            return GetVal<Sprite>(key);
        }

        [Bindable]
        private string GetString(string key)
        {
            return GetVal<string>(key);
        }

        [Bindable]
        private bool GetBoolean(string key)
        {
            return GetVal<bool>(key);
        }

        [Bindable]
        private Collection GetCollection(string key)
        {
            return GetVal<Collection>(key);
        }

        [Bindable]
        private Action GetAction(string key)
        {
            return GetVal<Action>(key);
        }

        [Bindable]
        private Color GetColor(string key)
        {
            return GetVal<Color>(key);
        }

        private T GetVal<T>(string key)
        {
            object val;
            if (_data.TryGetValue(key, out val)) return (T)val;

            return default;
        }

        public Action Click;

        public void DoClick()
        {
            var handler = Click;
            if (handler != null)
                handler();
        }
    }
}