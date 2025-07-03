using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JumpScare : MonoBehaviour
{
    public Image jumpScareImage; 
    public float displayDuration = 2f; 

    public void TriggerJumpScare()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowJumpScare());
    }

    private IEnumerator ShowJumpScare()
    {
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false); // if no need to end disactive the image remove this
    }
}
