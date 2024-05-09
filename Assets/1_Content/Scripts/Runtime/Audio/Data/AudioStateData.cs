using System;
using UnityEngine;

namespace BH.Runtime.Audio
{
    [Serializable]
    public struct AudioStateData
    {
        [field: SerializeField]
        public AudioState Type { get; private set; }
        [field: SerializeField]
        public AK.Wwise.State WwiseState { get; private set; }
    }
}