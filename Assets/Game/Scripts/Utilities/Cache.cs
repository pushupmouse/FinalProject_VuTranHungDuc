using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Cache<T> : MonoBehaviour where T : Component
{
    private static Dictionary<Collider2D, T> componentCache = new Dictionary<Collider2D, T>();

    public static T GetComponent(Collider2D collider)
    {
        if (collider == null)
        {
            return null; // Return null if collider is null
        }

        if (!componentCache.ContainsKey(collider))
        {
            T component = collider.GetComponent<T>();
            if (component != null)
            {
                componentCache.Add(collider, component);
            }
            else
            {
                return null; // Return null if component is not found
            }
        }

        return componentCache[collider];
    }
}
