using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public abstract class MeleeAIState : BaseState<MeleeAIState>
    {
        protected AIMeleeController _meleeAI;

        public MeleeAIState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(stateMachine)
        {
            _meleeAI = meleeAI;
        }
        
        public virtual void On2DCollisionEnter(Collision2D other)
        {
            // if (other.gameObject.TryGetComponent(out IDamageable damageable))
            // {
            //     Vector2 direction = (other.transform.position - _meleeAI.transform.position).normalized;
            //     damageable.HandleDamageWithForce(_meleeAI.CurrentDamage, direction, _meleeAI.PushForce);
            // }
        }
    }
}