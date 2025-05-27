using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class GameController : MonoBehaviour
{

    public static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public bool isInRogueLite = false;

    public bool firstTimeBuilding = true;

    //ShipConstruction
    public List<Vector3> shipPosition = new List<Vector3>();
    public List<GameObject> ship = new List<GameObject>();
    public List<int> modulesAmount = new List<int>();
    public int ShipWeapon = 0;
    public int ShipShield = 0;

    string fileName = "Ship";

    public int IALevel = 1;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
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

    public void AddWeapon(int amount)
    {
        ShipWeapon = amount;
    }

    public void AddShield(int amount)
    {
        ShipShield = amount;
    }


    public void SaveShipFile()
    {
        StreamWriter f = File.CreateText(fileName);

        for (int i = 0; i < ship.Count; i++)
        {
            f.WriteLine(ship[i].name + " $ " + shipPosition[i].ToString());
        }
        f.WriteLine("*");
        for (int i = 0; i < modulesAmount.Count; i++)
        {
            f.WriteLine(modulesAmount[i]);
        }
        f.WriteLine(IALevel);

        f.Close();
    }

    public void LoadShipFile()
    {
        if (firstTimeBuilding)
        {
            bool finishFirst = false;
            int j = 0;

            if (!File.Exists(fileName))
            {
                return;
            }
            ship.Clear();
            shipPosition.Clear();
            ShipWeapon = 0;
            ShipShield = 0;
            ModuleSpawner ms = GameObject.Find("ModuleSpawner").GetComponent<ModuleSpawner>();
            string[] f = File.ReadAllLines(fileName);
            for (int i = 0; i < f.Length; i++)
            {
                if (f[i] == "*")
                {
                    finishFirst = true;
                    i++;
                }
                if (!finishFirst)
                {
                    string[] parts = f[i].Split("$");
                    string moduleName = parts[0];
                    moduleName = moduleName.Replace(" ", "");
                    Vector3 pos = StringToVector3(parts[1]);

                    GameObject module = ms.GetModule(moduleName);
                    ship.Add(module);
                    shipPosition.Add(pos);
                    if (module.GetComponent<Module>().type == "Weapon")
                    {
                        ShipWeapon++;
                    }
                    else if (module.GetComponent<Module>().type == "Shield")
                    {
                        ShipShield++;
                    }
                }
                else
                {
                    if (j >= modulesAmount.Count)
                    {
                        IALevel = int.Parse(f[i]);
                    }
                    else
                    {
                        modulesAmount[j] = int.Parse(f[i]);
                    }
                    j++;
                }
            }
            firstTimeBuilding = false;
        }

    }

    public int GetShipWeapons()
    {
        return ShipWeapon;
    }

    public int GetShipShield()
    {
        return ShipShield;
    }

    public int GetShipHealth()
    {
        return ship.Count;
    }

    private Vector3 StringToVector3(string str)
    {

        str = str.Replace("(", "").Replace(")", "");
        string[] values = str.Split(", ");
        float x = float.Parse(values[0], CultureInfo.InvariantCulture);
        float y = float.Parse(values[1], CultureInfo.InvariantCulture);
        float z = float.Parse(values[2], CultureInfo.InvariantCulture);
        Vector3 result = new Vector3(x, y, z);
        return result;
    }

    public bool isShipSaved()
    {
        return File.Exists(fileName);
    }

    public void AddAmount(int idx, int amount)
    {
        modulesAmount[idx]++;
    }

    public void DeleteFile()
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }
}
