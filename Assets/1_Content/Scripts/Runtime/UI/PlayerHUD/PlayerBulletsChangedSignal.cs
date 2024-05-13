using System.Collections.Generic;
using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.UI
{
    public struct PlayerBulletsChangedSignal
    {
        public List<Color> EvolutionColors { get; }
        public List<int> EvolutionLevels { get; }
        
        public PlayerBulletsChangedSignal(List<Color> evolutionColors, List<int> evolutionLevels)
        {
            EvolutionColors = evolutionColors;
            EvolutionLevels = evolutionLevels;
        }
    }
}