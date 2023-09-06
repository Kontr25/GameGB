using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class DontDestroyObject : MonoBehaviour
    {
        private static Dictionary<string, GameObject> _instances = new Dictionary<string, GameObject>();
        private string ID;

        void Awake()
        {
            ID = gameObject.name;
            
            if (_instances.ContainsKey(ID))
            {
                var existing = _instances[ID];

                if (existing != null)
                {
                    if (ReferenceEquals(gameObject, existing))
                    {
                        return;
                    }

                    Destroy(gameObject);

                    return;
                }
            }

            _instances[ID] = gameObject;

            DontDestroyOnLoad(gameObject);
        }
    }
}