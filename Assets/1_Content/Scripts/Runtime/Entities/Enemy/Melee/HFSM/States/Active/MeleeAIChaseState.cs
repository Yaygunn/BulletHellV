using BH.Runtime.Entities;
using BH.Runtime.StateMachines;
using UnityEngine;

public class MeleeAIChaseState : MeleeAIActiveState
{
    public MeleeAIChaseState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(meleeAI, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        _meleeAI.StateName = "Chase";
        _meleeAI.Animator.SetBool(_meleeAI.AnimatorParams.IsMovingBool, true);
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (ShouldMove())
        {
            Vector2 direction = GetAttackTargetDirection();
            _meleeAI.Movement.Move(direction, _meleeAI.Stats.CurrentSpeed);
        }
        else if (ShouldAttack())
        {
            _meleeAI.EnemyHFSM.ChangeState(_meleeAI.AttackState);
        }

        ShouldAttack();
    }
    
    public override void Exit()
    {
        base.Exit();
        
        _meleeAI.Movement.Stop();
        _meleeAI.Animator.SetBool(_meleeAI.AnimatorParams.IsMovingBool, false);
    }
    
    private bool ShouldMove()
    {
        if (_meleeAI.PlayerTarget.PlayerHFSM.CurrentState == _meleeAI.PlayerTarget.DeadState)
            return false;
        
        Vector2 distance = _meleeAI.PlayerTarget.transform.position - _meleeAI.transform.position;
        return distance.magnitude > _meleeAI.AttackRange;
    }
    
    private bool ShouldAttack()
    {
        return _meleeAI.PlayerTarget.PlayerHFSM.CurrentState != _meleeAI.PlayerTarget.DeadState;
    }
    
    private Vector2 GetAttackTargetDirection()
    {
        Vector2 attackTaget = (Vector2)_meleeAI.PlayerTarget.transform.position;
        Vector2 attackTargetDirection = (attackTaget - (Vector2)_meleeAI.transform.position).normalized;
        return attackTargetDirection;
    }
}
