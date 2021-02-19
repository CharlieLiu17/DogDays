using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Item", order = 1)]
public class Item : ScriptableObject
{
    public int id;
    public string displayName;
    public string internalName;
    public string description;
    public Sprite sprite;
}
