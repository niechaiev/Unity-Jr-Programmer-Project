using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InfoPopup : MonoBehaviour
    {
        public Text Name;
        public Text Data;
        public RectTransform ContentTransform;

        public ContentEntry EntryPrefab;

        public void ClearContent()
        {
            foreach (Transform child in ContentTransform)
            {
                Destroy(child.gameObject);
            }
        }
    
        public void AddToContent(int count, Sprite icone)
        {
            var newEntry = Instantiate(EntryPrefab, ContentTransform);

            newEntry.Count.text = count.ToString();
            newEntry.Icone.sprite = icone;
        }
    }
}
