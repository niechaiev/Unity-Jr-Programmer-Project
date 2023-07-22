using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    [CreateAssetMenu(fileName = "ResourcesDatabase", menuName = "Tutorial/Resources Database")]
    public class ResourceDatabase : ScriptableObject
    {
        public List<ResourceItem> ResourceTypes = new();

        private Dictionary<string, ResourceItem> database;
    
        public void Init()
        {
            database = new Dictionary<string, ResourceItem>();
            foreach (var resourceItem in ResourceTypes)
            {
                database.Add(resourceItem.Id, resourceItem);
            }
        }

        public ResourceItem GetItem(string uniqueId)
        {
            database.TryGetValue(uniqueId, out ResourceItem type);
            return type;
        }
    }
}
