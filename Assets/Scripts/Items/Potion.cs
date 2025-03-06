using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, Item
{
    [SerializeField]
    int healing = 50;
    [SerializeField] Texture icon;
    [SerializeField] string type;
    [SerializeField] string title;
    [SerializeField] string description;

    public int ReturnBonus()
    {
        // Mai s'ha d'entrar aqui
        return 0;
    }

    public string ReturnDescription()
    {
        return description;
    }

    public Texture ReturnIcon()
    {
        return icon;
    }

    public string ReturnTitle()
    {
        return title;
    }

    public string ReturnType()
    {
        return type;
    }

    public void Use(PlayerController player)
    {
        Debug.Log("Healing: " + healing);
        player.Heal(healing);
    }
}
