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

        public void SetBulletVisual(Sprite sprite, int level)
        {
            _bulletImage.color = Color.white;
            _bulletImage.sprite = sprite;
            
            if (level != 0)
                _levelText.text = level.ToString();
        }
    }
}