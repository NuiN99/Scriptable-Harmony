using UnityEngine.UIElements.Experimental;

namespace SpleenTween
{
    using System;
    using UnityEngine;
    
    public enum Axis { x, y, z };

    /// <summary>
    /// Various helper functions for improved code readability
    /// </summary>
    public static class SpleenExt
    {
        public static float GetEase(float lerp, Ease easing)
        {
            return easing switch
            {
                Ease.Linear => lerp,
                
                Ease.InSine => Easing.InSine(lerp),
                Ease.OutSine => Easing.OutSine(lerp),
                Ease.InOutSine => Easing.InOutSine(lerp),

                Ease.InQuad => Easing.InQuad(lerp),
                Ease.OutQuad => Easing.OutQuad(lerp),
                Ease.InOutQuad => Easing.InOutQuad(lerp),

                Ease.InCubic => Easing.InCubic(lerp),
                Ease.OutCubic => Easing.OutCubic(lerp),
                Ease.InOutCubic => Easing.InOutCubic(lerp),
                
                Ease.InCirc => Easing.InCirc(lerp),
                Ease.OutCirc => Easing.OutCirc(lerp),
                Ease.InOutCirc => Easing.InOutCirc(lerp),

                Ease.InBack => Easing.InBack(lerp),
                Ease.OutBack => Easing.OutBack(lerp),
                Ease.InOutBack => Easing.InOutBack(lerp),

                Ease.InElastic => Easing.InElastic(lerp),
                Ease.OutElastic => Easing.OutElastic(lerp),
                Ease.InOutElastic => Easing.InOutElastic(lerp),

                Ease.InBounce => Easing.InBounce(lerp),
                Ease.OutBounce => Easing.OutBounce(lerp),
                Ease.InOutBounce => Easing.InOutBounce(lerp),
                
                Ease.InQuart => lerp * lerp * lerp * lerp,
                Ease.OutQuart => 1 - Mathf.Pow(1 - lerp, 4),
                Ease.InOutQuart => lerp < 0.5 ? 8 * lerp * lerp * lerp * lerp : 1 - Mathf.Pow(-2 * lerp + 2, 4) / 2,

                Ease.InQuint => lerp * lerp * lerp * lerp * lerp,
                Ease.OutQuint => 1 - Mathf.Pow(1 - lerp, 5),
                Ease.InOutQuint => lerp < 0.5 ? 16 * lerp * lerp * lerp * lerp * lerp : 1 - Mathf.Pow(-2 * lerp + 2, 5) / 2,

                Ease.InExpo => lerp == 0 ? 0 : Mathf.Pow(2, 10 * lerp - 10),
                Ease.OutExpo => Math.Abs(lerp - 1) < Mathf.Epsilon ? 1 : 1 - Mathf.Pow(2, -10 * lerp),
                Ease.InOutExpo => lerp == 0 ? 0 : Math.Abs(lerp - 1) < Mathf.Epsilon ? 1 : lerp < 0.5 ? Mathf.Pow(2, 20 * lerp - 10) / 2 : (2 - Mathf.Pow(2, -20 * lerp + 10)) / 2,

                _ => lerp
            };
        }
        
        static void SetAxis(Axis axis, Vector3 inVal, float targetVal, Action<Vector3> setAxis)
        {
            Vector3 newVal = inVal;
            switch (axis)
            {
                case Axis.x: newVal.x = targetVal; break;
                case Axis.y: newVal.y = targetVal; break;
                case Axis.z: newVal.z = targetVal; break;
            }
            setAxis?.Invoke(newVal);
        }
        static void AddAxis(Axis axis, float increment, Action<Vector3> addAxis)
        {
            Vector3 newVal = Vector3.zero;
            switch (axis)
            {
                case Axis.x: newVal.x = increment; break;
                case Axis.y: newVal.y = increment; break;
                case Axis.z: newVal.z = increment; break;
            }
            addAxis?.Invoke(newVal);
        }

        public static float GetAxis(Axis axis, Vector3 inVal)
        {
            return axis switch
            {
                Axis.x => inVal.x,
                Axis.y => inVal.y,
                Axis.z => inVal.z,
                _ => throw new MissingMemberException("Axis somehow does not exist")
            };
        }
        
        static void SetRotationAxis(Axis axis, Quaternion inVal, float targetVal, Action<Quaternion> setAxis)
        {
            Quaternion newVal = inVal;
            switch (axis)
            {
                case Axis.x: newVal.x = targetVal; break;
                case Axis.y: newVal.y = targetVal; break;
                case Axis.z: newVal.z = targetVal; break;
            }
            setAxis?.Invoke(newVal);
        }
        static void AddRotationAxis(Axis axis, float increment, Action<Quaternion> addAxis)
        {
            Quaternion newVal = Quaternion.identity;
            switch (axis)
            {
                case Axis.x: newVal.x = increment; break;
                case Axis.y: newVal.y = increment; break;
                case Axis.z: newVal.z = increment; break;
            }
            addAxis?.Invoke(newVal);
        }
        public static float GetRotAxis(Axis axis, Quaternion inVal)
        {
            return axis switch
            {
                Axis.x => inVal.x,
                Axis.y => inVal.y,
                Axis.z => inVal.z,
                _ => throw new MissingMemberException("Axis somehow does not exist")
            };
        }

        public static void SetPosAxis(Axis axis, Transform target, float targetVal) => SetAxis(axis, target.position, targetVal,
            (val) => target.transform.position = val);
        public static void SetLocalPosAxis(Axis axis, Transform target, float targetVal) => SetAxis(axis, target.localPosition, targetVal,
            (val) => target.transform.localPosition = val);
        public static void SetScaleAxis(Axis axis, Transform target, float targetVal) => SetAxis(axis, target.localScale, targetVal,
            (val) => target.transform.localScale = val);
        public static void SetEulerAxis(Axis axis, Transform target, float targetVal) => SetAxis(axis, target.eulerAngles, targetVal,
            (val) => target.transform.eulerAngles = val);
        public static void SetRotAxis(Axis axis, Transform target, float targetVal) => SetRotationAxis(axis, target.rotation, targetVal,
            (val) => target.rotation = val);

        public static void SetRBPosAxis(Axis axis, Rigidbody target, float targetVal) => SetAxis(axis, target.position, targetVal, target.MovePosition);
        public static void SetRB2DPosAxis(Axis axis, Rigidbody2D target, float targetVal) => SetAxis(axis, target.position, targetVal, pos => target.MovePosition(pos));

        public static void AddPosAxis(Axis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.position += val);
        public static void AddLocalPosAxis(Axis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.localPosition += val);
        public static void AddScaleAxis(Axis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.localScale += val);
        public static void AddEulerAxis(Axis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.eulerAngles += val);

        /// <summary>
        /// Checks the type of passed in generic and if possible performs A + B
        /// </summary>
        public static T AddGenerics<T>(T a, T b)
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)((float)(object)a + (float)(object)b);
            }

            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)((Vector3)(object)a + (Vector3)(object)b);
            }
            
            if (typeof(T) == typeof(Vector2))
            {
                return (T)(object)((Vector2)(object)a + (Vector2)(object)b);
            }

            if (typeof(T) == typeof(Color))
            {
                return (T)(object)((Color)(object)a + (Color)(object)b);
            }

            throw new System.NotSupportedException("Type not supported for generic addition");
        }

        /// <summary>
        /// Checks the type of passed in generic and if possible performs A - B
        /// </summary>
        public static T SubtractGenerics<T>(T a, T b)
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)((float)(object)a - (float)(object)b);
            }

            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)((Vector3)(object)a - (Vector3)(object)b);
            }
            
            if (typeof(T) == typeof(Vector2))
            {
                return (T)(object)((Vector2)(object)a - (Vector2)(object)b);
            }

            if (typeof(T) == typeof(Color))
            {
                return (T)(object)((Color)(object)a - (Color)(object)b);
            }

            throw new System.NotSupportedException("Type not supported for generic subtraction");
        }

        /// <summary>
        /// Checks the type of passed in generic and if possible lerps unclamped between A and B
        /// </summary>
        public static T LerpUnclampedGeneric<T>(T from, T to, float lerpProgress)
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)Mathf.LerpUnclamped((float)(object)from, (float)(object)to, lerpProgress);
            }

            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)Vector3.LerpUnclamped((Vector3)(object)from, (Vector3)(object)to, lerpProgress);
            }
            
            if (typeof(T) == typeof(Vector2))
            {
                return (T)(object)Vector2.LerpUnclamped((Vector2)(object)from, (Vector2)(object)to, lerpProgress);
            }
            
            if (typeof(T) == typeof(Quaternion))
            {
                return (T)(object)Quaternion.SlerpUnclamped((Quaternion)(object)from, (Quaternion)(object)to, lerpProgress);
            }

            if (typeof(T) == typeof(Color))
            {
                return (T)(object)Color.LerpUnclamped((Color)(object)from, (Color)(object)to, lerpProgress);
            }

            throw new NotSupportedException("Type not supported for generic lerping");
        }
    }
}
