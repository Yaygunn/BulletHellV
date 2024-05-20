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
                _patternCompleted = false;
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
            Debug.Log($"Current Health: {_bossAI.Stats.CurrentHealth} Max Health %: {_bossAI.Stats.MaxHealth * 0.70f}");
            if (_bossAI.Stats.CurrentHealth <= _bossAI.Stats.MaxHealth * 0.70f)
            {
                _stateMachine.ChangeState(_bossAI.MoveTwoState);
            }
            else
            {
                _patternCompleted = true;
            }
        }
    }
}