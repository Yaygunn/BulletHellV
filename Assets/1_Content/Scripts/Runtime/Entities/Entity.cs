using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        // TODO: Maybe set up animator here?
        
        private List<IEntityComponent> _components = new ();
        
        protected virtual void Awake()
        {
            IEntityComponent[] components = GetComponentsInChildren<IEntityComponent>();
            _components = components.ToList();
        }
        
        public T GetEntityComponent<T>() where T : class, IEntityComponent
        {
            return _components.OfType<T>().FirstOrDefault();
        }
        
        public bool TryGetEntityComponent<T>(out T component) where T : class, IEntityComponent
        {
            component = _components.OfType<T>().FirstOrDefault();
            return component != null;
        }
        
        protected T VerifyComponent<T>() where T : class, IEntityComponent
        {
            if (TryGetEntityComponent(out T component))
                return component;
            
            Debug.LogError($"[Entity] Missing a critical component of type {typeof(T)}.", this);
            return null;
        }
        
    }
}