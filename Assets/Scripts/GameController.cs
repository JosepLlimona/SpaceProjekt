using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public bool isInRogueLite = false;

    [Header("Stats")]
    [SerializeField] float health = 10;
    [SerializeField] int maxHealth = 10;
    [SerializeField] int strenght = 10;
    [SerializeField] int dexterity = 10;
    [SerializeField] int constitution = 10;
    [SerializeField] int inteligence = 10; // Ni polles per a que utilitzar-ho
    [SerializeField] int charisma = 10;
    [SerializeField] int luck = 10;

    [Header("MainGame")]
    public List<int> moduleInventory = new List<int>();
    public HistoryMoments actualHistoryMoment;
    public List<Vector3> mainShipPosition = new List<Vector3>();
    public List<GameObject> mainShip = new List<GameObject>();
    public int mainShipWeapon = 0;
    public int mainShipShield = 0;
    public bool hasMercancy = false;
    public List<int> playerInventory = new List<int>();

    [Header("Roguelite")]
    public bool firstTimeBuilding = true;

    //ShipConstruction
    public List<Vector3> shipPosition = new List<Vector3>();
    public List<GameObject> ship = new List<GameObject>();
    public List<int> modulesAmount = new List<int>();
    /*
     * 0 = Cabina
     * 1 = Passadis
     * 2 = Aterratge
     * 3 = Escuts
     * 4 = Canyo
     * 5 = Motor
     */
    public int ShipWeapon = 0;
    public int ShipShield = 0;

    string fileName = "Ship";

    public int IALevel = 1;

    public GameObject zarvis;
    [SerializeField] TextAsset newZarvisText;

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
    private void Start()
    {
        actualHistoryMoment = HistoryMoments.MainMenu;
    }

    public void ClearList()
    {
        ship.Clear();
        shipPosition.Clear();
    }

    public void ClearListMain()
    {
        mainShip.Clear();
        mainShipPosition.Clear();
    }

    public void AddModule(GameObject module, Vector3 pos)
    {
        ship.Add(module);
        shipPosition.Add(pos);
    }

    public void AddModuleMain(GameObject module, Vector3 pos)
    {
        mainShip.Add(module);
        mainShipPosition.Add(pos);
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

    public int GetMainShipHealth()
    {
        return mainShip.Count;
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

    public void AddAmountInventory(int idx, int amount)
    {
        moduleInventory[idx]++;
        ModuleGained();
    }

    public void DeleteFile()
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    public void ModuleGained()
    {
        zarvis.transform.position = new Vector3(-2.552f, 1.708f, 0);
        zarvis.GetComponent<NPCController>().dialogueFile.Clear();
        zarvis.GetComponent<NPCController>().dialogueFile.Add(newZarvisText);
        zarvis.GetComponent<NPCController>().i = 0;
    }

    public void changeHistoryMoment()
    {
        if (actualHistoryMoment == HistoryMoments.MainMenu)
            actualHistoryMoment = HistoryMoments.Start;
        else if (actualHistoryMoment == HistoryMoments.Start)
            actualHistoryMoment = HistoryMoments.Earth;
        else if (actualHistoryMoment == HistoryMoments.Earth)
            actualHistoryMoment = HistoryMoments.SpaceFight;
        else if (actualHistoryMoment == HistoryMoments.SpaceFight)
            actualHistoryMoment = HistoryMoments.Planet;
        else if (actualHistoryMoment == HistoryMoments.Planet)
            actualHistoryMoment = HistoryMoments.End;
        ManageHistoryMoment();
    }

    public void ManageHistoryMoment()
    {
        if (actualHistoryMoment == HistoryMoments.SpaceFight)
        {
            isInRogueLite = false;
            GameObject.Find("Player").GetComponent<PlayerController>().SaveStats();
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if(actualHistoryMoment == HistoryMoments.Start)
        {
            SceneManager.LoadScene(8, LoadSceneMode.Single);
        }
        else if (actualHistoryMoment == HistoryMoments.Earth)
        {
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }
        else if (actualHistoryMoment == HistoryMoments.Planet)
        {
            SceneManager.LoadScene(6, LoadSceneMode.Single);
        }
        else if (actualHistoryMoment == HistoryMoments.End)
        {
            SceneManager.LoadScene(7, LoadSceneMode.Single);
        }
    }

    public void SaveStats(float health, int strenght, int dexterity, int constitution, int inteligence, int charisma, int luck)
    {
        this.health = health;
        this.strenght = strenght;
        this.dexterity = dexterity;
        this.constitution = constitution;
        this.inteligence = inteligence;
        this.charisma = charisma;
        this.luck = luck;
    }

    public void LoadStats(PlayerController player)
    {
        player.loadStats(health, strenght, dexterity, constitution, inteligence, charisma, luck);
    }

    public void ChooseStarter(int starter)
    {
        if(starter == 1)
        {
            strenght = 10;
            dexterity = 14;
            constitution = 8;
            inteligence = 10;
            charisma = 12;
            luck = 8;

            playerInventory.Add(0);
            playerInventory.Add(0);
            playerInventory.Add(0);
            playerInventory.Add(5);
            playerInventory.Add(7);
        }
        else if(starter == 2)
        {
            strenght = 16;
            dexterity = 8;
            constitution = 14;
            inteligence = 10;
            charisma = 6;
            luck = 10;

            playerInventory.Add(0);
            playerInventory.Add(0);
            playerInventory.Add(0);
            playerInventory.Add(2);
            playerInventory.Add(6);
            playerInventory.Add(7);

        }
        else if(starter == 3)
        {
            strenght = 6;
            dexterity = 8;
            constitution = 10;
            inteligence = 12;
            charisma = 16;
            luck = 10;

            playerInventory.Add(0);
            playerInventory.Add(0);
            playerInventory.Add(0);
            playerInventory.Add(5);
        }
        else if(starter == 4)
        {
            strenght = 10;
            dexterity = 10;
            constitution = 10;
            inteligence = 10;
            charisma = 10;
            luck = 10;
        }
        changeHistoryMoment();
    }
}
