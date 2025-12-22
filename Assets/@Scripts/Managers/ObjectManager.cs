using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    #region Roots
    private Transform _playerRoot;
    public Transform PlayerRoot
    {
        get { return Utils.GetRootTransform(ref _playerRoot, "@Players"); }
    }

    private Transform _monsterRoot;
    public Transform MonsterRoot
    {
        get { return Utils.GetRootTransform(ref _monsterRoot, "@Monsters"); }
    }

    private Transform _npcRoot;
    public Transform NpcRoot
    {
        get { return Utils.GetRootTransform(ref _npcRoot, "@Npcs"); }
    }
    #endregion

    private HashSet<GameObject> _objects = new HashSet<GameObject>();
    private HashSet<Player> _players = new HashSet<Player>(); 
    private HashSet<Monster> _monsters = new HashSet<Monster>();    
    private HashSet<Npc> _npcs = new HashSet<Npc>();
   
    public Player SpawnPlayer(string prefab = "Player")
    {
        GameObject go = ResourceManager.Instance.Instantiate(prefab);
        go.name = prefab;
        go.transform.parent = PlayerRoot;

        Player player = go.AddComponent<Player>();
        _objects.Add(go);
        _players.Add(player);

        return player;
    }

    public Monster SpawnMonster(string prefab = "Monster")
    {
        GameObject go = ResourceManager.Instance.Instantiate(prefab);
        go.name = prefab;
        go.transform.parent = MonsterRoot;

        Monster monster = go.AddComponent<Monster>();
        _objects.Add(go);
        _monsters.Add(monster);

        return monster;
    }

    public Npc SpawnNpc(string prefab = "Npc")
    {
        GameObject go = ResourceManager.Instance.Instantiate(prefab);
        go.name = prefab;
        go.transform.parent = NpcRoot;

        Npc npc = go.AddComponent<Npc>();
        _objects.Add(go);
        _npcs.Add(npc);

        return npc;
    }


    public void Despawn(GameObject go)
    {
        if (_objects.Remove(go) == false)
            return;

        Player player = go.GetComponent<Player>();
        if (player != null)
        {
            _players.Remove(player);
            GameObject.Destroy(go);
            return;
        }

        Monster monster = go.GetComponent<Monster>();
        if (monster != null)
        {
            _monsters.Remove(monster);
            GameObject.Destroy(go);
            return;
        }

        Npc npc = go.GetComponent<Npc>();
        if (npc != null)
        {
            _npcs.Remove(npc);
            GameObject.Destroy(go);
            return;
        }

        GameObject.Destroy(go);
    }

}
