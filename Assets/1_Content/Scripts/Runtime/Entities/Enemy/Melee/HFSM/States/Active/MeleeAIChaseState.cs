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

        Vector2 direction = GetAttackTargetDirection();
        _meleeAI.Movement.Move(direction, _meleeAI.Stats.CurrentSpeed);
        
        ShouldIdle();
    }
    
    public override void Exit()
    {
        base.Exit();
        
        _meleeAI.Movement.Stop();
        _meleeAI.Animator.SetBool(_meleeAI.AnimatorParams.IsMovingBool, false);
    }
    
    private void ShouldIdle()
    {
        if (!_meleeAI.AttackTarget.gameObject.activeSelf)
        {
            _stateMachine.ChangeState(_meleeAI.AttackState);
        }
    }
    
    private Vector2 GetAttackTargetDirection()
    {
        Vector2 attackTaget = (Vector2)_meleeAI.AttackTarget.transform.position;
        Vector2 attackTargetDirection = (attackTaget - (Vector2)_meleeAI.transform.position).normalized;
        return attackTargetDirection;
    }
}
