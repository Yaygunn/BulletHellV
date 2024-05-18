using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class RangedAIShootState : RangedAIActiveState
    {
        public RangedAIShootState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _rangedAI.ShootPattern.ShootPatternCompletedEvent += OnShootPatternCompleted;
            _rangedAI.ShootPattern.StartPattern();
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
            
            _rangedAI.ShootPattern.ShootPatternCompletedEvent -= OnShootPatternCompleted;
            _rangedAI.ShootPattern.StopPattern();
        }

        private void OnShootPatternCompleted()
        {
            _stateMachine.ChangeState(_rangedAI.MoveState);
        }
    }
}