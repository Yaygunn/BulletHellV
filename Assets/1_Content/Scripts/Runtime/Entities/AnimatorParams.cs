using System;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public struct AnimatorParams
    {
        [field: SerializeField]
        public string IsMovingBool { get; private set; }
        [field: SerializeField]
        public string IsAttackingTrigger { get; private set; }
        [field: SerializeField]
        public float IsAttackingDuration { get; private set; }
        [field: SerializeField]
        public string IsDeadTrigger { get; private set; }
        [field: SerializeField]
        public float IsDeadDuration { get; private set; }
    }
}