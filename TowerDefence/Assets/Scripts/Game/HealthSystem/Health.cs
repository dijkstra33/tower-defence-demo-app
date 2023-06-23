using System;
using Core.ObjectPooling;
using Game.AttributeSystem;
using Game.AttributeSystem.Buffs;
using UnityEngine;

namespace Game.HealthSystem
{
    [RequireComponent(typeof(AbstractAttributeOwner))]
    public class Health : MonoBehaviour, IResettable
    {
        public event Action<Health> OnDamageReceived;
        public event Action OnDeath;
        
        public int MaxValue => maxValue;
        [SerializeField]
        private int maxValue;

        public int CurrentValue => currentValue;
        private int currentValue;

        private bool isDead = false;

        private AbstractAttributeOwner attributeOwner;
        private BuffHolder buffHolder;

        private void Awake()
        {
            currentValue = maxValue;
            attributeOwner = GetComponent<AbstractAttributeOwner>();
            buffHolder = GetComponent<BuffHolder>();
        }

        public void ReceiveDamage(float damage, Health attackerHealth = null, BuffHolder attackerBuffHolder = null)
        {
            if (isDead)
            {
                return;
            }

            var pureDamage = GetPureDamage(damage);
            OnDamageReceived?.Invoke(attackerHealth);
            currentValue = Math.Max(currentValue - pureDamage, 0);

            if (currentValue == 0)
            {
                Die();
            }
            else
            {
                TryToApplyBuffsOnHit(attackerBuffHolder);
            }
        }

        private void TryToApplyBuffsOnHit(BuffHolder attackerBuffHolder)
        {
            if (attackerBuffHolder == null || buffHolder == null)
            {
                return;
            }
            
            var buffsOnHit = attackerBuffHolder.BuffsToTargetOnHit;
            foreach (var buffOnHit in buffsOnHit)
            {
                buffHolder.ApplyBuff(buffOnHit, BuffApplicationType.Direct);
            }
        }

        private int GetPureDamage(float damage)
        {
            var armor = (int)attributeOwner.GetValue(AttributeType.Armor);
            return Math.Max(1, (int)damage - armor);
        }

        private void Die()
        {
            isDead = true;
            DeathManager.Instance.OnDeath(gameObject);
            OnDeath?.Invoke();
        }

        public void Reset()
        {
            isDead = false;
            currentValue = maxValue;
        }
    }
}
