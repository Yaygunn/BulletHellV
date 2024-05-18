using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class MeleeAIDeadState : MeleeAIState
    {
        private float _deadTimer;
        private float _deadDuration;
        
        public MeleeAIDeadState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(meleeAI, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _meleeAI.StateName = "Dead";
            _meleeAI.Animator.SetTrigger(_meleeAI.AnimatorParams.IsDeadTrigger);
            
            _deadDuration = _meleeAI.AnimatorParams.IsDeadDuration;
            _deadTimer = 0f;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _deadTimer += Time.deltaTime;
            
            if (_deadTimer >= _deadDuration)
            {
                _meleeAI.ReturnToPool();
            }
        }
    }
}