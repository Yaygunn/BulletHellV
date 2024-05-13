using System;
using UnityEngine;

namespace BH.Runtime.Audio
{
    [Serializable]
    public struct AudioEventData<T> where T : Enum
    {
        [field: SerializeField]
        public T Type { get; private set; }
        [field: SerializeField]
        public AK.Wwise.Event WwiseEvent { get; private set; }
    }
}