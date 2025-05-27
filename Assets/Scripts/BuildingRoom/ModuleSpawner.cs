using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModuleSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> modules = new List<GameObject>();
    [SerializeField]
    List<TMP_Text> modulesAmountText = new List<TMP_Text>();
    [SerializeField] GameObject moduleMov;
    List<Vector3> shipPosition = new List<Vector3>();
    List<GameObject> ship = new List<GameObject>();
    List<GameObject> shipModules = new List<GameObject>();

    [SerializeField] bool isInFight = false;

    [SerializeField] TMP_Text warningText;

    public GameObject currentModule;

    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private SceneController sceneController;

    //Checkers
    bool hasCabin = false;
    bool hasEngine = false;
    bool hasLanding = false;
    int weapons = 0;
    int shields = 0;

    [SerializeField]
    GameObject startCombatButton;

    private void Awake()
    {
        if (gameController == null)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
        if (sceneController == null)
        {
            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        }
    }

    private void Start()
    {
        gameController.LoadShipFile();

        if (modules.Count != gameController.modulesAmount.Count)
        {
        }
        if (!isInFight)
        {
            warningText.text = "";
            spawnShip();
            RefershAmounts();
            if (gameController.isInRogueLite)
            {
                startCombatButton.SetActive(true);
            }
            else
                startCombatButton.SetActive(false);
        }
    }

    public GameObject GetModule(string name)
    {
        for (int i = 0; i < modules.Count; i++)
        {
            if (modules[i].name == name)
            {
                return modules[i];
            }
        }
        return null;
    }

    public void DeleteShip()
    {
        for (int i = 0; i < ship.Count; i++)
        {
            if (ship[i].name == "Cabina")
            {
                AddModuleAmount("Cabina", 1);
            }
            else if (ship[i].name == "Passadis")
            {
                AddModuleAmount("Passadis", 1);
            }
            else if (ship[i].name == "Aterratge")
            {
                AddModuleAmount("Aterratge", 1);
            }
            else if (ship[i].name == "Escuts")
            {
                AddModuleAmount("Escuts", 1);
            }
            else if (ship[i].name == "Canyo")
            {
                AddModuleAmount("Canyo", 1);
            }
            else if (ship[i].name == "Motor")
            {
                AddModuleAmount("Motor", 1);
            }
            gameController.DeleteFile();
            Destroy(shipModules[i]);
        }

        ship.Clear();
        shipPosition.Clear();
        shipModules.Clear();
    }

    public void DeSpawnShip()
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
       
        if (gameController.ship.Count > 0)
        {
            this.ship.Clear();
            this.shipPosition.Clear();
            this.shipModules.Clear();
            for (int i = 0; i < gameController.ship.Count; i++)
            {
                GameObject moduleTmp = Instantiate(gameController.ship[i], gameController.shipPosition[i], Quaternion.identity);
                this.shipPosition.Add(gameController.shipPosition[i]);
                this.ship.Add(gameController.ship[i]);
                this.shipModules.Add(moduleTmp);
                weapons = 0;
                shields = 0;
                if (gameController.ship[i].name == "Cabina")
                {
                    hasCabin = true;
                }
                else if(gameController.ship[i].name == "Motor")
                {
                    hasEngine = true;
                }
                else if (gameController.ship[i].name == "Aterratge")
                {
                    hasLanding = true;
                }
                else if (gameController.ship[i].name == "Canyo")
                {
                    weapons++;
                }
                else if (gameController.ship[i].name == "Escuts")
                {
                    shields++;
                }
            }
        }
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
        int idx = modules.IndexOf(module);
        if (gameController.modulesAmount[idx] > 0)
        {
            GameObject moduleTmp = Instantiate(module, mousePos, Quaternion.identity);
            shipPosition.Add(mousePos);
            ship.Add(module);
            shipModules.Add(moduleTmp);
            if (module.GetComponent<Module>().type == "Cabin")
            {
                hasCabin = true;
            }
            else if (module.GetComponent<Module>().type == "Engine")
            {
                hasEngine = true;
            }
            else if (module.GetComponent<Module>().type == "Landing")
            {
                hasLanding = true;
            }
            else if (module.GetComponent<Module>().type == "Weapon")
            {
                weapons++;
            }
            else if (module.GetComponent<Module>().type == "Shield")
            {
                shields++;
            }
            gameController.modulesAmount[idx]--;
            RefershAmounts();
        }
        else
        {
            StartCoroutine(WriteWarning("No et queden moduls"));
        }
    }

    public void AddModuleAmount(string name, int amount)
    {
        for (int i = 0; i < modules.Count; i++)
        {
            if (modules[i].name == name)
            {
                gameController.AddAmount(i, amount);
                if (!isInFight)
                {
                    RefershAmounts();
                }
                gameController.SaveShipFile();
                return;
            }
        }

    }

    public void SaveShip()
    {
        if (hasCabin && hasEngine && hasLanding)
        {
            gameController.ClearList();
            for (int i = 0; i < ship.Count; i++)
            {
                gameController.AddModule(ship[i], shipPosition[i]);
            }
            gameController.ShipWeapon = weapons;
            gameController.ShipShield = shields;
            gameController.SaveShipFile();
        }
        else
        {
            StartCoroutine(WriteWarning("Et falten parts necessaries"));
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
        else if (shipPosition.Contains(pos))
        {
            Debug.Log("Entro");
            return false;
        }
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

    private void RefershAmounts()
    {
        for (int i = 0; i < gameController.modulesAmount.Count; i++)
        {
            modulesAmountText[i].text = gameController.modulesAmount[i].ToString();
        }
    }

    public void StartCombat()
    {
        if (gameController.isShipSaved())
        {
            sceneController.LoadSceneSingle(2);
        }
        else
        {
            StartCoroutine(WriteWarning("Nau no guardada"));
        }
    }

    private IEnumerator WriteWarning(string text)
    {
        warningText.text = text;
        yield return new WaitForSeconds(1f);
        warningText.text = "";
    }

    public void Exit()
    {
        Application.Quit();
    }
}
