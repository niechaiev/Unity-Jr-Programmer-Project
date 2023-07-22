using UnityEngine;

namespace Helpers
{
    [CreateAssetMenu(fileName = "ResourceItem", menuName = "Tutorial/Resource Item")]
    public class ResourceItem : ScriptableObject
    {
        public string Name;
        public string Id;
        public Sprite Icone;
    }
}
