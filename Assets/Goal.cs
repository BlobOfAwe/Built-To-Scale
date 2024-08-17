using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    public bool EndBig;
    public bool EndSmall;
    public Vector3 BigScale; // set this to the scale that the player will transform to when hitting the big coins
    public Vector3 SmallScale; // set this to the scale that the player will transform to when hitting the small coins
    // make sure this obj has a collider that is trigger & has a collider with frzen constraints
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
            if (EndBig && collision.transform.lossyScale == BigScale)
            {
                Debug.Log("Won Big");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (EndSmall && collision.transform.lossyScale == SmallScale)
            {
                Debug.Log("Won Small");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
