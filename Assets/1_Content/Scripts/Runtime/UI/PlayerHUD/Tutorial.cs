using System.Collections;
using UnityEngine;

namespace BH.Runtime.UI
{
    public class Tutorial : MonoBehaviour
    {
        public void Start()
        {
            StartCoroutine(Disappear());
        }

        IEnumerator Disappear()
        {
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
    }
}