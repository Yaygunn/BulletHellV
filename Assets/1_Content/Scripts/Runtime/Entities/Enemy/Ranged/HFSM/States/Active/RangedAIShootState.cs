using BH.Runtime.StateMachines;
using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class RangedAIShootState : RangedAIActiveState
    {
        private float _shootTimer;
        private float _shootDuration;
        
        public RangedAIShootState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _rangedAI.StateName = "Shoot";
            _rangedAI.Animator.SetTrigger(_rangedAI.AnimatorParams.IsAttackingTrigger);
            
            _rangedAI.ShootPattern.ShootPatternCompletedEvent += OnShootPatternCompleted;
            ProjectilePatternDataSO patternData = _rangedAI.ProjectilePatterns[Random.Range(0, _rangedAI.ProjectilePatterns.Count)];
            _rangedAI.ShootPattern.StartPattern(patternData);
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