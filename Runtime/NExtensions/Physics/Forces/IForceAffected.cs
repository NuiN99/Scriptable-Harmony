using UnityEngine;

namespace NExtensions.Forces
{
    public interface IForceAffected
    {
        void OnForceApplied(Vector3 direction, float force);
    }
}