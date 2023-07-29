using UnityEngine;

public class HealthBarEnabler : MonoBehaviour
{
    public void Enable()
    {
       gameObject.SetActive(true); 
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
