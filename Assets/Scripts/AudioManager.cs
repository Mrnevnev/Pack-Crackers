using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioSource menuMusic;
    public AudioSource battleSelectMusic;
    public AudioSource[] bgm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopMuisc()
    {
        menuMusic.Stop();
        battleSelectMusic.Stop();
        foreach (AudioSource track in bgm)
        {
            track.Stop();
        }
    }

    public void PlayMenuMusic()
    {
        StopMuisc();
        menuMusic.Play();
    }

    public void PlayBattleSelectMusic()
    {
        if (battleSelectMusic.isPlaying == false)
        {
            StopMuisc();
            battleSelectMusic.Play();
        }
    }

    public void PlayBGM()
    {
        StopMuisc();
    }
}
