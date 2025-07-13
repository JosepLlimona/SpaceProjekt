using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerPlanet : MonoBehaviour
{
    private void Awake()
    {
        GameObject.Find("Player").GetComponent<PlayerController>();
    }
}
