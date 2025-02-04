using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAudioManager : MonoBehaviour
{
    [Header("Entity Audio Clips")]
    [SerializeField] public AudioClip[] footstepClips;
    [SerializeField] public AudioClip[] jumpClips;
    [SerializeField] public AudioClip[] landClips;
    [SerializeField] public AudioClip[] attackClips;
    [SerializeField] public AudioClip[] damageClips;
    [SerializeField] public AudioClip[] collectClips;
    [SerializeField] public AudioClip[] deathClips;

    [Header("Gun Audio Clips")]
    [SerializeField] public AudioClip[] gunShotClips;
    [SerializeField] public AudioClip[] gunReloadClips;
    [SerializeField] public AudioClip[] gunEmptyClips;
    [SerializeField] public AudioClip[] stopClips;
    [SerializeField] public AudioClip[] gunIdleClips;

    [Header("Pitch")]
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    [Header("Gun Sound Pool")]
    [SerializeField] private int gunAudioPoolSize = 10; // Number of simultaneous gunshot sounds

    private AudioSource mainAudioSource;
    private AudioSource[] gunAudioSources;
    private int gunAudioIndex = 0;

    private void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();

        // Initialize gunshot audio source pool
        gunAudioSources = new AudioSource[gunAudioPoolSize];
        for (int i = 0; i < gunAudioPoolSize; i++)
        {
            GameObject audioObject = new GameObject("GunshotAudioSource_" + i);
            audioObject.transform.SetParent(transform);
            gunAudioSources[i] = audioObject.AddComponent<AudioSource>();
            gunAudioSources[i].playOnAwake = false;
        }
    }

    void RandomizePitch(AudioSource source)
    {
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
    }

    public void PlayCollectSound()
    {
        RandomizePitch(mainAudioSource);
        mainAudioSource.PlayOneShot(collectClips[Random.Range(0, collectClips.Length)]);
    }

    public void PlaySound(AudioClip[] clips)
    {
        if (clips.Length != 0)
        {
            mainAudioSource.Stop();
            mainAudioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            RandomizePitch(mainAudioSource);
        }
    }

    public void PlayFootsteps()
    {
        if (mainAudioSource.isPlaying)
        {
            return;
        }
        mainAudioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
    }

    public void PlayHitmarkers(AudioClip[] clips)
    {
        if (mainAudioSource.isPlaying)
        {
            return;
        }
        mainAudioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        RandomizePitch(mainAudioSource);
    }

    public void PlayUniqueSound(AudioClip clip)
    {
        mainAudioSource.PlayOneShot(clip);
    }

    public void PlayGunshot()
    {
        if (gunShotClips.Length == 0) return;

        // Get the next available gun AudioSource
        AudioSource source = gunAudioSources[gunAudioIndex];
        source.clip = gunShotClips[Random.Range(0, gunShotClips.Length)];
        
        RandomizePitch(source);
        source.Play();

        // Move to the next AudioSource in the pool
        gunAudioIndex = (gunAudioIndex + 1) % gunAudioPoolSize;
    }

    public void getListOfClips()
    {
        Debug.Log("Footstep Clips: " + footstepClips.Length);
        Debug.Log("Attack Clips: " + attackClips.Length);
        Debug.Log("Damage Clips: " + damageClips.Length);
        Debug.Log("Collect Clips: " + collectClips.Length);
        Debug.Log("Death Clips: " + deathClips.Length);
        Debug.Log("Gun Shot Clips: " + gunShotClips.Length);
        Debug.Log("Gun Reload Clips: " + gunReloadClips.Length);
        Debug.Log("Gun Empty Clips: " + gunEmptyClips.Length);
        Debug.Log("Stop Clips: " + stopClips.Length);
    }
}
