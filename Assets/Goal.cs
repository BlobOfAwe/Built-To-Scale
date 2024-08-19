using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    public int requiredSize = 0;
    [SerializeField] AudioClip levelComplete;
    private AudioSource audioManager;

    private void Start()
    {
        audioManager = GameObject.FindAnyObjectByType<AudioSource>();
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
        audioManager.Pause();
        audioManager.clip = levelComplete;
        audioManager.Play();

        while (audioManager.isPlaying) { yield return null; }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
