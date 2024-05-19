using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSound : MonoBehaviour
{
    public AudioClip buildingSoundClip; // Clip âm thanh muốn phát
    private AudioSource audioSource;
    public float maxDistance = 10f; // Khoảng cách tối đa âm thanh phát được

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = buildingSoundClip;
        audioSource.volume = Settings.volumeSFX;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
                Debug.Log("sound effect correct");
                // Nếu âm thanh chưa được phát, phát âm thanh
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
    }
}
