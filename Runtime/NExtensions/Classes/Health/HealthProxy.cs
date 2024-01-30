using UnityEngine;

namespace NuiN.NExtensions
{
    public class HealthProxy : MonoBehaviour, IDamageable
    {
        [SerializeField] Health target;

        void Reset() => target = GetComponentInParent<Health>() ?? GetComponentInChildren<Health>();

        void IDamageable.OnDamaged(float damage, Vector3 direction) => target.Damage(damage, direction);
        void IDamageable.OnHealed(float amount) => target.Heal(amount);
    }
}