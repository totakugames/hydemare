/*
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupInteractable : Interactable
{
    [Header("Item")]
    [SerializeField] private ItemDataSO itemData;

    public ItemDataSO ItemData => itemData;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;

        promptText = "[E] Pick Up";
    }
    public override bool CanInteract(PlayerController player)
    {
        return itemData != null;
    }

    protected override void PerformInteraction(PlayerController player)
    {
        //TODO :) Add item to player's inventory
    }

    public void Drop(Vector2 position)
    {
        transform.position = position;
        SetVisible(true);
    }

    public void SetVisible(bool visible)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = visible;
        if (collider != null)
            collider.enabled = visible;

        gameObject.SetActive(visible);
    }
}

*/