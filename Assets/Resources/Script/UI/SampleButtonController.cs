using UnityEngine;
using System.Collections;

public class SampleButtonController : BaseButtonController
{
    BattleSelectState mInstance = BattleSelectState.Instance;

    protected override void OnClick(string objectName)
    {
        // 渡されたオブジェクト名で処理を分岐
        if (mInstance.getButtonName
            (MainMenuController.eMainButton.eButton_Attack).Equals(objectName))
        {
            mInstance.Implement_Button_Attack();
        }
        else if (mInstance.getButtonName
            (MainMenuController.eMainButton.eButton_Skill).Equals(objectName))
        {
            mInstance.Implement_Button_Skill();
        }
        else if (mInstance.getButtonName
            (MainMenuController.eMainButton.eButton_Item).Equals(objectName))
        {
            mInstance.Implement_Button_Item();
        }
        else if (mInstance.getButtonName
            (MainMenuController.eMainButton.eButton_Escape).Equals(objectName))
        {
            mInstance.Implement_Button_Escape();
        }
        else
        {
            //サブメニュー内のボタン処理
            string[] splitName = objectName.Split('_');

            if (splitName[0] == "contentNo")
            {
                mInstance.Implement_Button_Content(int.Parse(splitName[1]));
            }
            else {
                throw new System.Exception("Not implemented!!");
            }
        }
    }
}