using System;
using System.Collections;
using UnityEngine;

namespace Common.Bindings
{
    [BindTo(typeof(bool))]
    public class AnimatorBinder : ABinder
    {
#pragma warning disable 649
        [SerializeField] private Animator _animator;
        [SerializeField] private string _variableName;
        [SerializeField] private bool _isBoolean;
        [SerializeField] private bool _isInverse;
#pragma warning restore 649

        private Func<bool> _getter;
        private Coroutine _coroutine;

        protected override void Bind(bool init)
        {
            if (_isBoolean)
                Coroutine(WaitAnimatorInitialization(SetBool));
            else if (IsGetter()) Coroutine(WaitAnimatorInitialization(SetTrigger));
        }

        private void SetBool()
        {
            if (string.IsNullOrEmpty(_variableName)) return;
            _animator.SetBool(_variableName, _getter());
        }

        private void SetTrigger()
        {
            if (string.IsNullOrEmpty(_variableName)) return;
            _animator.SetTrigger(_variableName);
        }

        private void StopCoroutine()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private void Coroutine(IEnumerator enumerator)
        {
            StopCoroutine();
            _coroutine = StartCoroutine(enumerator);
        }

        private IEnumerator WaitAnimatorInitialization(Action action)
        {
            while (!_animator.isInitialized) yield return null;

            action();
            StopCoroutine();
        }

        private bool IsGetter()
        {
            return _isInverse ? !_getter() : _getter();
        }

        protected override void OnDisable()
        {
            StopCoroutine();
            base.OnDisable();
        }

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            Init(ref _getter);
        }
    }
}