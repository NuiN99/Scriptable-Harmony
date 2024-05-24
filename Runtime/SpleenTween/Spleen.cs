using UnityEngine.UI;
using System.Collections;

namespace SpleenTween
{
    using System;
    using UnityEngine;
    
    /// <summary>
    /// Call the functions in this to start a tween
    /// </summary>
    public static class Spleen
    {
        public static void AddTween(Tween tween) => SpleenTweenManager.instance.StartTween(tween);
        public static void StopTween(Tween tween) => SpleenTweenManager.instance.StopTween(tween);
        public static void StopAllTweens() => SpleenTweenManager.instance.StopAllTweens();
        public static void StopAllTweens(GameObject target) => SpleenTweenManager.instance.StopAllTweensWithIdentifier(target);

        public static IEnumerator WaitForTween(Tween tween)
        {
            yield return new WaitForSeconds(tween.Duration + tween.Delay);
        }

        static Tween CreateTween<T>(T from, T to, float duration, Action<T> onUpdate)
        {
            Tween tween = new TweenInstance<T>(from, to, duration, onUpdate);
            AddTween(tween);
            return tween;
        }

        static Tween CreateTargetTween<T,K>(K target, GameObject identifier, T from, T to, float duration, Action<T> onUpdate)
        {
            Tween tween = new TweenInstance<T>(from, to, duration, onUpdate, identifier, () => identifier == null || target == null || target.Equals(null));
            AddTween(tween);
            return tween;
        }

        static Tween CreateRelativeTargetTween<T, K>(K target, GameObject identifier, T increment, float duration, Func<T> currentVal, Action<T, T> onUpdate)
        {
            T current = currentVal.Invoke();
            T from = current;
            T to = SpleenExt.AddGenerics(current, increment);

            Tween tween = new TweenInstance<T>(from, to, duration, (val) =>
            {
                onUpdate.Invoke(val, current);
                current = val;
            }, identifier, () => target == null || target.Equals(null) || identifier == null);

            tween.OnStart(() =>
            {
                if (Looping.IsLoopWeird(tween.LoopType)) return;

                current = currentVal.Invoke();
                from = current;
                to = SpleenExt.AddGenerics(current, increment);
                tween.From = from;
                tween.To = to;
            });

            AddTween(tween);
            return tween;
        }

        #region Create Tweens
        public static Tween Value(float from, float to, float duration, Action<float> onUpdate) => 
            CreateTween(from, to, duration, onUpdate);
        public static Tween Value0To1(float duration, Action<float> onUpdate) => 
            CreateTween(0, 1, duration, onUpdate);
        public static Tween Value1To0(float duration, Action<float> onUpdate) => 
            CreateTween(1, 0, duration, onUpdate);
        public static Tween Value3(Vector3 from, Vector3 to, float duration, Action<Vector3> onUpdate) => 
            CreateTween(from, to, duration, onUpdate);

        public static Tween DoAfter(float seconds, Action onComplete) =>
            CreateTween(0f, seconds, seconds, null).OnComplete(onComplete);
        public static Tween DoFor(float seconds, Action onUpdate) =>
            CreateTween(0f, seconds, seconds, (_) => onUpdate?.Invoke());
        public static Tween DoWhen(Func<bool> condition, Action doWhen, float timeOutAfterSeconds = float.MaxValue) =>
            CreateTween(0f, 1f, timeOutAfterSeconds, null).StopIf(condition, true).OnComplete(doWhen);
        public static Tween DoUntil(Func<bool> condition, Action doUntil, float timeOutAfterSeconds = float.MaxValue) =>
            CreateTween(0f, 1f, timeOutAfterSeconds, (_) => doUntil?.Invoke()).StopIf(condition);
        
        
        public static Tween Pos(Transform target, Vector3 from, Vector3 to, float duration) => 
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.position = val);
        public static Tween Pos(Transform target, Vector3 to, float duration) => 
            CreateTargetTween(target, target.gameObject, target.transform.position, to, duration, val => target.position = val);
        
        public static Tween PosAxis(Transform target, Axis axis, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => SpleenExt.SetPosAxis(axis, target, val));
        public static Tween PosAxis(Transform target, Axis axis, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.position), to, duration, val => SpleenExt.SetPosAxis(axis, target, val));
        
        public static Tween AddPos(Transform target, Vector3 increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, () => target.position, (val, from) => target.position += val - from);
        public static Tween AddPosAxis(Transform target, Axis axis, float increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, 
                () => SpleenExt.GetAxis(axis, target.position), (val, from) => SpleenExt.AddPosAxis(axis, target, val - from));
        
        
        public static Tween LocalPos(Transform target, Vector3 from, Vector3 to, float duration) => 
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.localPosition = val);
        public static Tween LocalPos(Transform target, Vector3 to, float duration) => 
            CreateTargetTween(target, target.gameObject, target.transform.localPosition, to, duration, val => target.localPosition = val);
        
        public static Tween LocalPosAxis(Transform target, Axis axis, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, 
                val => SpleenExt.SetLocalPosAxis(axis, target, val));
        public static Tween LocalPosAxis(Transform target, Axis axis, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.localPosition), to, duration, val => SpleenExt.SetLocalPosAxis(axis, target, val));
        
        public static Tween AddLocalPos(Transform target, Vector3 increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, () => target.localPosition, (val, from) => target.localPosition += val - from);
        public static Tween AddLocalPosAxis(Transform target, Axis axis, float increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, 
                () => SpleenExt.GetAxis(axis, target.localPosition), (val, from) => SpleenExt.AddLocalPosAxis(axis, target, val - from));
        
        
        public static Tween RBPos(Rigidbody target, Vector3 from, Vector3 to, float duration) => 
            CreateTargetTween(target, target.gameObject, from, to, duration, target.MovePosition);
        public static Tween RBPos(Rigidbody target, Vector3 to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.position, to, duration, target.MovePosition);
        
        public static Tween RBPosAxis(Rigidbody target, Axis axis, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => SpleenExt.SetRBPosAxis(axis, target, val));
        public static Tween RBPosAxis(Rigidbody target, Axis axis, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.position), to, duration, val => SpleenExt.SetRBPosAxis(axis, target, val));
        
        
        public static Tween RB2DPos(Rigidbody2D target, Vector2 from, Vector2 to, float duration) => 
            CreateTargetTween(target, target.gameObject, from, to, duration, target.MovePosition);
        public static Tween RB2DPos(Rigidbody2D target, Vector2 to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.position, to, duration, target.MovePosition);
        
        public static Tween RB2DPosAxis(Rigidbody2D target, Axis axis, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => SpleenExt.SetRB2DPosAxis(axis, target, val));
        public static Tween RB2DPosAxis(Rigidbody2D target, Axis axis, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.position), to, duration, val => SpleenExt.SetRB2DPosAxis(axis, target, val));

        
        public static Tween Scale(Transform target, Vector3 from, Vector3 to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.localScale = val);
        public static Tween Scale(Transform target, Vector3 to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.localScale, to, duration, val => target.localScale = val);
        
        public static Tween ScaleAxis(Transform target, Axis axis, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => SpleenExt.SetScaleAxis(axis, target, val));
        public static Tween ScaleAxis(Transform target, Axis axis, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.localScale), to, duration, val => SpleenExt.SetScaleAxis(axis, target, val));

        public static Tween AddScale(Transform target, Vector3 increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, () => target.localScale, (val, from) => 
            target.localScale += val - from);
        public static Tween AddScaleAxis(Transform target, Axis axis, float increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, 
                () => SpleenExt.GetAxis(axis, target.localScale), (val, from) => SpleenExt.AddScaleAxis(axis, target, val - from));

        
        public static Tween Euler(Transform target, Vector3 from, Vector3 to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.eulerAngles = val);
        public static Tween Euler(Transform target, Vector3 to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.eulerAngles, to, duration, val => target.eulerAngles = val);
        
        public static Tween EulerAxis(Transform target, Axis axis, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => SpleenExt.SetEulerAxis(axis, target, val));
        public static Tween EulerAxis(Transform target, Axis axis, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.eulerAngles), to, duration, val => SpleenExt.SetEulerAxis(axis, target, val));

        public static Tween AddEuler(Transform target, Vector3 increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, () => target.eulerAngles, (val, from) => 
            target.eulerAngles += val - from);
        public static Tween AddRotAxis(Transform target, Axis axis, float increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, 
                () => SpleenExt.GetAxis(axis, target.eulerAngles), (val, from) => SpleenExt.AddEulerAxis(axis, target, val - from));
        
        public static Tween Rot(Transform target, Quaternion from, Quaternion to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.rotation = val);
        public static Tween Rot(Transform target, Quaternion to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.rotation, to, duration, val => target.rotation = val);
        
        public static Tween RotAxis(Transform target, Axis axis, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => SpleenExt.SetRotAxis(axis, target, val));
        public static Tween RotAxis(Transform target, Axis axis, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetRotAxis(axis, target.rotation), to, duration, val => SpleenExt.SetRotAxis(axis, target, val));

        public static Tween Vol(AudioSource target, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.volume = val);
        public static Tween Vol(AudioSource target, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.volume, to, duration, val => target.volume = val);
        public static Tween AddVol(AudioSource target, float increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, 
                () => target.volume, (val, from) => target.volume += val - from);

        
        public static Tween SRColor(SpriteRenderer target, Color from, Color to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.color = val);
        public static Tween SRColor(SpriteRenderer target, Color to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.color, to, duration, val => target.color = val);
        public static Tween AddSRColor(SpriteRenderer target, Color increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, () => target.color, (val, from) =>
            target.color += val - from);
        
        public static Tween SRAlpha(SpriteRenderer target, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, 
                val => target.color = new Color(target.color.r, target.color.g, target.color.b, val));
        public static Tween SRAlpha(SpriteRenderer target, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.color.a, to, duration, 
                val => target.color = new Color(target.color.r, target.color.g, target.color.b, val));
        
        
        public static Tween ImageFill(Image target, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, 
                val => target.fillAmount = val);
        public static Tween ImageFill(Image target, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.fillAmount, to, duration, 
                val => target.fillAmount = val);
        
        
        public static Tween ImageAlpha(Image target, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, 
                val => target.color = new Color(target.color.r, target.color.g, target.color.b, val));
        public static Tween ImageAlpha(Image target, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.color.a, to, duration, 
                val => target.color = new Color(target.color.r, target.color.g, target.color.b, val));
        
        public static Tween RawImageAlpha(RawImage target, float from, float to, float duration) =>
                    CreateTargetTween(target, target.gameObject, from, to, duration, 
                        val => target.color = new Color(target.color.r, target.color.g, target.color.b, val));
        public static Tween RawImageAlpha(RawImage target, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.color.a, to, duration, 
                val => target.color = new Color(target.color.r, target.color.g, target.color.b, val));


        public static Tween CanvasGroupAlpha(CanvasGroup target, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.alpha = val);
        public static Tween CanvasGroupAlpha(CanvasGroup target, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.alpha, to, duration, val => target.alpha = val);


        public static Tween CamFOV(Camera target, float from, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, val => target.fieldOfView = val);
        public static Tween CamFOV(Camera target, float to, float duration) =>
            CreateTargetTween(target, target.gameObject, target.fieldOfView, to, duration, val => target.fieldOfView = val);
        public static Tween AddCamFOV(Camera target, float increment, float duration) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, 
                () => target.fieldOfView, (val, from) => target.fieldOfView += val - from);


        public static Tween TimeScale(float from, float to, float duration)
        {
            Tween tween = CreateTween(from, to, duration, val =>
            {
                if(float.IsNaN(val)) return;
                
                Time.timeScale = val;
                Time.fixedDeltaTime = SpleenTweenManager.fixedDeltaTime * val;
            });
            
            tween.OnUpdate<float>(val => tween.SetPlaybackSpeed(1 / val));
            return tween;
        }
        public static Tween TimeScale(float to, float duration)
        {
            Tween tween = CreateTween(Time.timeScale, to, duration, val =>
            {
                if(float.IsNaN(val)) return;

                Time.timeScale = val;
                Time.fixedDeltaTime = SpleenTweenManager.fixedDeltaTime * val;
            });
            
            tween.OnUpdate<float>(val => tween.SetPlaybackSpeed(1 / val));
            return tween;
        }


        #endregion
    }
}
