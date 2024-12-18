using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.NExtensions
{
    public static class GeneralUtils
    {
        public static T GetClosest<T>(Vector3 point, IEnumerable<T> objects, Func<T, bool> condition = null) where T : MonoBehaviour
        {
            T closestObj = null;
            float closestDist = float.MaxValue;
            foreach (T obj in objects)
            {
                if (!obj) continue;
                
                if (condition != null)
                {
                    bool result = condition.Invoke(obj);
                    if(!result) continue;
                }
                
                Vector3 offset = obj.transform.position - point;
                float sqrLength = offset.sqrMagnitude;
                
                if (sqrLength >= closestDist) continue;

                closestDist = sqrLength;
                closestObj = obj;
            }

            return closestObj;
        }
        
        public static Transform GetClosest(Vector3 point, IEnumerable<Transform> objects, Func<Transform, bool> condition = null)
        {
            Transform closestObj = null;
            float closestDist = float.MaxValue;
            foreach (var obj in objects)
            {
                if (!obj) continue;
                
                if (condition != null)
                {
                    bool result = condition.Invoke(obj);
                    if(!result) continue;
                }
                
                Vector3 offset = obj.transform.position - point;
                float sqrLength = offset.sqrMagnitude;
                
                if (sqrLength >= closestDist) continue;

                closestDist = sqrLength;
                closestObj = obj;
            }

            return closestObj;
        }

        public static GameObject GetClosest(Vector3 point, IEnumerable<GameObject> objects, Func<GameObject, bool> condition = null)
        {
            GameObject closestObj = null;
            float closestDist = float.MaxValue;
            foreach (var obj in objects)
            {
                if (!obj) continue;
                
                if (condition != null)
                {
                    bool result = condition.Invoke(obj);
                    if(!result) continue;
                }
                
                Vector3 offset = obj.transform.position - point;
                float sqrLength = offset.sqrMagnitude;
                
                if (sqrLength >= closestDist) continue;

                closestDist = sqrLength;
                closestObj = obj;
            }

            return closestObj;
        }

        public static Collider GetClosest(Vector3 point, IEnumerable<Collider> objects, Func<Collider, bool> condition = null)
        {
            Collider closestObj = null;
            float closestDist = float.MaxValue;
            foreach (var obj in objects)
            {
                if (!obj) continue;
                
                if (condition != null)
                {
                    bool result = condition.Invoke(obj);
                    if(!result) continue;
                }
                
                Vector3 offset = obj.transform.position - point;
                float sqrLength = offset.sqrMagnitude;
                
                if (sqrLength >= closestDist) continue;

                closestDist = sqrLength;
                closestObj = obj;
            }

            return closestObj;
        }

        public static Rigidbody GetClosest(Vector3 point, IEnumerable<Rigidbody> objects, Func<Rigidbody, bool> condition = null)
        {
            Rigidbody closestObj = null;
            float closestDist = float.MaxValue;
            foreach (var obj in objects)
            {
                if (!obj) continue;
                
                if (condition != null)
                {
                    bool result = condition.Invoke(obj);
                    if(!result) continue;
                }
                
                Vector3 offset = obj.transform.position - point;
                float sqrLength = offset.sqrMagnitude;
                
                if (sqrLength >= closestDist) continue;

                closestDist = sqrLength;
                closestObj = obj;
            }

            return closestObj;
        }

        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public static bool GetPrefsBool(bool startValue, string key)
        {
            int currentValue = startValue ? 1 : 0;
            int value = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : currentValue;
            return value == 1;
        }

        public static void SetPrefsBool(bool value, string key)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static LayerMask StringToLayerMask(string layerName)
        {
            return 1 << LayerMask.NameToLayer(layerName);
        }
    }
}
