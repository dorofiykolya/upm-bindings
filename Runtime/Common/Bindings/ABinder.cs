using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Bindings
{
    public abstract class ABinder : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Component _target;
        [SerializeField] private string _memberName;
        [SerializeField] private string _params;
#pragma warning restore 649

        public Component Target => _target;

        public string MemberName => _memberName;

        public static void Bind(GameObject go)
        {
            go.SendMessage("Bind", SendMessageOptions.DontRequireReceiver);
        }

        public static void BindBroadcast(GameObject go)
        {
            go.BroadcastMessage("Bind", SendMessageOptions.DontRequireReceiver);
        }

        [ContextMenu("Rebind")]
        public void Rebind()
        {
            SafeBind(false);
        }

        public override string ToString()
        {
            if (Target == null)
                return name + "->" + GetType().Name;

            return name + "->" + GetType().Name + " On " + Target.name + "->" + Target.GetType().Name + "." +
                   MemberName;
        }

        protected abstract void Bind(bool init);

        protected void Init<TArg>(ref Action<TArg> action, bool requereSetter = true)
        {
            var bindSource = new BindSource { Target = _target, MemberName = _memberName, Params = _params };

            Init(ref action, ref bindSource, requereSetter);
        }

        protected void Init<TResult>(ref Func<TResult> func, bool requireGetter = true)
        {
            var bindSource = new BindSource { Target = _target, MemberName = _memberName, Params = _params };

            Init(ref func, ref bindSource, requireGetter);
        }

        protected void Init<TResult, TArg>(ref Func<TResult, TArg> func, bool requireGetter = true)
        {
            var bindSource = new BindSource { Target = _target, MemberName = _memberName, Params = _params };

            Init<TResult, TArg>(ref func, ref bindSource, requireGetter);
        }

        private void Init<TArg, TResult>(ref Func<TArg, TResult> func, ref BindSource bindSource, bool requireGetter)
        {
            try
            {
                var type = bindSource.Target.GetType();

                do
                {
                    var method = type.GetMethod(bindSource.MemberName,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (method != null)
                    {
                        func = (Func<TArg, TResult>)Delegate.CreateDelegate(typeof(Func<TArg, TResult>),
                            bindSource.Target, method);
                        return;
                    }

                    type = type.BaseType;
                } while (type != typeof(Object));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }

            if (requireGetter)
                Debug.LogError(
                    "[ABinder] - Init Fail: " + bindSource.Target.name + "->" + bindSource.Target.GetType().Name + "." +
                    bindSource.MemberName + " has no getter", this);

            //else
            //    Debug.Log("[ABinder] - Property " + bindSource.Target.name + "->" + bindSource.Target.GetType().Name + "." + bindSource.MemberName + " has no setter. Binder set logic will not work.", this);
        }

        protected void Init<TArg>(ref Action<TArg> action, ref BindSource bindSource, bool requereSetter = true)
        {
            try
            {
                var type = bindSource.Target.GetType();

                do
                {
                    var prop = type.GetProperty(bindSource.MemberName,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (prop != null)
                    {
                        var propSetter = prop.GetSetMethod(true);

                        if (propSetter != null)
                        {
                            action = (Action<TArg>)Delegate.CreateDelegate(typeof(Action<TArg>), bindSource.Target,
                                propSetter);
                            return;
                        }
                    }

                    type = type.BaseType;
                } while (type != typeof(Object));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }

            if (requereSetter)
                Debug.LogError(
                    "[ABinder] - Init Fail: " + bindSource.Target.name + "->" + bindSource.Target.GetType().Name + "." +
                    bindSource.MemberName + " has no setter", this);

            //else
            //    Debug.Log("[ABinder] - Property " + bindSource.Target.name + "->" + bindSource.Target.GetType().Name + "." + bindSource.MemberName + " has no setter. Binder set logic will not work.", this);
        }

        protected void Init<TResult>(ref Func<TResult> action, ref BindSource bindSource, bool requireGetter = true)
        {
            try
            {
                var type = bindSource.Target.GetType();

                do
                {
                    var prop = type.GetProperty(bindSource.MemberName,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (prop != null)
                        if (prop.GetCustomAttributes(typeof(BindableAttribute), true).Length > 0)
                        {
                            var propGetter = prop.GetGetMethod(true);

                            if (propGetter.ReturnType == typeof(Enum) && typeof(TResult) != typeof(Enum))
                            {
                                var delegat = (Func<Enum>)Delegate.CreateDelegate(typeof(Func<Enum>),
                                    bindSource.Target, propGetter);

                                action = () => (TResult)(object)delegat();
                            }
                            else
                            {
                                action = (Func<TResult>)Delegate.CreateDelegate(typeof(Func<TResult>),
                                    bindSource.Target, propGetter);
                            }

                            return;
                        }

                    foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public |
                                                           BindingFlags.NonPublic))
                    {
                        if (method.Name != bindSource.MemberName ||
                            !typeof(TResult).IsAssignableFrom(method.ReturnType) ||
                            method.GetCustomAttributes(typeof(BindableAttribute), true).Length == 0)
                            continue;

                        action = BindMethod<TResult>(bindSource.Target, method, bindSource.Params);

                        if (action != null)
                            return;
                    }

                    type = type.BaseType;
                } while (type != typeof(Object));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }

            if (requireGetter)
                Debug.LogError(
                    "[ABinder] - Init Fail: " + bindSource.Target.name + "->" + bindSource.Target.GetType().Name + "." +
                    bindSource.MemberName + " has no getter", this);

            else
                Debug.Log(
                    "[ABinder] - Property " + bindSource.Target.name + "->" + bindSource.Target.GetType().Name + "." +
                    bindSource.MemberName + " has no getter. Binder get logic will not work.", this);
        }

        //
        //private					void			Init<TResult, TArg>			( ref Func<TResult, TArg> action, Object target, String memberName )	
        //{
        //	var type		= target.GetType( );

        //	foreach( var method in type.GetMethods( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) )
        //	{
        //		if( method.Name != memberName || method.ReturnType != typeof(TResult) || method.GetCustomAttributes( typeof(BindableAttribute), true ).Length == 0 )
        //			continue;

        //		var @params = method.GetParameters( );

        //		if( @params.Length == 0 || @params[0].ParameterType != typeof(TArg) )
        //			continue;

        //		action = BindMethod<TResult, TArg>( target, method );

        //		if( action == null )
        //			continue;
        //	}

        //	if( action == null )
        //		Debug.LogError		( "[ABinder] - Init Fail: " + target.GetType ( ).Name + " " + target.name + "->" + _property , this );
        //}

        protected virtual void OnEnable()
        {
            if (_target is IBindersNotifier target2)
            {
                target2.AttachBinder(this);

                if (target2.ReadyForBind)
                    SafeBind(true);

                return;
            }

            SafeBind(true);
        }

        protected virtual void OnDisable()
        {
            if (_target is IBindersNotifier target2)
                target2.DetachBinder(this);
        }

        protected virtual void OnDestroy()
        {
            if (_target is IBindersNotifier target2)
                target2.DetachBinder(this);
        }

        private void SafeBind(bool init)
        {
            try
            {
                Bind(init);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
        }

        private void RebindOnPropertyChanged(string prop)
        {
            if (prop != "*" && prop != _memberName)
                return;

            SafeBind(false);
        }

        private Func<TResult> BindMethod<TResult>(Object target, MethodInfo method, string parameters)
        {
            var @params = method.GetParameters();
            if (@params.Length == 0)
                return (Func<TResult>)Delegate.CreateDelegate(typeof(Func<TResult>), target, method);

            if (@params.Length == 1)
            {
                if (@params[0].ParameterType == typeof(string))
                    return new StringParamBinder<TResult>
                    {
                        Param = parameters,
                        Function = (Func<string, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult>),
                            target, method)
                    }.GetValue;

                if (@params[0].ParameterType == typeof(int))
                {
                    var result = 0;
                    if (int.TryParse(parameters, out result))
                        return new IntParamBinder<TResult>
                        {
                            Param = result,
                            Function = (Func<int, TResult>)Delegate.CreateDelegate(typeof(Func<int, TResult>),
                                target, method)
                        }.GetValue;
                }

                if (@params[0].ParameterType == typeof(float))
                {
                    var result = 0.0f;
                    if (float.TryParse(parameters, out result))
                        return new SingleParamBinder<TResult>
                        {
                            Param = result,
                            Function = (Func<float, TResult>)Delegate.CreateDelegate(typeof(Func<float, TResult>),
                                target, method)
                        }.GetValue;
                }

                if (@params[0].ParameterType == typeof(bool))
                {
                    var result = false;
                    if (bool.TryParse(parameters, out result))
                        return new BooleanParamBinder<TResult>
                        {
                            Param = result,
                            Function = (Func<bool, TResult>)Delegate.CreateDelegate(typeof(Func<bool, TResult>),
                                target,
                                method)
                        }.GetValue;
                }

                if (@params[0].ParameterType == typeof(Enum))
                {
                    var attributes = (BindableAttribute[])method.GetCustomAttributes(typeof(BindableAttribute), true);
                    var result = (Enum)Enum.Parse(attributes[0].ArgumentType, parameters);

                    return new EnumParamBinder<TResult>
                    {
                        Param = result,
                        Function = (Func<Enum, TResult>)Delegate.CreateDelegate(typeof(Func<Enum, TResult>), target,
                            method)
                    }.GetValue;
                }

                if (@params[0].ParameterType == typeof(Object))
                    return new ObjectParamBinder<TResult>
                    {
                        Param = gameObject,
                        Function = (Func<Object, TResult>)Delegate.CreateDelegate(typeof(Func<Object, TResult>),
                            target, method)
                    }.GetValue;
            }

            return null;
        }

        [Serializable]
        public struct BindSource
        {
            public Component Target;
            public string MemberName;
            public string Params;
        }

        #region | SubTypes |

        private class StringParamBinder<TType>
        {
            public string Param;
            public Func<string, TType> Function;

            public TType GetValue()
            {
                return Function(Param);
            }
        }

        private class IntParamBinder<TType>
        {
            public int Param;
            public Func<int, TType> Function;

            public TType GetValue()
            {
                return Function(Param);
            }
        }

        private class SingleParamBinder<TType>
        {
            public float Param;
            public Func<float, TType> Function;

            public TType GetValue()
            {
                return Function(Param);
            }
        }

        private class BooleanParamBinder<TType>
        {
            public bool Param;
            public Func<bool, TType> Function;

            public TType GetValue()
            {
                return Function(Param);
            }
        }

        private class EnumParamBinder<TType>
        {
            public Enum Param;
            public Func<Enum, TType> Function;

            public TType GetValue()
            {
                return Function(Param);
            }
        }

        private class ObjectParamBinder<TType>
        {
            public Object Param;
            public Func<Object, TType> Function;

            public TType GetValue()
            {
                return Function(Param);
            }
        }

        #endregion

        //
        //[Serializable]
        //private class PropertyRef
        //{
        //	[SerializeField]
        //	private					Component		_target;
        //	[SerializeField]
        //	private					String			_property;

        //	public					Func<TType>		Bind<TType>					( )								
        //	{
        //		var @delegate = Bind ( typeof(TType) );
        //		return (Func<TType>)@delegate;
        //	}

        //	private					Delegate		Bind						( Type returnType )				
        //	{
        //		var type				= _target.GetType	( );
        //		var prop				= type.GetProperty	( _property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
        //		var propGetter			= prop.GetGetMethod	( true );

        //		if( prop.PropertyType.IsSubclassOf ( typeof(Boolean) ) )
        //			return prop.PropertyType == returnType ? Delegate.CreateDelegate( typeof(Func<Boolean>), _target, propGetter )	: ConvertToString ( (Func<Boolean>)Delegate.CreateDelegate( typeof(Func<Boolean>), _target, propGetter ) );

        //		if( prop.PropertyType.IsSubclassOf ( typeof(Int32) ) )
        //			return prop.PropertyType == returnType ? Delegate.CreateDelegate( typeof(Func<Int32>), _target, propGetter )	: ConvertToString ( (Func<Int32>)Delegate.CreateDelegate( typeof(Func<Int32>), _target, propGetter ) );

        //		if( prop.PropertyType.IsSubclassOf ( typeof(Single) ) )
        //			return prop.PropertyType == returnType ? Delegate.CreateDelegate( typeof(Func<Single>), _target, propGetter )	: ConvertToString ( (Func<Single>)Delegate.CreateDelegate( typeof(Func<Single>), _target, propGetter ) );

        //		if( prop.PropertyType.IsSubclassOf ( typeof(String) ) )
        //			return prop.PropertyType == returnType ? Delegate.CreateDelegate( typeof(Func<String>), _target, propGetter )	: ConvertToString ( (Func<String>)Delegate.CreateDelegate( typeof(Func<String>), _target, propGetter ) );

        //		if( prop.PropertyType.IsSubclassOf ( typeof(Vector3) ) )
        //			return prop.PropertyType == returnType ? Delegate.CreateDelegate( typeof(Func<Vector3>), _target, propGetter )	: ConvertToString ( (Func<Vector3>)Delegate.CreateDelegate( typeof(Func<Vector3>), _target, propGetter ) );

        //		throw new NotImplementedException( "Binder of type " + prop.PropertyType + "is not inmplemented!" );
        //	}

        //	private					Func<String>	ConvertToString<TType>		( Func<TType> @delegate )		
        //	{
        //		return ( ) => @delegate ( ).ToString ( );
        //	}
        //}
        public static class Internal
        {
            public static void RebindOn(ABinder binder, string propertyName)
            {
                binder.RebindOnPropertyChanged(propertyName);
            }
        }
    }
}