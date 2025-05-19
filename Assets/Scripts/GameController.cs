using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    //ShipConstruction
    public List<Vector3> shipPosition = new List<Vector3>();
    public List<GameObject> ship = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ClearList()
    {
        ship.Clear();
        shipPosition.Clear();
    }

    public void AddModule(GameObject module, Vector3 pos)
    {
        ship.Add(module);
        shipPosition.Add(pos);
    }

    public List<GameObject> getShip()
    {
        Debug.Log("Retrono: " + ship.Count);
        return ship;
    }
    public List<Vector3> getShipPos()
    {
        return shipPosition;
    }
}
