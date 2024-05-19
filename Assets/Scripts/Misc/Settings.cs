using Antlr4.Runtime.Dfa;
using UnityEngine;

public static class Settings
{
    // MainMenu
    public static bool isMainMenuActive = false;

    // Sound
    public static string groupAudioName = "audio";
    public static float volumeMaster = 1.0f;
    public static float volumeSFX = 1f;
    public static float volumeBGM = 0.3f;
    public static string NameBGM = "BGM";
    [Tooltip("Âm thanh tiếng búa")] public static string hammerBuildingEffect = "HammerBuildingEffect";
    [Tooltip("Âm thanh tiếng sóng")] public static string wavesEffect = "WavesEffect";
    [Tooltip("Âm thanh tiếng gió")] public static string windsEffect = "WindsEffect";

    // Road 
    [Tooltip("Độ cao thấp của đường cho mesh không đè nhau")] public static float meshOnMesh = 0.01f;
    [Tooltip("Tỉ lệ của bản đồ so với thực tế")] public static float ratioWorld = 10f;
    public static string straightTypeRoad = "StraightRoad";
    public static string curveTypeRoad = "CurveRoad";
    public static bool RoadSnapToRoad = false;
    public static bool buildingSnapToRoad = false;
    public static bool EndRoadExit = false;
    public static GameObject parentOfSnapToSegment = null;
    public static string snapToFirstOrLastSegment = "";
    public static Vector3 positionRoadSnapToRoad = Vector3.zero;
    public static bool roadSnaptoRoad = false;
    //public static Transform directionSnapingBuilding;

    // Snap
    [Tooltip("Name of snap area")] public static string snapAreaName = "SnapArea";
    [Tooltip("Snap tag name")] public static string snapTag = "SnapCollison";
    [Tooltip("Tên thằng Grid chính")] public static string gridName = "GridCollsion";
    [Tooltip("Tên đối tượng quản lí cách cạnh Snap box ")] public static string snapParrent = "Snap";
    [Tooltip("Kiểm tra đang bị snap")] public static bool isSnaping = false;
    [Tooltip("Grid đang snap nhưng chưa xác nhận đặt ")] public static bool canSnap = true;

    // Snap tự động snap tới điểm của building khi đặt khoản cách cụ thể
    public static bool _canSnapFromPointToPointBottomOfBuilding = true;
    public static int _directionSnaptoPointBottomOfBuilding =0;
    public static bool _snapToPointBottomOfBuilding = false;
    public static float _distanceFromPointToPointBottomOfBuilding = float.MaxValue;
    public static Vector3 distancePointToPoint= Vector3.zero;
    [Tooltip("Có đang snap tới conner của building in map không?")] public static bool _snapToConner = false;

    // InventoryManager
    public static bool isOpenInventory = false; // Trạng thái của inventory

    // Edit mode Menu
    public static bool isOpenEditGrid = false; // Trạng thái của Edit Grid menu
    public static bool evenNumberXScale = false; // Kiểm tra xem scale là số chẵn hay lẽ đẻ đặt grid đúng vị trí
    public static bool evenNumberYScale = false;

    // InputManager
    public static LayerMask placementLayerMask;

    // Building
    //[Tooltip("LayerMask of building")] public static LayerMask buildingLayerMask = LayerMask.NameToLayer("Building");
    public static bool canBuild = true; // Có thể đặt building hay không 
    public static bool onGrid = false; // Công trình có nằm trong phạm vi grid hay không
    public const float scaleSpeed = 2f; // Tốc độ thay đổi tỷ lệ item
    public const float minScale = 0.1f;   // Tỷ lệ nhỏ nhất item
    public const float maxScale = 100f;   // Tỷ lệ lớn nhất item
    public const float rotationSpeed = 45.0f; // Tốc độ xoay item
    public static bool inScale = false; // Bật tắt scale
    public static Vector3 scaleOriginBuild; // Origin scale 
    [Tooltip("Collision ra khỏi construction")] public static bool EndConstructionExit;
    [Tooltip("Construction có đang nằm trong một construction khác")] public static bool insideConstructionOther = false;

    public static ItemType typeOfCurrentBulld = ItemType.Build; 
    // Button trigger on action event
    public static bool triggerButtonLeftController = false;
    public static bool triggerButtonRightController = true;
    public static Vector3 currentPositionRay;

    // Tags
    public static string roadTag = "Road";
    public static string contructionTag = "Building";

}
