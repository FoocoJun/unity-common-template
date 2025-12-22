using UnityEngine;

public static class Define
{
    public enum EScene
    {
        Unknown,
        LoadingScene,
        DevScene,
    }

    public enum EEventType
    {
        None,
        GoldChanged,
        LanguageChanged,
    }

    public enum ELanguage
    {
        KOR,
        ENG,
    }

    public enum EObjectType
    {
        None,
        Creature,
        Projectile,
        Env
    }

    public enum ECreatureType
    {
        None,
        Player,
        Monster,
        Npc
    }

    public enum ESound
    {
        Bgm,
        Effect,

        Max,
    }
}
