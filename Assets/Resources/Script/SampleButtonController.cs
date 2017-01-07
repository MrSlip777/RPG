using UnityEngine;
using System.Collections;

public class SampleButtonController : BaseButtonController
{
　  BattleStateManager mBattleStateManager = new BattleStateManager();

    protected override void OnClick(string objectName)
    {
        // 渡されたオブジェクト名で処理を分岐
        if (mBattleStateManager.getButtonName
            (MainMenuController.eMainButton.eButton_Attack).Equals(objectName))
        {
            mBattleStateManager.Implement_Button_Attack();
        }
        else if (mBattleStateManager.getButtonName
            (MainMenuController.eMainButton.eButton_Skill).Equals(objectName))
        {
            mBattleStateManager.Implement_Button_Skill();
        }
        else if (mBattleStateManager.getButtonName
            (MainMenuController.eMainButton.eButton_Item).Equals(objectName))
        {
            mBattleStateManager.Implement_Button_Item();
        }
        else if (mBattleStateManager.getButtonName
            (MainMenuController.eMainButton.eButton_Escape).Equals(objectName))
        {
            mBattleStateManager.Implement_Button_Escape();
        }
        else
        {
            throw new System.Exception("Not implemented!!");
        }
    }
}