using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FontLoader : MonoBehaviour
{
    public static void Init(Font font)
    {
        IEnumerable<Text> dataPersistanceObjects = FindObjectsOfType<Text>()
            .OfType<Text>();

        List<Text> textMeshProUGUIs = new List<Text>(dataPersistanceObjects);
        
        foreach (Text textMesh in textMeshProUGUIs)
        {
            textMesh.font = font;
        }
    }
}
