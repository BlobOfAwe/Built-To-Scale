using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uimanager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject RestartButton;
    GameObject globalPauseMenuinstance;
    GameObject globalRestartinstance;

    RectTransform pauseMenuTransform;
    RectTransform RestartTransform;
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
            RestartTransform.anchoredPosition = new Vector3(-80, 15, 0);

        }
    }

    private void OnLevelWasLoaded(int level)
    {
       
        if (level > 0) 
        {
            GameObject PausemenuInstance = Instantiate(PauseMenu);
            GameObject restartButtonInstance = Instantiate(RestartButton);
            globalPauseMenuinstance = PausemenuInstance;
            globalRestartinstance = restartButtonInstance;
            pauseMenuTransform = globalPauseMenuinstance.GetComponent<RectTransform>();
            RestartTransform = globalRestartinstance.GetComponent<RectTransform>();
            PausemenuInstance.gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
            restartButtonInstance.gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
            restartButtonInstance.gameObject.SetActive(true);
        }
    }

    
}
