using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities.States.Active
{
    public class RangedAIMoveState : RangedAIActiveState
    {
        private Vector2 _movePoint;
        
        public RangedAIMoveState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            //_movePoint = GetNextValidPoint();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            //_rangedAI.Movement.Move();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            
            _rangedAI.Movement.Stop();
        }
        
        // private Vector2 GetNextValidPoint()
        // {
        //     
        // }
    }
}