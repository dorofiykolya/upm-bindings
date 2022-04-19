using System;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(string))]
    public class StringEqualBinder : ABinder
    {
#pragma warning disable 649
        [Serializable]
        private struct StringBindingPair
        {
            public string Str;
            public GameObject Obj;
        }

        [SerializeField] private StringBindingPair[] _binds;
#pragma warning restore 649

        private Func<string> _getter;

        protected override void Bind(bool init)
        {
            var key = _getter();

            GameObject activeItem = null;
            foreach (var i in _binds)
            {
                if (i.Str == key)
                    activeItem = i.Obj;
                else
                    i.Obj.SetActive(false);

                if (activeItem != null) activeItem.SetActive(true);
            }
        }

        private void Awake()
        {
            Init(ref _getter);
        }
    }
}