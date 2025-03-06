using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{ 
    public void Use(PlayerController player);
    public Texture ReturnIcon();

    public string ReturnTitle();
    public string ReturnDescription();
    public int ReturnBonus();
    public string ReturnType();
}
