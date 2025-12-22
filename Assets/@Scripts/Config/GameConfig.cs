using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Localization")]
    public Material koreanFontMaterial;
    public Material englishFontMaterial;

    [Header("Game Settings")]
    public int initialGold = 1000;
    public int initialLevel = 1;

    // TODO: Add more game settings as needed
}
