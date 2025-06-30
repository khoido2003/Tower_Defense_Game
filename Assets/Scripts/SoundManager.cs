using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    public static SoundManager Instance { get; private set; }

    private float volume = .5f;

    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
    }

    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();

        foreach (Sound sound in Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
    }

    public void IncreaseVolume()
    {
        volume += .1f;
        volume = Mathf.Clamp01(volume);
    }

    public void DecreaseVolume()
    {
        volume -= .1f;
        volume = Mathf.Clamp01(volume);
    }

    public float GetVolume ( ) {
        return volume;
    }
}
