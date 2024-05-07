using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Runtime.Systems
{
    public struct HealthChangedSignal
    {
        public int MaxHealth { get; }
        public int CurrentHealth { get; }

        public HealthChangedSignal(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }
    }
}
