using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class AbilityComponent : MonoBehaviour
    {
        [InfoBox("This is an example ability without strategy pattern. I will be implementing it after testing...")]
        [BoxGroup("Dash Ability"), SerializeField]
        private float _dashSpeed = 10f;
        [BoxGroup("Dash Ability"), SerializeField]
        private float _dashDuration = 0.5f;
        [BoxGroup("Dash Ability"), SerializeField]
        private float _dashCooldown = 5f;
        
        private Rigidbody2D _rigidbody;
        private bool _isOnCooldown;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                Debug.LogError("[AbilityComponent] Rigidbody2D is missing. Disabling component...");
                enabled = false;
            }
        }
        
        public void UseAbility()
        {
            
        }
    }
}