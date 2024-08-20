using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeWall : MonoBehaviour
{
    public int requiredSize = 0;
    [SerializeField] Sprite[] passable = new Sprite[5];
    [SerializeField] Sprite[] impassable = new Sprite[5];
    private PlayerController player;
    private SpriteRenderer spriteRender;
    private Collider2D collider;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        collider = GetComponent<BoxCollider2D>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    public void SizeWallCheck(int size)
    {
        if (size == requiredSize)
        {
            StartCoroutine("FinishLevel");
        }
    }

    private void Update()
    {
        if (player.size == requiredSize) 
        { 
            //spriteRender.sprite = passable[requiredSize + 2]; 
            collider.enabled = false;
            spriteRender.color = Color.green;
        }
        else 
        {
            //spriteRender.sprite = impassable[requiredSize + 2]; 
            spriteRender.color = Color.red;
            collider.enabled = true; 
        }
    }
}
