using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_TitleScene : UI_Scene
{
    enum GameObjects
    {
        StartImage
    }

    enum Texts
    {
        DisplayText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));

        GetObject((int)GameObjects.StartImage).BindEvent((evt) =>
        {
            Debug.Log("ChangeScene");
            Managers.Scene.LoadScene(EScene.GameScene);
        });

        GetObject((int)GameObjects.StartImage).gameObject.SetActive(false);
        GetText((int)Texts.DisplayText).text = $"";
        
        StartLoadAssets();

        return true;
    }
    
    /**
     * 진입점인 TitleScene에서는 LoadAllAsync를 호출하기 전이라
     * UIManager의 ShowSceneUI(Addressable)를 수행할 수 없으므로
     * LoadAllAsync를 호출하기 위해 최소한의 하이어라키 구현은 해두어야 함.
     */
    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.Data.Init();
                
                // 세이브 데이터 불러오기 등

                GetObject((int)GameObjects.StartImage).gameObject.SetActive(true);
                GetText((int)Texts.DisplayText).text = "Touch To Start";
            }
        });
    }
}