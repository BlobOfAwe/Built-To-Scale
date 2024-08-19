using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    public int requiredSize = 0;
    [SerializeField] Sprite[] activeGoal = new Sprite[5];
    [SerializeField] Sprite[] inactiveGoal = new Sprite[5];
    [SerializeField] AudioClip levelComplete;
    private AudioSource musicManager;
    private PlayerController player;
    private SpriteRenderer spriteRender;

    private void Start()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        player = FindAnyObjectByType<PlayerController>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    public void GoalCheck(int size)
    {
        if (size == requiredSize)
        {
            StartCoroutine("FinishLevel");
        }
    }

    private void Update()
    {
        if (player.size == requiredSize) { spriteRender.sprite = activeGoal[requiredSize + 2]; }
        else { spriteRender.sprite = inactiveGoal[requiredSize + 2]; }
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
