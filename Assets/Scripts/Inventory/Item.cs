using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public int id;
    public string displayName;
    public string internalName;
    public Sprite sprite;
}
