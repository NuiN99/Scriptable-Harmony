using UnityEngine;

namespace NuiN.NExtensions
{
    public class Health : MonoBehaviour
    {
        IDamageable[] _damageables;
        
        [SerializeField, ReadOnlyPlayMode] float currentHealth;
        [SerializeField] float maxHealth;
        [SerializeField] bool allowOverHeal;
        
        public bool Dead { get; private set; }
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool AllowOverHeal => allowOverHeal;

        void Awake()
        {
            _damageables = GetComponents<IDamageable>();
            currentHealth = maxHealth;
        }

        public void Damage(float damage, Vector3 direction = default)
        {
            if (Dead) return;
            
            currentHealth -= damage;
            _damageables.ForEach(dmg => dmg.OnDamaged(damage, direction));

            if (currentHealth > 0) return;
            
            Dead = true;
            currentHealth = 0;
            _damageables.ForEach(dmg => dmg.OnDied(damage, direction));
        }

        public void Heal(float amount)
        {
            if (allowOverHeal) currentHealth += amount;
            else currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            
            _damageables.ForEach(dmg => dmg.OnHealed(amount));
        }
    }
}