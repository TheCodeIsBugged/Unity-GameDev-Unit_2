using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicSource;
    public AudioClip playingMusic;
    public AudioClip winMusic;

    public GameObject winText;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = playingMusic;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (winText.activeInHierarchy && winText.GetComponent<TextMeshProUGUI>().text == "YOU WIN!")
        {
            if (musicSource.clip == playingMusic)
            {
                musicSource.clip = winMusic;
                musicSource.Play();
            }
        }
    }
}
