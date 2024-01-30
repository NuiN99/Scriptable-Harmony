using UnityEngine;

namespace NuiN.NExtensions
{
    public interface IDamageable
    {
        public void OnDamaged(float damage, Vector3 direction = default);
        public void OnDied(float damage, Vector3 direction = default) { }
        public void OnHealed(float amount) { }
    }
}