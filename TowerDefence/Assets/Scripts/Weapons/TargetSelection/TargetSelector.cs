﻿using UnityEngine;

namespace TowerDefence.TargetSelection
{
    public abstract class TargetSelector : ScriptableObject
    {
        protected float attackRange;

        public abstract TargetInfo? SelectTarget(Vector3 transformPosition);
        
        public void SetAttackRange(float attackRange)
        {
            this.attackRange = attackRange;
        }
    }
}