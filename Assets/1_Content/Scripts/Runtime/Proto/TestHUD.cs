using UnityEngine;
using UnityEngine.UI;

namespace BH.Runtime.Test
{
    public class TestHUD : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;
        
        private void Start()
        {
            _slider.value = 1f;
        }
        
        public void SetSliderValue(float value)
        {
            _slider.value = value;
        }
        
        public float GetSliderValue()
        {
            return _slider.value;
        }
    }
}