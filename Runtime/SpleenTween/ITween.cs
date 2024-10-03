namespace NuiN.SpleenTween 
{
    using System;
    using UnityEngine;
    
    public interface ITween
    {
        /// <summary>
        ///  Gets called every frame to calculate and update the values of the tween
        /// </summary>
        /// <returns>True if valid, False if stopped or complete</returns>
        bool Run();

        ITween OnStart(Action onStart);
        ITween OnUpdate<TU>(Action<TU> onUpdate);
        ITween OnComplete(Action onComplete);
        ITween OnStop(Action onStop);
        ITween SetEase(Ease ease);
        ITween SetEase(AnimationCurve animationCurve);
        ITween SetEase(TweenSettings settings);
        ITween SetLoop(Loop loopType, int cycles = -1);
        ITween SetDelay(float delay, bool startDelay = true);
        ITween SetDestroyOnLoad(bool destroy);
        ITween Stop();
        ITween StopIfNull(GameObject target);
        ITween StopIf(Func<bool> stopCondition, bool invokeComplete = false);

        ITween Pause();
        ITween Play();
        ITween Toggle();

        ITween SetPlaybackSpeed(float targetSpeed);
        ITween SetTimeScaleIndependant(bool option);

        /// <summary>
        /// Tweens the playback speed from the current playback speed
        /// </summary>
        ITween SetPlaybackSpeed(float targetSpeed, float smoothTime);

        /// <summary>
        /// Tweens the playback speed from the specified value
        /// </summary>
        ITween SetPlaybackSpeed(float startSpeed, float targetSpeed, float smoothTime);
        
        GameObject Identifier { get; }
        object CurrentValue { get; }
        object From { get; set; }
        object To { get; set; }
        float PlaybackSpeed { get; }
        float CurrentTime { get; } 
        float Duration { get; }
        float Delay { get; }
        float LerpProgress { get; }
        float EasedLerpProgress { get; }
        bool Active { get; }
        Ease EaseType { get; }
        Loop LoopType { get; }
        int Cycles { get; }
        int CycleCount { get; }
        int Direction { get; }
        bool Paused { get; }
        bool DontDestroyOnLoad { get; }
        bool TimeScaleIndependant { get; }
    }
}
