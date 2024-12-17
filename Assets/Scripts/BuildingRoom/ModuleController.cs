using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{

    [SerializeField]
    List<GameObject> modules = new List<GameObject>();
    public List<Vector3> ship = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        mousePos.y = Mathf.RoundToInt(mousePos.y);
        mousePos.x = Mathf.RoundToInt(mousePos.x);
        transform.position = mousePos;

        bool canPlace = checkPlacement(mousePos);

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
            ship.Add(Instantiate(modules[0], mousePos, Quaternion.identity).transform.position);
        }

    }

    private bool checkPlacement(Vector3 pos)
    {
        if (ship.Count <= 0)
            return true;
        else
        {
            Vector3 left = new Vector3(pos.x - 1, pos.y, 0);
            Vector3 right = new Vector3(pos.x + 1, pos.y, 0);
            Vector3 up = new Vector3(pos.x, pos.y - 1, 0);
            Vector3 down = new Vector3(pos.x, pos.y + 1, 0);

            return ship.Contains(left) || ship.Contains(right) || ship.Contains(down) || ship.Contains(up);
        }
    }
}
