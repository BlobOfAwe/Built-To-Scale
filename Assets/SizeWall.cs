using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeWall : MonoBehaviour
{
    public int requiredSize = 0;
    [SerializeField] Sprite passable;
    [SerializeField] Sprite impassable;
    [SerializeField] Sprite[] active = new Sprite[5];
    [SerializeField] Sprite[] inactive = new Sprite[5];

    private PlayerController player;
    private SpriteRenderer spriteRender;
    [SerializeField] SpriteRenderer numberRenderer;
    private Collider2D collider;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        collider = GetComponent<BoxCollider2D>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player.size == requiredSize) 
        { 
            spriteRender.sprite = passable;
            numberRenderer.sprite = active[requiredSize + 2];
            collider.enabled = false;
        }
        else 
        {
            spriteRender.sprite = impassable;
            numberRenderer.sprite = inactive[requiredSize + 2];
            collider.enabled = true; 
        }
    }
}
