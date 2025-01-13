using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;
        
        // 이하 씬이 시작 할 때 원하는 셋팅

        return true;
    }

    public override void Clear()
    {

    }
}