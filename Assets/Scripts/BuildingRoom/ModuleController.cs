using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{

    [SerializeField]
    GameObject module;

    GameObject moduleSpawner;

    private void Awake()
    {
        moduleSpawner = GameObject.Find("ModuleSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        mousePos.y = Mathf.RoundToInt(mousePos.y);
        mousePos.x = Mathf.RoundToInt(mousePos.x);
        transform.position = mousePos;

        bool canPlace = moduleSpawner.GetComponent<ModuleSpawner>().checkPlacement(mousePos, module);

        if (canPlace)
        {
            Color green = Color.green;
            green.a = 0.6f;
            GetComponent<SpriteRenderer>().color = green;
        }
        else
        {
            Color red = Color.red;
            red.a = 0.6f;
            GetComponent<SpriteRenderer>().color = red;
        }
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            moduleSpawner.GetComponent<ModuleSpawner>().AddModule(module, mousePos);   
        }

    }
}
