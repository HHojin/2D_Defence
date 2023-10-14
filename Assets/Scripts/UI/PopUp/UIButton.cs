using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButton : UIPopUp
{
    enum ButtonType
    {
        Structure,
        Weapons,
        Orbital,
    }

    enum TextType
    {
        Structure,
        Weapons,
        Orbital,
    }

    enum ImageType
    {
        Structure,
        Weapons,
        Orbital,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(ButtonType));
        Bind<Text>(typeof(TextType));
        Bind<GameObject>(typeof(GameObject));
        Bind<Image>(typeof(ImageType));

        GetButton((int)ButtonType.Structure).gameObject.BindEvent(OnButtonClicked);

        GameObject go = GetImage((int)ImageType.Structure).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Click);
    }

    public void OnButtonClicked(PointerEventData data)
    {
        
    }
}
