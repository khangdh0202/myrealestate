using UnityEngine;

public class UILoad : MonoBehaviour
{
    [SerializeField] private Font font;

    private void Awake()
    {
        // Load Font text
        FontLoader.Init(font);
    }
}
