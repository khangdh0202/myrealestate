using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonMonobehaviours<GameController>
{

    [Header("Hệ thống xây dựng")]
    public PlacementSystem placementSystem ;
    [Header("Player Controller")]
    public Player playerController;
    public DeleteItem deleteItem;
    public RoadController roadController;

    private void Start()
    {
        placementSystem = PlacementSystem.Instance;
        playerController = Player.Instance;
        deleteItem = placementSystem.GetComponent<DeleteItem>();

        // Player Events
        playerController.ItemCancelSnap += placementSystem.CancelSnap;
        playerController.ItemDeleteInMap += deleteItem.ActivateDelete;
        playerController.ItemInLeftHand += placementSystem.ItemInLeftHand;
        playerController.ItemInRightHand += placementSystem.ItemInRightHand;
        playerController.PutBuildingInMap += placementSystem.PutBuildingInMap;

        // Contruction Events
        placementSystem.placeContruction += ContructionController.Instance.ContructionBuild.PlaceItem;
        playerController.ItemScale += ContructionController.Instance.ContructionBuild.ReSizeItem;
        playerController.ItemRotation += ContructionController.Instance.ContructionBuild.ReRotateItem;
        placementSystem.deleteContruction += ContructionController.Instance.ContructionBuild.DeleteItem;

        // Road Events
        placementSystem.placeRoad += RoadController.Instance.RoadBuild.PlaceItem;
        playerController.StopSetPointRoad += RoadController.Instance.StopCreateStartPointRoad;
        placementSystem.deleteRoad += RoadController.Instance.RoadBuild.DeleteItem;

        // Menu
        playerController.MenuUI += UISystem.Instance.UIActive;
    }
}
