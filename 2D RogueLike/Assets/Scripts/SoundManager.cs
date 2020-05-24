using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource; //audio source that will play sound effects
    public AudioSource musicSource; // audio soure that will play the music
    public static SoundManager instance = null; //call functions from SoundManager

    public float lowpitchRange = 0.95f; //for random pitch
    public float highPitchRange = 1.05f;//for random pitch


    void Awake()
    {
        if (instance == null) //check if there is already an istance of SoundManager
            instance = this; //if not, set to this
        else if (instance != null) //If so, destroy the existing one
            Destroy(gameObject); //Enforce singleton pattern. Only one soundmanager at a time

        DontDestroyOnLoad(gameObject); // dont destroy when reloading a scene
    }

    public void PlaySingle(AudioClip clip) //play sound clips
    {
        efxSource.clip = clip;
        efxSource.Play();

    }

    public void RandomizeSfx(params AudioClip[] clips) //this function will reduce the repetetive sound effects from being too annoying
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowpitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }


}
