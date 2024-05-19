using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class BossAIMoveOneState : BossAIActiveOneState
    {
        public BossAIMoveOneState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _bossAI.StateName = "Phase1 Move";
            _bossAI.Animator.SetBool(_bossAI.AnimatorParams.IsMovingBool, true);

            _bossAI.Collider.enabled = false;
            _bossAI.Movement.MoveTo(Vector2.zero, _bossAI.Stats.CurrentSpeed, OnReachedDestination);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            
            _bossAI.Animator.SetBool(_bossAI.AnimatorParams.IsMovingBool, false);
            _bossAI.Collider.enabled = true;
        }
        
        private void OnReachedDestination()
        {
            _bossAI.EnemyHFSM.ChangeState(_bossAI.ShootOneState);
        }
    }
}