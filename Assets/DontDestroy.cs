using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(gameObject.tag);

        if (objs.Length > 1)
        {
            if (gameObject.CompareTag("Music"))
            {
                foreach (var obj in objs)
                {
                    if (obj != this.gameObject && obj.GetComponent<AudioSource>().clip != GetComponent<AudioSource>().clip)
                    {
                        obj.GetComponent<AudioSource>().clip = GetComponent<AudioSource>().clip;
                        obj.GetComponent<AudioSource>().Play();
                    }
                }
            }
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
