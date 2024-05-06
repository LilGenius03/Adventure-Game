using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemData : ScriptableObject
{
    public Sprite sprite;
    public string title, description, target, effect, type;
    public int potency;
}
