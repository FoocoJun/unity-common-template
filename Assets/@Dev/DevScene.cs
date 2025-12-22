using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DevScene : BaseScene
{
	protected override void Awake()
	{
		base.Awake();

		SceneType = Define.EScene.DevScene;

        ResourceManager.Instance.LoadAll();
        DataManager.Instance.LoadData();

        // Cache
        // UIManager.Instance.ShowPopupUI<UI_Popup_Item>();

        AdsManager.Instance.Init();
        IAPManager.Instance.Init();

        StartCoroutine(CoTest());
    }

    IEnumerator CoTest()
    {
        yield return new WaitForSeconds(2);

        IAPManager.Instance.Purchase("rookiss_s2_noads", () =>
        {
            Debug.Log("Purchase Completed!");
        });

        //AdsManager.Instance.ShowInterstitialAds();
        //AdsManager.Instance.ShowRewardedAds(() =>
        //{
        //    Debug.Log("Rewarded Ad Completed!");
        //});

    }

    private void OnEnable()
    {
        EventManager.Instance.AddEvent(Define.EEventType.GoldChanged, OnGoldChanged);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveEvent(Define.EEventType.GoldChanged, OnGoldChanged);
    }

    void OnGoldChanged()
    {
        Debug.Log($"Gold Changed!");
    }
}
