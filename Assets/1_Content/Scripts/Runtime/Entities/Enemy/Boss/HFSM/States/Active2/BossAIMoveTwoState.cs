using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class BossAIMoveTwoState : BossAIActiveTwoState
    {
        private Vector2 _leftSideDestination = new (-7f, 0f);
        private Vector2 _rightSideDestination = new (6.5f, 3f);
        private bool _isAtLeftSide;
        
        public BossAIMoveTwoState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            _bossAI.StateName = "Phase2 Move";
            _bossAI.Animator.SetBool(_bossAI.AnimatorParams.IsMovingBool, true);

            _bossAI.Collider.enabled = false;
            if (_isAtLeftSide)
                _bossAI.Movement.MoveTo(_rightSideDestination, _bossAI.Stats.CurrentSpeed, OnReachedDestination);
            else
                _bossAI.Movement.MoveTo(_leftSideDestination, _bossAI.Stats.CurrentSpeed, OnReachedDestination);
            
            _bossAI.Feedbacks.StartMoveFeedbackPlayer?.PlayFeedbacks();
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
            _isAtLeftSide = !_isAtLeftSide;
            _bossAI.EnemyHFSM.ChangeState(_bossAI.ShootTwoState);
        }
    }
}