using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities.States.Active
{
    public class RangedAIMoveState : RangedAIActiveState
    {
        private Vector2 _movePoint;

        private float _minDistanceFromCenter = 2f;
        private float _marginFromEdges = 1f;

        public RangedAIMoveState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _movePoint = GetNextValidPoint();
            _rangedAI.Movement.MoveTo(_movePoint, _rangedAI.Stats.CurrentSpeed, OnReachDestination);
        }

        public override void Exit()
        {
            base.Exit();
            
            _rangedAI.Movement.Stop();
        }
        
        private Vector2 GetNextValidPoint()
        {
            Vector3 cameraPos = _rangedAI.Camera.transform.position;
            float cameraHeight = 2f * _rangedAI.Camera.orthographicSize;
            float cameraWidth = cameraHeight * _rangedAI.Camera.aspect;

            float minX = cameraPos.x - cameraWidth / 2 + _marginFromEdges;
            float maxX = cameraPos.x + cameraWidth / 2 - _marginFromEdges;
            float minY = cameraPos.y - cameraHeight / 2 + _marginFromEdges;
            float maxY = cameraPos.y + cameraHeight / 2 - _marginFromEdges;

            Vector2 randomPoint;
            do
            {
                float randomX = Random.Range(minX, maxX);
                float randomY = Random.Range(minY, maxY);
                randomPoint = new Vector2(randomX, randomY);
            } while (Vector2.Distance(randomPoint, cameraPos) < _minDistanceFromCenter);

            return randomPoint;
        }

        private void OnReachDestination()
        {
            _stateMachine.ChangeState(_rangedAI.ShootState);
        }
    }
}
