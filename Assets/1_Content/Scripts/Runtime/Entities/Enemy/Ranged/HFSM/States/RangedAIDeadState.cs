using BH.Runtime.Audio;
using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class RangedAIDeadState : RangedAIState
    {
        private float _deadTimer;
        private float _deadDuration;
        
        public RangedAIDeadState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _rangedAI.StateName = "Dead";
            _rangedAI.Animator.SetTrigger(_rangedAI.AnimatorParams.IsDeadTrigger);
            
            _deadDuration = _rangedAI.AnimatorParams.IsDeadDuration;
            _deadTimer = 0f;
            
            _rangedAI.Collider.enabled = false;
            _rangedAI.Feedbacks.DieFeedbackPlayer?.PlayFeedbacks();
            _rangedAI.WwiseEventHandler.PostAudioEvent(EnemySFX.Die, _rangedAI.gameObject);
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _deadTimer += Time.deltaTime;
            
            if (_deadTimer >= _deadDuration)
            {
                _rangedAI.ReturnToPool();
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            
            _rangedAI.Feedbacks.DieFeedbackPlayer?.StopFeedbacks();
            _rangedAI.Collider.enabled = true;
            _rangedAI.ModelRenderer.color = Color.white;
        }
    }
}