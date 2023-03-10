using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Common
{
    public sealed class ServiceContainer : IServiceProvider, IDisposable
    {
        private readonly Dictionary<Type, ServiceRegistration> _registrations =
            new Dictionary<Type, ServiceRegistration>();

        public object Unknown { get; set; }

        public ServiceContainer ParentContainer { get; set; }

        public bool Register<T>(Scope scope = Scope.Singleton)
        {
            return Register(typeof(T), scope);
        }

        public bool Register<T>(T instance, Scope scope = Scope.Singleton)
        {
            return Register(typeof(T), instance, scope);
        }

        public bool Register<T>(ObjectCreatorCallback callback, Scope scope = Scope.Singleton)
        {
            return Register(typeof(T), callback, scope);
        }

        public bool Register(Type svcType, Scope scope = Scope.Singleton)
        {
            Guard.ThrowIfNull("svcType", svcType);

            if (FindRegistration(svcType) == null)
            {
                var svcReg = CreateServiceRegistration(svcType, null, null, scope);

                _registrations.Add(svcType, svcReg);

                return true;
            }

            return false;
        }

        public bool Register(Type svcType, object instance, Scope scope = Scope.Singleton)
        {
            Guard.ThrowIfNull("svcType", svcType);
            Guard.ThrowIfNull("instance", instance);
            if (!svcType.IsInstanceOfType(instance))
            {
                throw new ArgumentException("The instance does not implement the specified service type: " + svcType.FullName, "instance");
            }
            if (scope != Scope.Singleton && !(instance is ICloneable) && !(instance is Jacobi.Vst3.Core.ICloneable))
            {
                throw new ArgumentException("The instance needs to implement IClonable if to use with a PerCall scope.", "instance");
            }

            if (FindRegistration(svcType) == null)
            {
                var svcReg = CreateServiceRegistration(svcType, instance, null, scope);

                _registrations.Add(svcType, svcReg);

                return true;
            }

            return false;
        }

        public bool Register(Type svcType, ObjectCreatorCallback callback, Scope scope = Scope.Singleton)
        {
            Guard.ThrowIfNull("svcType", svcType);
            Guard.ThrowIfNull("callback", callback);

            if (FindRegistration(svcType) == null)
            {
                var svcReg = CreateServiceRegistration(svcType, null, callback, scope);

                _registrations.Add(svcType, svcReg);

                return true;
            }

            return false;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public T TryGetService<T>()
        {
            return (T)TryGetService(typeof(T));
        }

        public object TryGetService(Type serviceType)
        {
            var svcReg = FindRegistration(serviceType);

            if (svcReg != null)
            {
                return GetInstance(svcReg);
            }

            if (serviceType.IsInterface && Unknown != null)
            {
                var intf = Marshal.GetComInterfaceForObject(Unknown, serviceType);
                return Marshal.GetObjectForIUnknown(intf);
            }

            if (ParentContainer != null)
            {
                return ParentContainer.GetService(serviceType);
            }

            return null;
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            var instance = TryGetService(serviceType);

            if (instance == null)
            {
                throw new ArgumentException(
                    "The requested Service Type '" + serviceType.FullName + "' was not found.",
                    nameof(serviceType)
                );
            }

            return instance;
        }

        #endregion

        private ServiceRegistration FindRegistration(Type svcType)
        {

            if (_registrations.TryGetValue(svcType, out ServiceRegistration svcReg))
            {
                return svcReg;
            }

            return null;
        }

        private object GetInstance(ServiceRegistration svcReg)
        {
            object instance = null;

            if (svcReg.Instance == null)
            {
                if (svcReg.Callback != null)
                {
                    instance = svcReg.Callback(this, svcReg.ServiceType);
                }
                else
                {
                    instance = Activator.CreateInstance(svcReg.ServiceType);
                }

                if (svcReg.Scope == Scope.Singleton)
                {
                    svcReg.Instance = instance;
                }
            }
            else // Instance != null
            {
                if (svcReg.Scope == Scope.PerCall)
                {
                    var comCloneable = svcReg.Instance as Core.ICloneable;

                    if (svcReg.Instance is ICloneable cloneable)
                    {
                        instance = cloneable.Clone();
                    }
                    else if (comCloneable != null)
                    {
                        instance = comCloneable.Clone();
                    }
                    else
                    {
                        throw new InvalidOperationException("Cannot clone Service instance for PerCall service request.");
                        //instance = svcReg.Instance;
                    }
                }
                else
                {
                    instance = svcReg.Instance;
                }
            }

            return instance;
        }

        private ServiceRegistration CreateServiceRegistration(Type svcType, object instance, ObjectCreatorCallback callback, Scope scope)
        {
            return new ServiceRegistration()
            {
                ServiceType = svcType,
                Instance = instance,
                Callback = callback,
                Scope = scope,
            };
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (var svcReg in _registrations.Values)
            {
                svcReg.Dispose();
            }

            _registrations.Clear();

            ParentContainer = null;
            Unknown = null;

            GC.SuppressFinalize(this);
        }

        #endregion

        //---------------------------------------------------------------------

        public enum Scope
        {
            Singleton,
            PerCall,
        }

        //---------------------------------------------------------------------

        private sealed class ServiceRegistration : IDisposable
        {
            public Type ServiceType;
            public object Instance;
            public ObjectCreatorCallback Callback;
            public Scope Scope { get; set; }

            public void Dispose()
            {
                if (Instance is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
