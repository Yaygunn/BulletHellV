using System;
using BH.Runtime.Systems;
using BH.Scriptables;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BH.Scripts.Runtime.UI
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField]
        private Image _iconImage;
        [SerializeField]
        private TMP_Text _descriptionText;
        [SerializeField]
        private RectTransform _rectTransform;

        private Button _button;
        private int _buttonIndex;

        public event Action<int> ButtonClickedEvent;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void SetUp(int index)
        {
            _buttonIndex = index;
        }

        public void UpdateUpgradeDisplay(UpgradeOption upgradeOption)
        {
            switch (upgradeOption.Type)
            {
                case UpgradeType.AddBullet:
                    _iconImage.sprite = upgradeOption.ProjectileData.Icon;
                    _descriptionText.text = BuildProjectileDescription(upgradeOption.ProjectileData);
                    break;
                case UpgradeType.UpgradeBullet:
                    _iconImage.sprite = upgradeOption.ProjectileData.Icon;
                    _descriptionText.text = BuildProjectileDescription(upgradeOption.ProjectileData);
                    break;
                case UpgradeType.UpgradeWeapon:
                    _iconImage.sprite = upgradeOption.WeaponUpgrade.Icon;
                    _descriptionText.text = BuildBasicDescription(upgradeOption.WeaponUpgrade.UpgradeName, 
                        upgradeOption.WeaponUpgrade.UpgradeDescription);
                    break;
                case UpgradeType.UpgradePlayer:
                    _iconImage.sprite = upgradeOption.StatUpgrade.Icon;
                    _descriptionText.text = BuildBasicDescription(upgradeOption.StatUpgrade.UpgradeName, 
                        upgradeOption.StatUpgrade.UpgradeDescription);
                    break;
            }

            //reset scale while maintaining width
            _iconImage.SetNativeSize();
            float width = _rectTransform.rect.width;
            float height = _rectTransform.rect.height;

            if (height != width)
            {
                float aspectRatio = _rectTransform.rect.height / width; 
                _rectTransform.sizeDelta = new Vector2 (width, width * aspectRatio);
            }
            
        }
        
        private string BuildProjectileDescription(ProjectileDataSO projectileData)
        {
            string description = $"{projectileData.ProjectileName}\n\n" +
                                 $"{projectileData.Description}\n\n" +
                                 $"<color=green>{projectileData.PosativeEffect}</color>\n\n" +
                                 $"<color=red>{projectileData.NegativeEffect}</color>";
            return description;
        }
        
        private string BuildBasicDescription(string upgradeName, string upgradeDescription)
        {
            string description = $"{upgradeName}\n\n" +
                                 $"{upgradeDescription}";
            return description;
        }

        private void OnButtonClicked()
        {
            ButtonClickedEvent?.Invoke(_buttonIndex);
        }
    }
}