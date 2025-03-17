using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance;
    public float interactionDistance = 10f;
    public LayerMask interactionLayer;
    public Text interactionText;

    public IInteractable currentInteractable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null && interactable != currentInteractable)
            {
                currentInteractable = interactable;
                if (interactable.IsInteractable() || interactable.IsAttackable())
                {
                    interactionText.text = interactable.GetName();
                }
            }
        }
        else
        {
            currentInteractable = null;
            interactionText.text = "";
        }

        // »¥¶¯Âß¼­
        /*if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null && currentInteractable.IsInteractable())
        {
            currentInteractable.Interact();
        }*/

        // ¹¥»÷Âß¼­
        /*if (Input.GetMouseButtonDown(0) && currentInteractable != null && currentInteractable.IsAttackable())
        {
            currentInteractable.TakeDamage("Sword", 10); // ¼ÙÉèÎäÆ÷Ãû³ÆÎª"Sword"£¬¹¥»÷ÖµÎª10
        }*/

        
    }

    public void TakeInteract(float maxdistance)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hit, maxdistance, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (interactable.IsInteractable())
                {
                    Debug.Log("PlayerInteract: Interact applied!");
                    interactable.Interact();
                }
            }
        }
    }

    public void TakeDamage(string ToolName, int hitPoint, float maxdistance)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hit, maxdistance, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (interactable.IsAttackable())
                {
                    Debug.Log("PlayerInteract: Damage applied!");
                    interactable.TakeDamage(ToolName, hitPoint);
                }
            }
        }
    }
}
