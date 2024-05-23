using BH.Runtime.Audio;
using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIDeadState : BossAIState
    {
        public BossAIDeadState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _bossAI.WwiseEventHandler.PostAudioEvent(EnemySFX.Die, _bossAI.gameObject);
        }
    }
}