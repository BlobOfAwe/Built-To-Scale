using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float resizeWhenTimer = -1; // Accessed by the player to indicate how much time into the movement should pass before the token is collected.
    [SerializeField] int tokenType; // an int with a value either 1 or -1. 1 for grow token, -1 for shrink token
    [SerializeField] Sprite[] sprites = new Sprite[3];
    private SpriteRenderer spriteRender;
    private PlayerController player;
    private UIScale uiScale;
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        if (tokenType != 1 && tokenType != -1) { Debug.LogError("Sprite " + this.gameObject + " Token of Invalid Type " + tokenType); }
        spriteRender.sprite = sprites[tokenType + 1]; // Set the token's sprite based on its type.
        player = FindAnyObjectByType<PlayerController>(); // Find the player object
        uiScale = GameObject.FindAnyObjectByType<UIScale>(); // Find the UI Scale
    }

    public IEnumerator Collect()
    {
        yield return new WaitForSeconds(resizeWhenTimer);
        Resize();
    }
    private void Resize()
    { 
        player.size += tokenType; // Change the player's size by either growing by 1 or shrinking by 1
        tokenType *= -1; // Switch the token's type
        spriteRender.sprite = sprites[tokenType + 1]; // Set the token's sprite based on its type.

        // If the player's size exceeds its boundary, kill the player
        if (player.size < -2 || player.size > 2) { player.Die(); }

        uiScale.SetAngle(player.size); // Set the UI Scale's angle to match the player's size
    }
}
