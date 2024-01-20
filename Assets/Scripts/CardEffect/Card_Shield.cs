using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Shield : CardBase, IDestroyable
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void ShieldEnable(bool isEnabled)
    {
        _spriteRenderer.enabled = isEnabled;
        _boxCollider.enabled = isEnabled;
    }

    public void SetColor(Color newColor)
    {
        _spriteRenderer.color = newColor;
    }

    public void DestroyObject()
    {
        CardPlacementController.DestroyCard(EnumDefs.Card.Shield);
        ShieldEnable(false);
    }
}
