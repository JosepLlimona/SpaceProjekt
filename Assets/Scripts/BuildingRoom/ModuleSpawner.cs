using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> modulesMov = new List<GameObject>();
    List<Vector3> shipPosition = new List<Vector3>();
    List<GameObject> ship = new List<GameObject>();

    GameObject currentModule;

    public void spawnModule(int i)
    {
        if (currentModule != null)
        {
            Destroy(currentModule);
        }
        currentModule = Instantiate(modulesMov[i], Vector3.zero, Quaternion.identity);
    }

    public void AddModule(GameObject module, Vector3 mousePos)
    {
        shipPosition.Add(Instantiate(module, mousePos, Quaternion.identity).transform.position);
        ship.Add(Instantiate(module, mousePos, Quaternion.identity));
    }

    public bool checkPlacement(Vector3 pos, GameObject module)
    {
        if (ship.Count <= 0)
            return true;
        else
        {
            Vector3 left = new Vector3(pos.x - 1, pos.y, 0);
            Vector3 right = new Vector3(pos.x + 1, pos.y, 0);
            Vector3 up = new Vector3(pos.x, pos.y - 1, 0);
            Vector3 down = new Vector3(pos.x, pos.y + 1, 0);

            int idx = shipPosition.IndexOf(left);
            if(idx == -1)
            {
                idx = shipPosition.IndexOf(right);
            }
            if (idx == -1)
            {
                idx = shipPosition.IndexOf(up);
            }
            if (idx == -1)
            {
                idx = shipPosition.IndexOf(down);
            }

            if (idx != -1)
            {
                Debug.Log(idx);
                return ship[idx].GetComponent<Module>().checkPlacment(module.GetComponent<Module>());
            }
            return false;
        }
    }
}
