using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour, Item
{
    [SerializeField] Texture icon;
    [SerializeField] string type;
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] int bonus;
    public Texture ReturnIcon()
    {
        return icon;
    }

    public void Use(PlayerController player)
    {
        Debug.Log("Equip");
    }
    public string ReturnDescription()
    {
        return description;
    }
    public string ReturnTitle()
    {
        return title;
    }
    public string ReturnType()
    {
        return type;
    }

    public int ReturnBonus()
    {
        return bonus;
    }
}
