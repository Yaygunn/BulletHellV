using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class EnemyIdleState : EnemyActiveState
    {
        public EnemyIdleState(EnemyController enemy, StateMachine<EnemyState> stateMachine) : base(enemy, stateMachine)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (ShouldChase())
            {
                _enemy.EnemyHFSM.ChangeState(_enemy.ChaseState);
            }
        }

        private bool ShouldChase()
        {
            return _enemy.AttackTarget != null;
        }
    }
}