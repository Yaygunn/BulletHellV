using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public interface IDamageable
    {
        public void HandleDamage(int amount);
        public void HandleDamageWithForce(int amount, Vector2 direction, float force);
    }
}
