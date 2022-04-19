using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Bindings
{
    public abstract class APropertyBindableBehaviour : MonoBehaviour, IBindersNotifier
    {
        private readonly List<ABinder> _attachedBinders = new List<ABinder>();

        private bool _readyForBind = false;

        public bool ReadyForBind => _readyForBind;

        void IBindersNotifier.AttachBinder(ABinder binder)
        {
            _attachedBinders.Add(binder);
        }

        void IBindersNotifier.DetachBinder(ABinder binder)
        {
            _attachedBinders.Remove(binder);
        }

        public void MakeBindReadyAndRebindAll()
        {
            _readyForBind = true;
            RaisePropertyChanged("*");
        }

        public void MakeBindReady()
        {
            _readyForBind = true;
        }

        public void MakeBindUnready()
        {
            _readyForBind = false;
        }

        protected void RebindAll()
        {
            RaisePropertyChanged("*");
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (_attachedBinders == null)
                return;

            for (var i = 0; i < _attachedBinders.Count; i++)
            {
                var binder = _attachedBinders[i];
                var unityObject = binder.Target as UnityEngine.Object;

                if (unityObject != null && !unityObject)
                {
                    _attachedBinders.RemoveAt(i);
                    i--;
                    continue;
                }

                try
                {
                    ABinder.Internal.RebindOn(binder, propertyName);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        public void Reset()
        {
            MakeBindUnready();
        }
    }
}