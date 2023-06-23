﻿using Game.HealthSystem;
using UnityEngine;

namespace Game.Weapons.TargetSelection
{
    public struct TargetInfo
    {
        public Health Health { get; }
        public Transform Transform { get; }
        public BattleContext BattleContext { get; }

        public TargetInfo(Health health, Transform transform)
        {
            Health = health;
            Transform = transform;
            BattleContext = BattleContextManager.Instance.GetBattleContextBy(health);
        }
    }
}