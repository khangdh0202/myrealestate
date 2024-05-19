using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AudioManager : SingletonMonobehaviours<AudioManager>
{
    public AudioClip backgroundAudioClip;
    private AudioSource audioSource;

/*
    [Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid) { }
    }*/

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        PlayBackgroundMusnic(backgroundAudioClip);

    }
    private void FixedUpdate()
    {
        audioSource.volume = Settings.volumeBGM * Settings.volumeMaster;
    }

    /* private void LoadAudioFromGroup()
     {
         // Tải tất cả các tài nguyên từ nhóm audioGroup
         AsyncOperationHandle<IList<AudioClip>> handle = Addressables.LoadAssetsAsync<AudioClip>(Settings.groupAudioName, null);
         // Gắn sự kiện Completed để xử lý khi tải hoàn tất
         handle.Completed += (operation) =>
         {
             if (operation.Status == AsyncOperationStatus.Succeeded)
             {
                 // Xử lý các AudioClip được tải ở đây
                 foreach (AudioClip clip in operation.Result)
                 {
                     AudioClips.Add(clip);

                     // Do something with the audio clip
                     if (clip.name == Settings.NameBGM)
                     {
                         PlaySound(clip);
                     }
                     if(clip.name == Settings.hammerBuildingEffect)
                     {
                         PlacementSystem.Instance.AssignBuildingEffect(clip);
                     }
                     if(clip.name == Settings.windsEffect)
                     {

                     }
                     if (clip.name == Settings.wavesEffect)
                     {

                     }
                 }
             }
             else
             {
                 // Xử lý trường hợp tải không thành công
                 Debug.LogError("Failed to load audio from group: " + Settings.groupAudioName);
             }
         };
     }*/

    void PlayBackgroundMusnic(AudioClip soundClip)
    {
        // Kiểm tra nếu AudioClip không null và AudioSource không null
        if (soundClip != null && audioSource != null)
        {
            // Gán AudioClip cho AudioSource và phát âm thanh
            audioSource.clip = soundClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Missing AudioClip or AudioSource component!");
        }
    }
}
