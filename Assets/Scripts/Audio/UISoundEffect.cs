using UnityEngine;
using UnityEngine.UI;

public class UISoundEffect : MonoBehaviour
{
    public AudioClip clickSound; // Âm thanh khi nút được nhấn
    private Button[] buttons; // Mảng chứa tất cả các nút

    void Start()
    {
        buttons = FindObjectsOfType<Button>(true); // Tìm tất cả các nút trong Scene

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => PlayClickSound()); // Gán hàm PlayClickSound cho sự kiện click của mỗi nút
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, Settings.volumeMaster * Settings.volumeSFX); // Phát âm thanh tại vị trí của Camera
        }
    }
}
