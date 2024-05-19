
using UnityEngine;

public interface IInputBuilding
{
    public void PlaceItem();

    public void ReSizeItem(float y);

    public void ReRotateItem(float x, float y);

    public void DeleteItem(GameObject obj);
}
