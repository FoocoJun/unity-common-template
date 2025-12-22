using UnityEngine;
using static Define;

public class Creature : ObjectBase
{
    protected ECreatureType _creatureType;

    public Creature()
    {
        _objectType = EObjectType.Creature;
    }
}
