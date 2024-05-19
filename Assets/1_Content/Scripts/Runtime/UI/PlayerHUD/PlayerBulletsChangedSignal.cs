using System.Collections.Generic;
using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.UI
{
    public struct PlayerBulletsChangedSignal
    {
        public List<Sprite> EvolutionIcons { get; }
        public List<int> EvolutionLevels { get; }
        
        public PlayerBulletsChangedSignal(List<Sprite> evolutionIcons, List<int> evolutionLevels)
        {
            EvolutionIcons = evolutionIcons;
            EvolutionLevels = evolutionLevels;
        }
    }
}