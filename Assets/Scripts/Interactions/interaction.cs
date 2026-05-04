using UnityEngine;

public class interaction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactionLayers;

    private IInteractable lastInteractable;

    void Update()
    {
        var cam = Camera.main;
        if (!cam) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // center of screen
        bool hitSomething = Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance,
            interactionLayers,
            QueryTriggerInteraction.Ignore
        );

        if (!hitSomething)
        {
            HideLastPrompt();
            return;
        }

        // In case the collider is on a child object
        IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

        if (interactable == null)
        {
            HideLastPrompt();
            return;
        }

        if (lastInteractable != null && lastInteractable != interactable)
            lastInteractable.ShowPrompt(false);

        lastInteractable = interactable;
        interactable.ShowPrompt(true);

        if (Input.GetKeyDown(KeyCode.E))
            interactable.Interact();
    }

    void HideLastPrompt()
    {
        if (lastInteractable != null)
        {
            lastInteractable.ShowPrompt(false);
            lastInteractable = null;
        }
    }
}