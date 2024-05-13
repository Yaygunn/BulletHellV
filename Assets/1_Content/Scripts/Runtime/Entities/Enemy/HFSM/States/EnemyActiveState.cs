using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class EnemyActiveState : EnemyState
    {
        public EnemyActiveState(EnemyController enemy, StateMachine<EnemyState> stateMachine) : base(enemy, stateMachine)
        {
            
        }
    }
}