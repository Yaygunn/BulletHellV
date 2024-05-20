using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public class EntityFeedbacks
    {
        [field: SerializeField]
        public MMF_Player StartMoveFeedbackPlayer { get; private set; }
        [field: SerializeField]
        public MMF_Player StopMoveFeedbackPlayer { get; private set; }
        [field: SerializeField]
        public MMF_Player AttackFeedbackPlayer { get; private set; }
        [field: SerializeField]
        public MMF_Player HitFeedbackPlayer { get; private set; }
        [field: SerializeField]
        public MMF_Player DieFeedbackPlayer { get; private set; }
        [field: SerializeField]
        public MMF_Player RespawnFeedbackPlayer { get; private set; }
    }
}