using BH.Runtime.StateMachines;
using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class BossAIShootTwoState : BossAIActiveTwoState
    {
        private bool _isLeftSide = true;
        
        public BossAIShootTwoState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _bossAI.StateName = "Shoot";
            _bossAI.Animator.SetTrigger(_bossAI.AnimatorParams.IsAttackingTrigger);
            
            _bossAI.ShootPattern.ShootPatternCompletedEvent += OnShootPatternCompleted;

            ProjectilePatternDataSO patternData = _isLeftSide ? _bossAI.PhasetwoLeft : _bossAI.PhasetwoRight;
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
            
            _isLeftSide = !_isLeftSide;
            _bossAI.ShootPattern.ShootPatternCompletedEvent -= OnShootPatternCompleted;
            _bossAI.ShootPattern.StopPattern();
        }

        private void OnShootPatternCompleted()
        {
            if (_bossAI.Stats.CurrentHealth <= _bossAI.Stats.MaxHealth * 0.30f)
            {
                _stateMachine.ChangeState(_bossAI.MoveThreeState);
            }
            else
            {
                _stateMachine.ChangeState(_bossAI.MoveTwoState);
            }
        }
    }
}