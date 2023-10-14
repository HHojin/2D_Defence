using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUp : UIBase
{
    public override void Init()
    {
        UIManager.Instance.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
