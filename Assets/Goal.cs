using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    public bool EndBig;
    public bool EndSmall;
    public Vector3 BigScale;
    public Vector3 SmallScale;
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
            }
        }
    }
}
