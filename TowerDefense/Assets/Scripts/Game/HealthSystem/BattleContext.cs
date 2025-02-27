﻿using System.Collections.Generic;
using System.Linq;
using Core.ObjectPooling;
using Game.WeaponSystem;
using UnityEngine;

namespace Game.HealthSystem
{
    public class BattleContext : MonoBehaviour, IBeforeGetFromPool
    {
        private readonly Dictionary<AbstractWeapon, int> hitsCountByWeapon = new();

        private void Start()
        {
            BattleContextManager.Instance.Register(this);
        }

        public void RegisterHitBy(AbstractWeapon weapon)
        {
            if (weapon == null)
            {
                return;
            }
            
            if (!hitsCountByWeapon.ContainsKey(weapon))
            {
                hitsCountByWeapon[weapon] = 0;
            }

            hitsCountByWeapon[weapon]++;
        }

        public int GetHitsCountBy(AbstractWeapon weapon)
        {
            return hitsCountByWeapon.TryGetValue(weapon, out var hitsCount) ? hitsCount : 0;
        }

        public void RemoveInfluenceOf(AbstractWeapon weapon)
        {
            if (hitsCountByWeapon.ContainsKey(weapon))
            {
                hitsCountByWeapon[weapon] = 0;
            }
        }
        
        public void Execute()
        {
            var keys = hitsCountByWeapon.Keys.ToArray();
            foreach (var key in keys)
            {
                hitsCountByWeapon[key] = 0;
            }
        }
    }
}