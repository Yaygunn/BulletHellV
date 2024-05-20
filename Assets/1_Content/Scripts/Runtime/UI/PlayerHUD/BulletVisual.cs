using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BH.Runtime.UI
{
    public class BulletVisual : MonoBehaviour
    {
        private Image _bulletImage;
        private TMP_Text _levelText;
        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _bulletImage = GetComponent<Image>();
            _levelText = GetComponentInChildren<TMP_Text>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetBulletVisual(Sprite sprite, int level)
        {
            _bulletImage.color = Color.white;
            _bulletImage.sprite = sprite;

            //reset scale while maintaining width
            _bulletImage.SetNativeSize();
            float width = _rectTransform.rect.width;
            float aspectRatio = _rectTransform.rect.height / width; 
            _rectTransform.sizeDelta = new Vector2 (50, 50 * aspectRatio);

            
            if (level != 0)
                _levelText.text = level.ToString();
        }
    }
}