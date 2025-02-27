using UnityEngine;

namespace NuiN.NExtensions
{
    public class VelocityCalculator : MonoBehaviour
    {
        [ShowInInspector] public Vector3 Velocity { get; private set; }
        Vector3 _position;
        Vector3 _previousPosition;
    
        void FixedUpdate()
        {
            _previousPosition = _position;
            _position = transform.position;
            Velocity = (_position - _previousPosition) / Time.fixedDeltaTime;
        }
    }
}