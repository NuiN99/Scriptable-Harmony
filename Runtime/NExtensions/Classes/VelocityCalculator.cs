using UnityEngine;

namespace NuiN.NExtensions
{
    public class VelocityCalculator : MonoBehaviour
    {
        public enum SmoothingMode
        {
            None,
            Exponential,
            RollingAverage
        }

        public enum UpdateMode
        {
            LateUpdate,
            Update,
        }

        [ShowInInspector]
        public Vector3 Velocity { get; private set; }

        [ShowInInspector]
        public Vector3 RawVelocity { get; private set; }

        [ShowInInspector]
        public Vector3 AngularVelocity { get; private set; } // degrees/sec

        [ShowInInspector]
        public Vector3 RawAngularVelocity { get; private set; }

        [SerializeField] UpdateMode updateMode;

        [Header("Smoothing")]
        [SerializeField] SmoothingMode smoothingMode = SmoothingMode.None;

        [SerializeField, Range(0f, 1f), ShowIf(nameof(smoothingMode), (int)SmoothingMode.Exponential)]
        float exponentialFactor = 0.2f;

        [SerializeField, Min(1), ShowIf(nameof(smoothingMode), (int)SmoothingMode.RollingAverage)]
        int averageFrameCount = 5;

        Vector3 _position;
        Vector3 _previousPosition;

        Quaternion _rotation;
        Quaternion _previousRotation;

        bool _hasPreviousSample;

        Vector3[] _velocityBuffer;
        Vector3[] _angularVelocityBuffer;
        int _bufferIndex;

        void Awake()
        {
            InitializeAverageBuffer();
        }

        void Update()
        {
            if (updateMode == UpdateMode.Update)
                Calculate();
        }

        void LateUpdate()
        {
            if (updateMode == UpdateMode.LateUpdate)
                Calculate();
        }

        void InitializeAverageBuffer()
        {
            if (smoothingMode != SmoothingMode.RollingAverage)
                return;

            averageFrameCount = Mathf.Max(1, averageFrameCount);

            _velocityBuffer = new Vector3[averageFrameCount];
            _angularVelocityBuffer = new Vector3[averageFrameCount];
            _bufferIndex = 0;
        }

        void Calculate()
        {
            if (!_hasPreviousSample)
            {
                _position = transform.position;
                _previousPosition = _position;

                _rotation = transform.rotation;
                _previousRotation = _rotation;

                Velocity = Vector3.zero;
                RawVelocity = Vector3.zero;
                AngularVelocity = Vector3.zero;
                RawAngularVelocity = Vector3.zero;

                _hasPreviousSample = true;
                return;
            }

            float dt = Time.deltaTime;
            if (dt <= 0f)
                return;

            _previousPosition = _position;
            _position = transform.position;
            RawVelocity = (_position - _previousPosition) / dt;

            _previousRotation = _rotation;
            _rotation = transform.rotation;

            Quaternion delta = _rotation * Quaternion.Inverse(_previousRotation);
            delta.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180f)
                angle -= 360f;

            RawAngularVelocity = axis * (angle / dt);

            switch (smoothingMode)
            {
                case SmoothingMode.None:
                    Velocity = RawVelocity;
                    AngularVelocity = RawAngularVelocity;
                    break;

                case SmoothingMode.Exponential:
                    Velocity = Vector3.Lerp(Velocity, RawVelocity, exponentialFactor);
                    AngularVelocity = Vector3.Lerp(AngularVelocity, RawAngularVelocity, exponentialFactor);
                    break;

                case SmoothingMode.RollingAverage:
                    ApplyRollingAverage(RawVelocity, RawAngularVelocity);
                    break;
            }
        }

        void ApplyRollingAverage(Vector3 frameVelocity, Vector3 frameAngularVelocity)
        {
            if (_velocityBuffer == null ||
                _angularVelocityBuffer == null ||
                _velocityBuffer.Length != averageFrameCount)
            {
                InitializeAverageBuffer();
            }

            _velocityBuffer[_bufferIndex] = frameVelocity;
            _angularVelocityBuffer[_bufferIndex] = frameAngularVelocity;

            _bufferIndex = (_bufferIndex + 1) % _velocityBuffer.Length;

            Vector3 velocitySum = Vector3.zero;
            Vector3 angularSum = Vector3.zero;

            for (int i = 0; i < _velocityBuffer.Length; i++)
            {
                velocitySum += _velocityBuffer[i];
                angularSum += _angularVelocityBuffer[i];
            }

            Velocity = velocitySum / _velocityBuffer.Length;
            AngularVelocity = angularSum / _angularVelocityBuffer.Length;
        }
    }
}
