using UnityEngine;

public class RoadSpline : MonoBehaviour, IDataPersistantce
{
    [SerializeField] private string id;

    public string Id { get => id; set => id = value; }

    private void Awake()
    {
        GenerateGuid();
    }

    private void GenerateGuid()
    {
        this.Id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        //throw new System.NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        if(data.roadDictionary.ContainsKey(Id))
        {
            data.roadDictionary.Remove(Id);
        }

        data.roadDictionary.Add(Id, new RoadData(this.gameObject));
    }
}
