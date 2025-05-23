namespace NuiN.SpleenTween
{
    using System;
    using UnityEngine;
    
    public class TweenInstance<T> : ITween
    {
        T _currentValue;
        T _from;
        T _to;
        
        Action<T> _onUpdate;
        Func<bool> _nullCheck;
        Func<bool> _stopCondition;
        
        Action _onComplete;
        Action _onStart;
        Action _onStop;

        AnimationCurve _customEase;
        
        bool _started;
        bool _invokeCompleteAfterStopped;
        int _cycles;
        bool _completed;
        bool _useCustomEase;

        public GameObject Identifier { get; private set; }

        public object CurrentValue => _currentValue;
        public object From { get => _from; set { } }
        public object To { get => _to; set { } }

        public float CurrentTime { get; private set; }
        public float Duration { get; private set; }

        public float PlaybackSpeed { get; private set; } = 1f;

        public float Delay { get; private set; }

        public float LerpProgress => Mathf.Clamp01(CurrentTime / Duration);

        public float EasedLerpProgress
        {
            get
            {
                float easeVal = EaseType != Ease.Custom ? SpleenExt.GetEase(LerpProgress, EaseType) : _customEase.Evaluate(LerpProgress);

                switch (LoopType)
                {
                    case Loop.Rewind:
                        float backwardsLerp = 1 - LerpProgress;
                        float lerpBasedOnDirection = Direction == 0 ? backwardsLerp : LerpProgress;
                        return EaseType != Ease.Custom ? SpleenExt.GetEase(lerpBasedOnDirection, EaseType) : _customEase.Evaluate(lerpBasedOnDirection);
                    case Loop.Yoyo:
                        float backwardsEase = 1 - easeVal;
                        return Direction == 0 ? backwardsEase : easeVal;
                    default:
                        return easeVal;
                }
            }
            private set => _ = value;
        }

        public bool Active => CurrentTime < Duration;

        public Ease EaseType { get; private set; }
        public Loop LoopType { get; private set; }

        /// <summary>
        /// How many times the tween will repeat
        /// </summary>
        public int Cycles => Mathf.Clamp(_cycles - CycleCount, -1, int.MaxValue);
        public int CycleCount { get; private set; }

        // will always be forward(1) if loop type is not rewind or yoyo
        public int Direction => (!Looping.IsLoopWeird(LoopType) || (CycleCount % 2) == 0) ? 1 : 0; 
        public bool Paused { get; private set; }
        public bool DontDestroyOnLoad { get; private set; }
        public bool  TimeScaleIndependant { get; private set; }

        public TweenInstance(T from, T to, float duration, Action<T> onUpdate, GameObject identifier = null, Func<bool> nullCheck = null)
        {
            _from = from;
            _to = to;
            Duration = duration;
            _onUpdate = onUpdate;
            Identifier = identifier;
            _nullCheck = nullCheck;
        }

        bool ITween.Run()
        {
            if (Paused || NullTarget() || StopConditionMet()) return false;

            if (TimeScaleIndependant) SetPlaybackSpeed(Mathf.Min(float.MaxValue, (1 / Time.timeScale)));
            
            CurrentTime += Time.deltaTime * PlaybackSpeed;

            switch (CurrentTime)
            {
                case < 0: return true; // wait for delay
                case >= 0 when !_started: InvokeOnStart(); break;
            }

            if (!Active)
            {
                EasedLerpProgress = Direction; // set final value for precision
                RestartLoop();
                UpdateValue();
                InvokeOnComplete();
                return !(LoopType == Loop.None || Cycles is not (-1 or > 0));
            }

            UpdateValue();
            return true;
        }

        void UpdateValue()
        {
            _currentValue = SpleenExt.LerpUnclampedGeneric(_from, _to, EasedLerpProgress);
            _onUpdate?.Invoke(_currentValue);
        }

        void InvokeOnComplete()
        {
            _started = false;
            _onComplete?.Invoke();
        } 
        void InvokeOnStart()
        {
            _started = true;
            _onStart?.Invoke();
        }
        void InvokeOnStop()
        {
            _onStop?.Invoke();
        }

        void RestartLoop()
        {
            if (LoopType == Loop.None || Cycles is not (-1 or > 0)) return;

            Looping.RestartLoopTypes(LoopType, ref _from, ref _to);
            CurrentTime = 0;
            DelayCycle(Delay);
            CycleCount++;
        }

        void DelayCycle(float delay)
        {
            CurrentTime -= delay;
        }

        /// <summary>
        /// Checks if the target is null to avoid Null Ref Exceptions. The nullCheck callback is set when creating a tween if the tween has a target, eg. a transform.
        /// </summary>
        bool NullTarget()
        {
            bool isNull = _nullCheck != null && _nullCheck.Invoke();
            if (isNull) InvokeOnStop();
            return isNull;
        }

        bool StopConditionMet()
        {
            bool conditionMet = _stopCondition != null && _stopCondition.Invoke();
            switch (conditionMet)
            {
                case true when _invokeCompleteAfterStopped && !_completed:
                    _completed = true;
                    InvokeOnComplete();
                    break;
            }
            
            if(conditionMet) InvokeOnStop();
            
            return conditionMet;
        }

        #region Method Chains

        ITween ITween.OnComplete(Action onComplete)
        {
            _onComplete += onComplete;
            return this;
        }
        ITween ITween.OnStart(Action onStart)
        {
            _onStart += onStart;
            return this;
        }
        public ITween OnUpdate<TU>(Action<TU> onUpdate)
        {
            if (typeof(T) != typeof(TU))
            {
                Debug.LogError("Incompatible Types in OnUpdate callback: OnUpdate callback will not run");
                return this;
            }

            _onUpdate += (value) => onUpdate((TU)(object)value);
            return this;
        }

        ITween ITween.SetEase(Ease ease)
        {
            EaseType = ease;
            return this;
        }

        ITween ITween.SetEase(AnimationCurve animationCurve)
        {
            EaseType = Ease.Custom;
            _customEase = animationCurve;
            return this;
        }

        ITween ITween.SetEase(TweenSettings settings)
        {
            EaseType = settings.Ease;
            if (settings.Ease == Ease.Custom)  _customEase = settings.CustomEase;

            return this;
        }

        ITween ITween.SetLoop(Loop loopType, int cycles)
        {
            if (cycles == 0)
                SpleenTween.StopTween(this);

            LoopType = loopType;

            _cycles = cycles - 1;

            return this;
        }

        ITween ITween.SetDelay(float delay, bool startDelay)
        {
            Delay = delay;
            if (startDelay) DelayCycle(delay);
            return this;
        }

        ITween ITween.SetDestroyOnLoad(bool destroy)
        {
            DontDestroyOnLoad = destroy;
            return this;
        }

        ITween ITween.Stop()
        {
            SpleenTween.StopTween(this);
            return this;
        }
        
        ITween ITween.StopIfNull(GameObject target)
        {
            _nullCheck += () => target == null;
            return this;
        }

        ITween ITween.StopIf(Func<bool> stopCondition, bool invokeComplete)
        {
            _invokeCompleteAfterStopped = invokeComplete;
            _stopCondition += stopCondition;
            return this;
        }

        ITween ITween.OnStop(Action onStop)
        {
            _onStop += onStop;
            return this;
        }

        ITween ITween.Pause()
        {
            Paused = true;
            return this;
        }
        ITween ITween.Play()
        {
            Paused = false;
            return this;
        }
        ITween ITween.Toggle()
        {
            Paused = !Paused;
            return this;
        }

        public ITween SetPlaybackSpeed(float targetSpeed)
        {
            PlaybackSpeed = targetSpeed;
            return this;
        }
        
        ITween ITween.SetPlaybackSpeed(float targetSpeed, float smoothTime)
        {
            SpleenTween.Value(PlaybackSpeed, targetSpeed, smoothTime, (val) => 
            {
                if(!Paused) PlaybackSpeed = val;
            });
            return this;
        }
        ITween ITween.SetPlaybackSpeed(float startSpeed, float targetSpeed, float smoothTime)
        {
            SpleenTween.Value(startSpeed, targetSpeed, smoothTime, (val) =>
            {
                if (!Paused) PlaybackSpeed = val;
            });
            return this;
        }
        
        ITween ITween.SetTimeScaleIndependant(bool option)
        {
            TimeScaleIndependant = option;
            return this;
        }

        #endregion
    }
}

