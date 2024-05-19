using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BH.Runtime.UI
{
    public class BulletVisual : MonoBehaviour
    {
        private Image _bulletImage;
        private TMP_Text _levelText;
        
        private void Awake()
        {
            _bulletImage = GetComponent<Image>();
            _levelText = GetComponentInChildren<TMP_Text>();
        }

        public void SetBulletVisual(Color color, int level)
        {
            _bulletImage.color = color;
            
            if (level != 0)
                _levelText.text = level.ToString();
        }
    }
}