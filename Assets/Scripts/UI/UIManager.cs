using UnityEngine;

public class UIManager : SingletonMonobehaviours<UIManager>
{
    public GameObject menuUI;
    protected override void Awake()
    {
        base.Awake();
        menuUI = this.transform.Find("Canvas VR Menu").gameObject;
    }
}
