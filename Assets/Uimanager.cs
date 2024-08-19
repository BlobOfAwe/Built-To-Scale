using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uimanager : MonoBehaviour
{
    public GameObject PauseMenu;
   
    GameObject globalPauseMenuinstance;
    GameObject globalRestartinstance;

    RectTransform pauseMenuTransform;

    void Start()
    {
     
    }
    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(this);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            globalPauseMenuinstance.SetActive(true);
            float RightpauseMenuTransform = -pauseMenuTransform.offsetMax.x;
            RightpauseMenuTransform = 0;

            foreach (Transform t in globalPauseMenuinstance.transform)
            {
                t.gameObject.SetActive(true);
            }

        }
    }

    private void OnLevelWasLoaded(int level)
    {
       
        if (level > 0) 
        {
            GameObject PausemenuInstance = Instantiate(PauseMenu);
    
            globalPauseMenuinstance = PausemenuInstance;
          
            pauseMenuTransform = globalPauseMenuinstance.GetComponent<RectTransform>();
           
            PausemenuInstance.gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
           
        }
    }

    
}
