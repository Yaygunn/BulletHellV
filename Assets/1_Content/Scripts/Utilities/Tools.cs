using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DP.Utilities
{
    public static class Tools
    {
        /// <summary>
        /// Will get all the layers that are currently set in the editor that are not blank.
        /// </summary>
        public static IEnumerable<string> GetLayerNames()
        {
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName != "")
                {
                    yield return layerName;
                }
            }
        }
        
        /// <summary>
        /// Generates a collection of layer indices for all non-empty layers defined in the Unity Editor
        /// </summary>
        public static IEnumerable<int> GetLayerIndices()
        {
            return GetLayerNames().Select(LayerMask.NameToLayer);
        }

        /// <summary>
        /// Will Debug.Log all layers in a given mask. Optionally provide a collider name for easier readability.
        /// </summary>
        public static void LogLayersInMask(LayerMask mask, string colliderName = "NOT PROVIDED")
        {
            for (int i = 0; i < 32; i++)
            {
                int shifted = 1 << i; // Shift 1 by i places to get the mask for layer i
                if ((mask.value & shifted) == shifted) // Check if the mask includes this layer
                {
                    string layerName = LayerMask.LayerToName(i);
                    Debug.Log("Name: " + colliderName + " Layer: " + i + ": " + layerName);
                }
            }
        }
        
        /// <summary>
        /// Toggles the visibility of a CanvasGroup.
        /// </summary>
        public static void ToggleVisibility(CanvasGroup canvasGroup, bool isVisible)
        {
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.blocksRaycasts = isVisible;
            canvasGroup.interactable = isVisible;
        }
        
        public static bool TryLoadResource<T>(string resourceName, out T asset) where T : Object
        { 
            asset = Resources.Load<T>(resourceName);
            if (asset != null) return true;
            
            Debug.LogError($"{typeof(T).Name} asset file '{resourceName}' not found in the resources folder.");
            return false;
        }
    }
}