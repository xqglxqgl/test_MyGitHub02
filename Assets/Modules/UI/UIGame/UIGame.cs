using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : UIBase
{
    public override void OnShow()
    {
        var player = UnitManager.Instance.CreatePlayer(AssetPathUtility.UnitView_Archer);

        CameraManager.Instance.Target = player.transform;
    }
}
