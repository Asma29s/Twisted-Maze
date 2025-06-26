using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManger : MonoBehaviour
{
    public void ReloadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
