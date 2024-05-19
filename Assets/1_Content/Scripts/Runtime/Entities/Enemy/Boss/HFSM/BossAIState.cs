using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class BossAIState : BaseState<BossAIState>
    {
        protected AIBossController _bossAI;
        
        public BossAIState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(stateMachine)
        {
            _bossAI = controller;
        }
        
        public virtual void On2DCollisionEnter(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                Vector2 direction = (other.transform.position - _bossAI.transform.position).normalized;
                damageable.HandleDamageWithForce(_bossAI.CurrentTouchDamage, direction, _bossAI.PushForce);
            }
        }
    }
}

