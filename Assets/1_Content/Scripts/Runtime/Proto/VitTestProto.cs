using System;
using BH.Runtime.Managers;
using BH.Utilities;
using UnityEngine;
using Zenject;

namespace BH.Runtime
{
    public class VitTestProto : MonoBehaviour
    {
        [Inject]
        private GameManager _gameManager;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameManager.TestMethod();
            }
        }
    }
}