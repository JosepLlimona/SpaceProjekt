using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceFightingController : MonoBehaviour
{
    GameController controller;
    ModuleSpawner spawner;
    SceneController sceneController;

    int maxWeapons = 0;
    int weapons = 0;
    int maxShields = 0;
    int shields = 0;
    int maxActions = 0;
    int actions = 0;
    int playerHealth = 0;

    [SerializeField]
    TMP_Text actionNumbers;
    [SerializeField]
    TMP_Text actionListText;
    [SerializeField]
    TMP_Text actionListTextIA;
    [SerializeField]
    TMP_Text healthNumber;
    [SerializeField]
    TMP_Text healthNumberIA;
    [SerializeField]
    TMP_Text warningText;
    [SerializeField]
    List<Button> priceButtons = new List<Button>();
    [SerializeField]
    Canvas mainCanvas;
    [SerializeField]
    Canvas winCanvas;

    [SerializeField]
    int IAHealth = 0;

    List<string> actionsList = new List<string>();
    public List<string> actionsListIA = new List<string>();

    private void Awake()
    {
        if (controller == null)
        {
            controller = GameObject.Find("GameController").GetComponent<GameController>();
        }
        if (spawner == null)
        {
            spawner = GameObject.Find("ModuleSpawner").GetComponent<ModuleSpawner>();
            if (spawner != null)
            {
                StartCoroutine(WriteWarning("Spawner torbat"));
            }
        }
        if (sceneController == null)
        {
            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WriteWarning("Estic entrant"));
        mainCanvas.enabled = true;
        winCanvas.enabled = false;

        spawner.spawnShip();
        maxWeapons = controller.GetShipWeapons();
        maxShields = controller.GetShipShield();
        weapons = maxWeapons;
        shields = maxShields;

        maxActions = (weapons + shields) + 3;
        actions = maxActions;
        RefreshActionNumber();
        actionListText.text = "";
        actionListTextIA.text = "";
        warningText.text = "";
        IAActions();
        playerHealth = controller.GetShipHealth();
        RefreshPlayerHealthNumber();
        IAHealth = controller.IALevel + 1;
        RefreshIAHelathNumber();
    }

    public void AddAction(string type)
    {
        if (actions <= 0)
        {
            return;
        }
        if (type == "Attack" && weapons > 0)
        {
            actionListText.text += "- Atacar\n";
            weapons--;
            actions--;
            actionsList.Add(type);
        }
        else if (type == "Attack" && weapons <= 0)
        {
            StartCoroutine(WriteWarning("Armes sobreescalfades"));
        }
        else if (type == "Defense" && shields > 0)
        {
            actionListText.text += "- Defensar\n";
            shields--;
            actions--;
            actionsList.Add(type);
        }
        else if (type == "Defense" && shields <= 0)
        {
            StartCoroutine(WriteWarning("Tots els escuts activats"));
        }
        else if (type == "Wait")
        {
            actionListText.text += "- Esperar\n";
            actions--;
            actionsList.Add(type);
        }
        RefreshActionNumber();
    }

    void RefreshActionNumber()
    {
        actionNumbers.text = actions.ToString();
    }

    void RefreshPlayerHealthNumber()
    {
        healthNumber.text = playerHealth.ToString();
    }

    void RefreshIAHelathNumber()
    {
        healthNumberIA.text = IAHealth.ToString();
    }

    void IAActions()
    {
        actionsListIA.Clear();
        int iaActions = controller.IALevel + 3;
        int iaWeapons = (int)Mathf.Ceil(controller.IALevel / 2f);
        int iaShields = (int)Mathf.Floor(controller.IALevel / 2f);
        for (int i = 0; i < iaActions; i++)
        {
            actionListTextIA.text += "- ???\n";
            int rand = Random.Range(1, 4);
            if (rand == 1 && iaWeapons > 0)
            {
                actionsListIA.Add("Attack");
                iaWeapons--;
            }
            else if (rand == 2 && iaShields > 0)
            {
                actionsListIA.Add("Defense");
                iaShields--;
            }
            else
            {
                actionsListIA.Add("Wait");
            }
        }
    }

    public void Act()
    {
        if (actions == 0)
        {
            Debug.Log("Actuant");
            StartCoroutine(ResolveFight());
        }
        else
        {
            StartCoroutine(WriteWarning("No has acabat d'actuar"));
        }
    }

    private IEnumerator ResolveFight()
    {
        Debug.Log("Entro");
        actionListTextIA.text = "";
        for (int i = 0; i < actionsListIA.Count; i++)
        {
            actionListTextIA.text += "- " + actionsListIA[i] + "\n";
        }
        yield return new WaitForSeconds(1f);
        Debug.Log("despres del wait");
        int j = 0;
        if (actionsList.Count >= actionsListIA.Count)
        {
            while (j < actionsListIA.Count)
            {
                Debug.Log("Comprovant: Jugador: " + actionsList[j] + " IA: " + actionsListIA[j]);
                CheckActions(actionsList[j], actionsListIA[j]);
                j++;
            }
            while (j < actionsList.Count)
            {
                if (actionsList[j] == "Attack")
                {
                    HurtIA();
                }
                j++;
            }
        }
        else if (actionsList.Count < actionsListIA.Count)
        {
            while (j < actionsList.Count)
            {
                Debug.Log("Comprovant: Jugador: " + actionsList[j] + " IA: " + actionsListIA[j]);
                CheckActions(actionsList[j], actionsListIA[j]);
                j++;
            }
            while (j < actionsListIA.Count)
            {
                if (actionsListIA[j] == "Attack")
                {
                    HurtPlayer();
                }
                j++;
            }
        }

        yield return new WaitForSeconds(1f);
        ResetTurn();
    }

    private void ResetTurn()
    {
        weapons = maxWeapons;
        shields = maxShields;
        actions = maxActions;
        RefreshActionNumber();
        actionsList.Clear();
        actionListText.text = "";
        actionListTextIA.text = "";
        IAActions();
    }

    private void HurtPlayer()
    {
        playerHealth--;
        RefreshPlayerHealthNumber();
        if (playerHealth <= 0)
        {
            controller.DeleteFile();
            sceneController.LoadSceneSingle(0);
        }
    }

    private void HurtIA()
    {
        IAHealth--;
        RefreshIAHelathNumber();
        if (IAHealth <= 0)
        {
            GivePrize();
        }
    }

    private IEnumerator WriteWarning(string text)
    {
        warningText.text = text;
        yield return new WaitForSeconds(1f);
        warningText.text = "";
    }

    private void CheckActions(string playerAction, string iaAction)
    {
        if (playerAction == "Attack")
        {
            if (iaAction == "Defense")
            {
                // No passa res
            }
            else
            {
                HurtIA();
            }
        }
        if (iaAction == "Attack")
        {
            if (playerAction == "Defense")
            {
                // No passa res
            }
            else
            {
                HurtPlayer();
            }
        }
    }
    private void GivePrize()
    {
        spawner.DeSpawnShip();
        controller.IALevel++;
        mainCanvas.enabled = false;
        winCanvas.enabled = true;
        for (int i = 0; i < 3; i++)
        {
            int firstPrize = Random.Range(0, 3);
            if (firstPrize == 0)
            {
                var bColors = priceButtons[i].colors;
                bColors.normalColor = Color.green;
                priceButtons[i].colors = bColors;
                priceButtons[i].GetComponentInChildren<TMP_Text>().text = "Escut";
                priceButtons[i].onClick.AddListener(() => { GainPrize("Shield"); });
            }
            else if (firstPrize == 1)
            {
                var bColors = priceButtons[i].colors;
                bColors.normalColor = Color.red;
                priceButtons[i].colors = bColors;
                priceButtons[i].GetComponentInChildren<TMP_Text>().text = "Canyo";
                priceButtons[i].onClick.AddListener(() => { GainPrize("Weapon"); });
            }
            else if (firstPrize == 2)
            {
                var bColors = priceButtons[i].colors;
                bColors.normalColor = new Color(255, 190, 0, 255);
                priceButtons[i].colors = bColors;
                priceButtons[i].GetComponentInChildren<TMP_Text>().text = "Passadís";
                priceButtons[i].onClick.AddListener(() => { GainPrize("Hallway"); });
            }
        }
    }

    private void GainPrize(string type)
    {
        if (type == "Shield")
        {
            spawner.AddModuleAmount("Escuts", 1);
        }
        else if (type == "Weapon")
        {
            spawner.AddModuleAmount("Canyo", 1);
        }
        else if (type == "Hallway")
        {
            spawner.AddModuleAmount("Passadis", 1);
        }
        sceneController.LoadSceneSingle(1);
    }
}
