using System;
using UnityEngine;

namespace Common.Bindings
{
    [RequireComponent(typeof(Animator))]
    [BindTo(typeof(bool))]
    public class AnimationBooleanParameterBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private string _parameterName;
        [SerializeField] private Animator _animator;
#pragma warning restore 649

        private Func<bool> _getter;

        protected override void Bind(bool init)
        {
            var isTrue = _getter();
            _animator.SetBool(_parameterName, isTrue);
        }

        private void Awake()
        {
            Init(ref _getter);

            if (_animator == null) _animator = GetComponent<Animator>();

            ;
        }
    }
}