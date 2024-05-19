using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesSoundEffect : MonoBehaviour
{
    public AudioClip wavesSoundClip; // Clip âm thanh muốn phát
    private AudioSource audioSource;
    public float maxDistance = 10f / Settings.ratioWorld; // Khoảng cách tối đa âm thanh phát được

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = wavesSoundClip;
        audioSource.volume = Settings.volumeSFX * Settings.volumeMaster;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if(Camera.main != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Camera.main.transform.position);

            // Nếu khoảng cách đến player nhỏ hơn hoặc bằng maxDistance
            if (distanceToPlayer <= maxDistance)
            {
                // Tính toán âm lượng dựa trên khoảng cách
                float volume = Settings.volumeSFX - (distanceToPlayer / maxDistance);
                audioSource.volume = volume * Settings.volumeMaster;

                // Nếu âm thanh chưa được phát, phát âm thanh
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                // Nếu khoảng cách lớn hơn maxDistance và âm thanh đang phát, dừng phát âm thanh
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }
}
