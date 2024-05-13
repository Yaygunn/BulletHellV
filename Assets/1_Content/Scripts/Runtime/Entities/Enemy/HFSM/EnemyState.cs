using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public abstract class EnemyState : BaseState<EnemyState>
    {
        protected EnemyController _enemy;

        public EnemyState(EnemyController enemy, StateMachine<EnemyState> stateMachine) : base(stateMachine)
        {
            _enemy = enemy;
        }
    }
}