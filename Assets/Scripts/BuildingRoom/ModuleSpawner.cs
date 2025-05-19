using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> modules = new List<GameObject>();
    [SerializeField] GameObject moduleMov;
    List<Vector3> shipPosition = new List<Vector3>();
    List<GameObject> ship = new List<GameObject>();
    List<GameObject> shipModules = new List<GameObject>();
    public List<Vector3> shipPositionTmp = new List<Vector3>();
    public List<GameObject> shipTmp = new List<GameObject>();

    public GameObject currentModule;

    public GameController gameController;

    private void Start()
    {
        if (gameController == null)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }

        /*shipTmp = gameController.getShip();
        shipPositionTmp = gameController.getShipPos();

        if (shipTmp.Count > 0)
        {
            ship.Clear();
            shipPosition.Clear();
            for (int i = 0; i < ship.Count; i++)
            {
                GameObject moduleTmp = Instantiate(shipTmp[i], shipPositionTmp[i], Quaternion.identity);
                shipPosition.Add(moduleTmp.transform.position);
                ship.Add(moduleTmp);
                gameController.AddModule(moduleTmp, moduleTmp.transform.position);
            }
        }*/
    }

    public void DeleteShip()
    {
        for (int i = 0; i < ship.Count; i++)
        {
            Destroy(shipModules[i]);
        }

        ship.Clear();
        shipPosition.Clear();
        shipModules.Clear();
    }

    public void spawnShip()
    {
        if (ship.Count > 0)
            return;
        shipTmp = gameController.getShip();
        shipPositionTmp = gameController.getShipPos();

        Debug.Log("Rebo: " + shipTmp.Count);

        if (shipTmp.Count > 0)
        {
            Debug.Log("Entro");
            ship.Clear();
            shipPosition.Clear();
            shipModules.Clear();
            for (int i = 0; i < shipTmp.Count; i++)
            {
                GameObject moduleTmp = Instantiate(shipTmp[i], shipPositionTmp[i], Quaternion.identity);
                shipPosition.Add(shipPositionTmp[i]);
                ship.Add(shipTmp[i]);
                shipModules.Add(moduleTmp);
                //gameController.AddModule(moduleTmp, moduleTmp.transform.position);
            }
        }
        /*shipTmp.Clear();
        shipPositionTmp.Clear();*/
    }

    public void spawnModule(int i)
    {
        if (currentModule != null)
        {
            currentModule.GetComponent<ModuleController>().module = modules[i];
        }
        else
        {
            currentModule = Instantiate(moduleMov, Vector3.zero, Quaternion.identity);
            currentModule.GetComponent<ModuleController>().module = modules[i];
        }
    }

    public void AddModule(GameObject module, Vector3 mousePos)
    {
        GameObject moduleTmp = Instantiate(module, mousePos, Quaternion.identity);
        shipPosition.Add(mousePos);
        ship.Add(module);
        shipModules.Add(moduleTmp);
    }

    public void SaveShip()
    {
        gameController.ClearList();
        for(int i = 0; i<ship.Count; i++)
        {
            gameController.AddModule(ship[i], shipPosition[i]);
        }
    }

    public void StopAdding()
    {
        Destroy(currentModule);
        currentModule = null;
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
            string position = "left";
            if (idx == -1)
            {
                idx = shipPosition.IndexOf(right);
                position = "right";
            }
            if (idx == -1)
            {
                idx = shipPosition.IndexOf(up);
                position = "up";
            }
            if (idx == -1)
            {
                idx = shipPosition.IndexOf(down);
                position = "down";
            }

            if (idx != -1)
            {
                return ship[idx].GetComponent<Module>().checkPlacment(module.GetComponent<Module>(), position);
            }
            return false;
        }
    }
}
