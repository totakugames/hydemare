using UnityEngine;
using UnityEngine.Events;
public abstract class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] protected string promptText = "[E] Interact";
    [SerializeField] protected bool requiresHold;
    [SerializeField] protected float holdDuration = 1.5f;

    [Header("Events")]
    public UnityEvent OnInteractionStarted;
    public UnityEvent OnInteractionCompleted;
    public UnityEvent OnInteractionCancelled;

    public string PromptText => promptText;
    public bool RequiresHold => requiresHold;
    public float HoldDuration => holdDuration;

    //manu how do u make it? player player?
    /*
    public void Interact(PlayerInteraction player)
    {
        OnInteractionStarted?.Invoke();

        if (CanInteract(player))
        {
            PerformInteraction(player);
            OnInteractionCompleted?.Invoke();
        }
    }

    public virtual bool CanInteract(PlayerInteraction player) => true;

    protected abstract void PerformInteraction(PlayerInteraction player);

    public virtual void CancelInteraction()
    {
        OnInteractionCancelled?.Invoke();
    }
    */
}