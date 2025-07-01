using UnityEngine;
using UnityEngine.UI;

public class ItemInteractor : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactableLayer;
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.green;

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            crosshairImage.color = hoverColor;

            if (Input.GetKeyDown(interactKey))
            {
                InteractableItem item = hit.collider.GetComponent<InteractableItem>();
                if (item != null)
                {
                    item.PickUp();
                }
            }
        }
        else
        {
            crosshairImage.color = normalColor;
        }
    }
}
