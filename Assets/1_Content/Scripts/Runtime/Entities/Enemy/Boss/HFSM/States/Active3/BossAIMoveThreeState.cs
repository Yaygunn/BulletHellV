using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class BossAIMoveThreeState : BossAIActiveThreeState
    {
        private Vector2[] _destinations = new []
        {
            new Vector2(-7.3f, 3.3f),
            new Vector2(7.2f, 3.2f),
            new Vector2(7f, -3.5f),
            new Vector2(-7.5f, -3.3f)
        };
        
        public BossAIMoveThreeState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            _bossAI.StateName = "Phase1 Move";
            _bossAI.Animator.SetBool(_bossAI.AnimatorParams.IsMovingBool, true);

            _bossAI.Collider.enabled = false;
            _bossAI.Movement.MoveTo(GetRandomDestination(), _bossAI.Stats.CurrentSpeed * 2, OnReachedDestination);
            
            _bossAI.Feedbacks.StartMoveFeedbackPlayer?.PlayFeedbacks();
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
            _bossAI.Feedbacks.StopMoveFeedbackPlayer?.PlayFeedbacks();
            _bossAI.EnemyHFSM.ChangeState(_bossAI.ShootThreeState);
        }
        
        private Vector2 GetRandomDestination()
        {
            return _destinations[Random.Range(0, _destinations.Length)];
        }
    }
}