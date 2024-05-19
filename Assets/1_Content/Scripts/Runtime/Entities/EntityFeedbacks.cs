﻿using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public class EntityFeedbacks
    {
        [field: SerializeField]
        public MMF_Player HitFeedbackPlayer { get; private set; }
    }
}