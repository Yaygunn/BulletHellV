using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class RangedAIState : BaseState<RangedAIState>
    {
        protected AIRangedController _rangedAI;
        
        public RangedAIState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(stateMachine)
        {
            _rangedAI = rangedController;
        }
        
        public virtual void On2DCollisionEnter(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                Vector2 direction = (other.transform.position - _rangedAI.transform.position).normalized;
                damageable.HandleDamageWithForce(_rangedAI.CurrentTouchDamage, direction, _rangedAI.PushForce);
            }
        }
    }
}