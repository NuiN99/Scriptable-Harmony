using UnityEngine;

namespace NuiN.NExtensions
{
    public class VelocityCalculator : MonoBehaviour
    {
        [ShowInInspector] public Vector3 Velocity { get; private set; }

        [SerializeField] bool lateUpdate;

        Vector3 _position;
        Vector3 _previousPosition;

        void Update()
        {
            if (!lateUpdate) Calculate();
        }

        void LateUpdate()
        {
            if (lateUpdate) Calculate();
        }

        void Calculate()
        {
            _previousPosition = _position;
            _position = transform.position;
            Velocity = (_position - _previousPosition) / Time.deltaTime;
        }
    }
}