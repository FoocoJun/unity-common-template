using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_GameScene : UI_Scene
{
    enum Texts
    {
        CurrentFPSText,
    }
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindTexts(typeof(Texts));
        
        return true;
    }
    
    private float _elapsedTime = 0.0f;
    private readonly float _updateInterval = 1.0f;

    private void Update()
    {
        UpdateCurrentFPS();
    }

    void UpdateCurrentFPS()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _updateInterval)
        {
            float fps = 1.0f / Time.deltaTime;
            float ms = Time.deltaTime * 1000.0f;
            string text = $"{fps:N1} FPS ({ms:N1}ms)";
            GetText((int)Texts.CurrentFPSText).text = text;

            _elapsedTime = 0;
        }
    }
}