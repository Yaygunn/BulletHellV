using BH.Runtime.StateMachines;
using BH.Scriptables;

namespace BH.Runtime.Entities
{
    public class BossAIShootOneState : BossAIActiveOneState
    {
        public BossAIShootOneState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _bossAI.StateName = "Shoot";
            _bossAI.Animator.SetTrigger(_bossAI.AnimatorParams.IsAttackingTrigger);
            
            _bossAI.ShootPattern.ShootPatternCompletedEvent += OnShootPatternCompleted;
            ProjectilePatternDataSO patternData = _bossAI.PhaseOneCenter;
            _bossAI.ShootPattern.StartPattern(patternData);
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
            
            _bossAI.ShootPattern.ShootPatternCompletedEvent -= OnShootPatternCompleted;
            _bossAI.ShootPattern.StopPattern();
        }

        private void OnShootPatternCompleted()
        {
            //_stateMachine.ChangeState(_bossAI.MoveState);
        }
    }
}