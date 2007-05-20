using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Security.Permissions;

namespace PublicDomain
{
    /// <summary>
    /// Methods to help in common Reflection tasks.
    /// </summary>
    public static class ReflectionUtilities
    {
        /// <summary>
        /// Gets the name of the strong.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static System.Security.Policy.StrongName GetStrongName(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            AssemblyName assemblyName = assembly.GetName();

            // get the public key blob
            byte[] publicKey = assemblyName.GetPublicKey();
            if (publicKey == null || publicKey.Length == 0)
                throw new InvalidOperationException(
                    String.Format("{0} is not strongly named",
                    assembly));

            StrongNamePublicKeyBlob keyBlob =
                new StrongNamePublicKeyBlob(publicKey);

            // create the StrongName
            return new System.Security.Policy.StrongName(
                keyBlob, assemblyName.Name, assemblyName.Version);
        }

        /// <summary>
        /// Finds the type by interface.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static Type FindTypeByInterface<T>(Assembly assembly) where T : class
        {
            Type[] types = assembly.GetTypes();
            Type interfaceType;
            foreach (Type type in types)
            {
                interfaceType = type.GetInterface(typeof(T).ToString(), false);
                if (interfaceType != null)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the instance by interface.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static T FindInstanceByInterface<T>(Assembly assembly) where T : class
        {
            Type type = FindTypeByInterface<T>(assembly);
            return assembly.CreateInstance(type.FullName) as T;
        }

        /// <summary>
        /// Finds the method.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="methodName">Fully qualified name of the method.</param>
        /// <returns></returns>
        public static MethodInfo FindMethod(Assembly assembly, string methodName)
        {
            string[] pieces = StringUtilities.SplitAround(methodName, methodName.LastIndexOf('.'));

            Type type = assembly.GetType(pieces[0], false, true);

            if (type != null)
            {
                return type.GetMethod(pieces[1]);
            }

            return null;
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static object InvokeMethod(MethodInfo methodInfo, params object[] parameters)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            if (methodInfo.IsStatic)
            {
                return methodInfo.Invoke(null, parameters);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static object InvokeMethod(Assembly assembly, string methodName, params object[] parameters)
        {
            MethodInfo methodInfo = FindMethod(assembly, methodName);
            if (methodInfo == null)
            {
                throw new ReflectionException(string.Format("Could not find method {0} in assembly {1}.", methodName, assembly));
            }
            return InvokeMethod(methodInfo, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class ReflectionException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ReflectionException"/> class.
            /// </summary>
            public ReflectionException() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="ReflectionException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public ReflectionException(string message) : base(message) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="ReflectionException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="inner">The inner.</param>
            public ReflectionException(string message, Exception inner) : base(message, inner) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="ReflectionException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected ReflectionException(
              SerializationInfo info,
              StreamingContext context)
                : base(info, context) { }
        }
    }
}
