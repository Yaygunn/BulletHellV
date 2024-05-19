using BH.Runtime.StateMachines;
using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class BossAIShootOneState : BossAIActiveOneState
    {
        private float _attackDuration;
        private float _attackTimer;
        private bool _patternCompleted;
        
        public BossAIShootOneState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _bossAI.StateName = "Shoot";
            _bossAI.Animator.SetTrigger(_bossAI.AnimatorParams.IsAttackingTrigger);
            
            _attackDuration = _bossAI.AnimatorParams.IsAttackingDuration;
            _attackTimer = 0f;
            
            _bossAI.ShootPattern.ShootPatternCompletedEvent += OnShootPatternCompleted;
            ProjectilePatternDataSO patternData = _bossAI.PhaseOneCenter;
            _bossAI.ShootPattern.StartPattern(patternData);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _attackTimer += Time.deltaTime;
            
            if (_attackTimer >= _attackDuration && _patternCompleted)
            {
                _bossAI.Animator.SetTrigger(_bossAI.AnimatorParams.IsAttackingTrigger);
                ProjectilePatternDataSO patternData = _bossAI.PhaseOneCenter;
                _bossAI.ShootPattern.StartPattern(patternData);
                _attackTimer = 0f;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            
            _patternCompleted = false;
            _bossAI.ShootPattern.ShootPatternCompletedEvent -= OnShootPatternCompleted;
            _bossAI.ShootPattern.StopPattern();
        }

        private void OnShootPatternCompleted()
        {
            _patternCompleted = true;
            //_stateMachine.ChangeState(_bossAI.MoveState);
        }
    }
}