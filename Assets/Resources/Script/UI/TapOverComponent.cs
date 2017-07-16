using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TapOverComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    SubMenuController mSubMenuController = SubMenuController.Instance;

    public Button btContent { get { return GetComponent<Button>(); } }

    // オブジェクトの範囲内にマウスポインタが入った際に呼び出されます。
    // this method called by mouse-pointer enter the object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        mSubMenuController.ChangeDescription(btContent.name);
    }

    // オブジェクトの範囲内からマウスポインタが出た際に呼び出されます。
    // 
    public void OnPointerExit(PointerEventData eventData)
    {
       
    }
}