using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class MeleeAIAttackState : MeleeAIActiveState
    {
        private float _attackDuration;
        private float _attackTimer;
        
        public MeleeAIAttackState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(meleeAI, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _meleeAI.StateName = "Attack";
            _meleeAI.Animator.SetTrigger(_meleeAI.AnimatorParams.IsAttackingTrigger);
            
            _attackDuration = _meleeAI.AnimatorParams.IsAttackingDuration;
            _attackTimer = 0f;
            
            AttackPlayer();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _attackTimer += Time.deltaTime;
            
            if (_attackTimer >= _attackDuration)
            {
                if (ShouldAttackAgain())
                {
                    AttackPlayer();
                    _attackTimer = 0f;
                }
                else
                {
                    _meleeAI.EnemyHFSM.ChangeState(_meleeAI.ChaseState);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        private void AttackPlayer()
        {
            if (_meleeAI.PlayerTarget.gameObject.TryGetComponent(out IDamageable damageable))
            {
                Vector2 direction = (_meleeAI.PlayerTarget.transform.position - _meleeAI.transform.position).normalized;
                damageable.HandleDamageWithForce(_meleeAI.CurrentDamage, direction, _meleeAI.PushForce);
            }
        }
        
        private bool ShouldAttackAgain()
        {
            Vector2 distance = _meleeAI.PlayerTarget.transform.position - _meleeAI.transform.position;
            return distance.magnitude <= _meleeAI.AttackRange;
        }
    }
}