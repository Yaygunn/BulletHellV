using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class MeleeAIIdleState : MeleeAIActiveState
    {
        public MeleeAIIdleState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(meleeAI, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _meleeAI.StateName = "Idle";
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (ShouldChase())
            {
                _meleeAI.EnemyHFSM.ChangeState(_meleeAI.ChaseState);
            }
        }

        private bool ShouldChase()
        {
            return _meleeAI.AttackTarget != null && _meleeAI.AttackTarget.transform.gameObject.activeSelf;
        }
    }
}