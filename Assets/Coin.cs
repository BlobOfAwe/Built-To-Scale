using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Vector3 SetPlayerScaleOnCollison; // the scale set when collidng with player.just this once then make a prefab of the coin.
    // make sure coins have a rigibody with frozen constraints & collider that istrigger
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Vector3 NewScale = SetPlayerScaleOnCollison;
            collision.transform.localScale = NewScale;
        }
    }
}
