using System;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralUtils
{
    public static T GetClosest<T>(Vector3 point, List<T> objects, Func<bool, T> condition = null) where T : MonoBehaviour
    {
        T closestObj = null;
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
    
    public static Transform GetClosest(Vector3 point, List<Transform> objects, Func<bool, Transform> condition = null)
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
}
