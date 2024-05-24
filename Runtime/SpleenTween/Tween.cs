namespace NuiN.SpleenTween 
{
    using System;
    using UnityEngine;
    
    public interface Tween
    {
        /// <summary>
        ///  Gets called every frame to calculate and update the values of the tween
        /// </summary>
        /// <returns>True if valid, False if stopped or complete</returns>
        bool Run();

        Tween OnStart(Action onStart);
        Tween OnUpdate<TU>(Action<TU> onUpdate);
        Tween OnComplete(Action onComplete);
        Tween OnStop(Action onStop);
        Tween SetEase(Ease ease);
        Tween SetEase(AnimationCurve animationCurve);
        Tween SetEase(TweenSettings settings);
        Tween SetLoop(Loop loopType, int cycles = -1);
        Tween SetDelay(float delay, bool startDelay = true);
        Tween SetDestroyOnLoad(bool destroy);
        Tween Stop();
        Tween StopIfNull(GameObject target);
        Tween StopIf(Func<bool> stopCondition, bool invokeComplete = false);

        Tween Pause();
        Tween Play();
        Tween Toggle();

        Tween SetPlaybackSpeed(float targetSpeed);
        Tween SetTimeScaleIndependant(bool option);

        /// <summary>
        /// Tweens the playback speed from the current playback speed
        /// </summary>
        Tween SetPlaybackSpeed(float targetSpeed, float smoothTime);

        /// <summary>
        /// Tweens the playback speed from the specified value
        /// </summary>
        Tween SetPlaybackSpeed(float startSpeed, float targetSpeed, float smoothTime);
        
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
