using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NuiN.NExtensions
{
    public static class Extensions
    {
        /// <summary>
        /// Checks to see if the containing value is valid and returns it through the out parameter if so
        /// </summary>
        /// <param name="SerializedInterface"></param>
        /// <param name="value">The containing value, if valid</param>
        /// <returns>True if the containing value is valid, false if not</returns>
        public static bool IsDefined<TInterface>(
            this SerializedInterface<TInterface> SerializedInterface,
            out TInterface value
        )
            where TInterface : class
        {
            if (SerializedInterface == null)
            {
                value = default;
                return false;
            }

            if (EqualityComparer<TInterface>.Default.Equals(SerializedInterface.Value, default))
            {
                value = default;
                return false;
            }

            value = SerializedInterface.Value;
            return true;
        }

        /// <inheritdoc cref="IsDefined{TInterface}"/>
        public static bool TryGetValue<TInterface>(
            this SerializedInterface<TInterface> SerializedInterface,
            out TInterface value
        )
            where TInterface : class
        {
            return IsDefined(SerializedInterface, out value);
        }

        /// <summary>
        /// Convert a IEnumerable of Interfaces to a List of SerializedInterfaces
        /// </summary>
        public static List<SerializedInterface<T>> ToSerializedInterfaceList<T>(this IEnumerable<T> list) where T : class
        {
            return list.Select(e => new SerializedInterface<T>(e)).ToList();
        }

         /// <summary>
        /// Convert a IEnumerable of Interfaces to an Array of SerializedInterfaces
        /// </summary>
        public static SerializedInterface<T>[] ToSerializedInterfaceArray<T>(this IEnumerable<T> list) where T : class
        {
            return list.Select(e => new SerializedInterface<T>(e)).ToArray();
        }

        public static TInterface Instantiate<TInterface>(this SerializedInterface<TInterface> SerializedInterface) where TInterface : class
        {
            if (!SerializedInterface.TryGetObject(out Object unityObject)) 
            {
                throw new System.Exception($"Cannot instantiate {SerializedInterface} because it's has no reference of type UnityEngine.Object");
            }

            Object instantiatedObject = Object.Instantiate(unityObject);

            return GetInterfaceReference<TInterface>(instantiatedObject);
        }

        public static TInterface Instantiate<TInterface>(this SerializedInterface<TInterface> SerializedInterface, Transform parent) where TInterface : class
        {
            if (!SerializedInterface.TryGetObject(out Object unityObject))
            {
                throw new System.Exception($"Cannot instantiate {SerializedInterface} because it's has no reference of type UnityEngine.Object");
            }

            Object instantiatedObject = Object.Instantiate(unityObject, parent);

            return GetInterfaceReference<TInterface>(instantiatedObject);
        }        

        public static TInterface Instantiate<TInterface>(this SerializedInterface<TInterface> SerializedInterface, Vector3 position, Quaternion rotation) where TInterface : class
        {
            if (!SerializedInterface.TryGetObject(out Object unityObject))
            {
                throw new System.Exception($"Cannot instantiate {SerializedInterface} because it's has no reference of type UnityEngine.Object");
            }

            Object instantiatedObject = Object.Instantiate(unityObject, position, rotation);

            return GetInterfaceReference<TInterface>(instantiatedObject);
        }

        public static TInterface Instantiate<TInterface>(this SerializedInterface<TInterface> SerializedInterface, Vector3 position, Quaternion rotation, Transform parent) where TInterface : class
        {
            if (!SerializedInterface.TryGetObject(out Object unityObject))
            {
                throw new System.Exception($"Cannot instantiate {SerializedInterface} because it's has no reference of type UnityEngine.Object");
            }

            Object instantiatedObject = Object.Instantiate(unityObject, position, rotation, parent);

            return GetInterfaceReference<TInterface>(instantiatedObject);
        }

        private static TInterface GetInterfaceReference<TInterface>(Object instantiatedObject) where TInterface : class
        {
            if (instantiatedObject is GameObject gameObject)
                return gameObject.TryGetComponent(out TInterface component) ? component : null;

            return instantiatedObject as TInterface;
        }
    }
}
