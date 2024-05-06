using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment")]
public class EquipmentData : ScriptableObject
{
    public Sprite sprite;
    public string title, description, slot;
    public int[] stats = new int[12];
}
