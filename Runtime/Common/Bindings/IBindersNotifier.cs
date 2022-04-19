using System;

namespace Common.Bindings
{
    public interface IBindersNotifier
    {
        void AttachBinder(ABinder binder);
        void DetachBinder(ABinder binder);

        bool ReadyForBind { get; }
    }
}