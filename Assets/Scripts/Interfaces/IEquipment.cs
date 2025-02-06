
using System.Collections.Generic;
using UnityEngine;

public interface IEquipment
{
    List<GameObject> AllWeapons { get; }
    GameObject Longbow { get; }
    GameObject Crossbow { get; }
    GameObject Sword { get; }

} // interface IEquipment