using UnityEngine;

public class FlashlightSystem : MonoBehaviour
{
    public static FlashlightSystem Instance; //singleton

    public GameObject flashlight;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            flashlight.SetActive(false); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void FlashlightActive()
    {
        flashlight.SetActive(true);
    }
}
