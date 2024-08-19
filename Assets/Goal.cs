using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    public int requiredSize = 0;
    [SerializeField] AudioClip levelComplete;
    private AudioSource musicManager;

    private void Start()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<AudioSource>();
    }

    public void GoalCheck(int size)
    {
        if (size == requiredSize)
        {
            StartCoroutine("FinishLevel");
        }
    }

    private IEnumerator FinishLevel()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
        musicManager.Pause();
        musicManager.loop = false;
        musicManager.clip = levelComplete;
        musicManager.Play();

        while (musicManager.isPlaying) { yield return null; }

        Destroy(musicManager.gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
