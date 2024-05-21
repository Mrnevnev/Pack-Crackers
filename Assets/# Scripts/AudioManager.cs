using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


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
        else  if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public AudioSource menuMusic;
    public AudioSource battleSelectMusic;
    public AudioSource[] bgm;
    private int currentBGM;
    private bool playingBGM;

    public AudioSource[] sfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playingBGM)
        {
            if (bgm[currentBGM].isPlaying == false)
            {
                currentBGM++;
                if (currentBGM >= bgm.Length)
                {
                    currentBGM = 0;
                }
                bgm[currentBGM].Play();
            }
        }
    }

    public void StopMuisc()
    {
        menuMusic.Stop();
        battleSelectMusic.Stop();
        foreach (AudioSource track in bgm)
        {
            track.Stop();
        }

        playingBGM = false;
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

        currentBGM = UnityEngine.Random.Range(0, bgm.Length);
        
        bgm[currentBGM].Play();
        playingBGM = true;
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}
