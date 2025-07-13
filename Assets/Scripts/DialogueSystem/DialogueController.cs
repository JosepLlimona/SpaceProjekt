using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{

    [SerializeField]
    private TMP_Text textSquare;
    [SerializeField]
    float textAnimTime = 0.5f;
    [SerializeField]
    GameObject choiceItem;
    [SerializeField]
    GameObject choiceBox;

    public TextAsset dialogueFile;

    [SerializeField]
    PlayerController player;
    [SerializeField]
    Animator animator;

    NPCController npc;

    public string[] fs;

    bool canContinue = false;
    public int i;
    public bool hasDecided = false;
    int choiceDificulty = 0; // 1 = Facil, 2 = Mitj, 3 = Dificil
    int price = 0;

    private void Start()
    {
        if (player == null)
        {
            GameObject.Find("Player").GetComponent<PlayerController>();
        }
    }

    // Start is called before the first frame update
    public void StartDialog(NPCController talker)
    {
        npc = talker;
        animator.SetBool("IsInDialog", true);
        if (dialogueFile == null)
        {
            return;
        }

        /*string[]*/
        fs = dialogueFile.text.Split(new string[] { ",", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        StartCoroutine(ManageDialogue(fs));
    }

    private IEnumerator ManageDialogue(string[] script)
    {
        for (i = 0; i < script.Length; i++)
        {
            canContinue = false;
            StopCoroutine("TextAnim");
            string textToSay = script[i];
            canContinue = false;
            i++;
            switch (script[i])
            {
                case "C":
                    StartCoroutine(TextAnim(textToSay));
                    while (!canContinue)
                    {
                        yield return null;
                    }
                    i++;
                    int next = int.Parse(script[i]);
                    break;
                case "O":
                    StartCoroutine(TextAnim(textToSay));
                    while (!canContinue)
                    {
                        yield return null;
                    }
                    i++;
                    int nextChoices = int.Parse(script[i]);
                    i = nextChoices * 3;
                    yield return StartCoroutine(ManageChoices(script));
                    break;
                case "F":
                    i++;
                    int ending = int.Parse(script[i]);
                    if (ending == 0)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                    }
                    else if (ending > 0)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                        player.GainMoney(ending);
                    }
                    else if (ending == -1)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                    }
                    else if (ending == -2)
                    {
                        if (player.money >= 10)
                        {
                            StartCoroutine(TextAnim(textToSay));
                            while (!canContinue)
                            {
                                yield return null;
                            }
                            player.GainMoney(-10);
                            player.canAct = false;
                            SceneManager.LoadScene(4, LoadSceneMode.Additive);
                        }
                        else
                        {
                            textToSay = "No tens suficients diners.";
                            StartCoroutine(TextAnim(textToSay));
                            while (!canContinue)
                            {
                                yield return null;
                            }
                        }
                    }
                    else if (ending == -3)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                        player.StartCombat();
                    }
                    else if (ending == -4)
                    {
                        if (player.money >= 100)
                        {
                            StartCoroutine(TextAnim(textToSay));
                            while (!canContinue)
                            {
                                yield return null;
                            }
                            player.GainModule();
                        }
                        else
                        {
                            textToSay = "No tens suficients diners.";
                            StartCoroutine(TextAnim(textToSay));
                            while (!canContinue)
                            {
                                yield return null;
                            }
                        }
                    }
                    else if(ending == -5)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                        player.ObtainWeapon(2);
                    }
                    else if (ending == -6)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                        player.ObtainWeapon(1);
                    }
                    else if (ending == -7)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                        player.changeHistoryMoment();
                    }
                    else if (ending == -8)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                        SceneManager.LoadScene(2, LoadSceneMode.Single);
                    }
                    else if(ending == -9)
                    {
                        if (!player.hasMercadery)
                        {
                            textToSay = "Encara no tens la mercaderia.";
                        }
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                        player.changeHistoryMoment();
                    }
                    else if(ending == -10)
                    {
                        StartCoroutine(TextAnim(textToSay));
                        while (!canContinue)
                        {
                            yield return null;
                        }
                        player.hasMercadery = true;
                    }
                        animator.SetBool("IsInDialog", false);
                    player.isTalking = false;
                    player.canAct = true;
                    npc.ChangeFile();
                    yield break;
                default:
                    break;
            }
        }
    }

    private IEnumerator ManageChoices(string[] script)
    {
        bool isInChoices = true;
        //i++;
        DeleteOptions();
        textSquare.gameObject.SetActive(false);
        choiceBox.SetActive(true);
        while (isInChoices)
        {
            GameObject choiceTmp = Instantiate(choiceItem);
            choiceTmp.GetComponent<ChoiceController>().chocieText = script[i];
            i++;
            choiceTmp.GetComponent<ChoiceController>().nextLine = script[i];
            choiceTmp.GetComponent<ChoiceController>().dialogueController = this;
            choiceTmp.transform.SetParent(choiceBox.transform.GetChild(0));
            choiceTmp.transform.localScale = Vector3.one;
            i++;
            if (script[i] == "F")
            {
                isInChoices = false;
            }
            else if (script[i] == "MCF")
            {
                choiceTmp.GetComponent<ChoiceController>().hasCheck = true;
                choiceDificulty = 1;
            }
            else if (script[i] == "MCM")
            {
                choiceTmp.GetComponent<ChoiceController>().hasCheck = true;
                choiceDificulty = 2;
            }
            else if (script[i] == "MCH")
            {
                choiceTmp.GetComponent<ChoiceController>().hasCheck = true;
                choiceDificulty = 3;
            }
            else if (script[i] == "FCF")
            {
                choiceTmp.GetComponent<ChoiceController>().hasCheck = true;
                isInChoices = false;
                choiceDificulty = 1;
            }
            else if (script[i] == "FCM")
            {
                choiceTmp.GetComponent<ChoiceController>().hasCheck = true;
                isInChoices = false;
                choiceDificulty = 1;
            }
            else if (script[i] == "FCH")
            {
                choiceTmp.GetComponent<ChoiceController>().hasCheck = true;
                isInChoices = false;
                choiceDificulty = 1;
            }
            else if (script[i].Contains("MB"))
            {
                string[] separator = script[i].Split('-');
                price = int.Parse(separator[1]);
                choiceTmp.GetComponent<ChoiceController>().hasToPay = true;
            }
            else if (script[i].Contains("FB"))
            {
                string[] separator = script[i].Split('-');
                price = int.Parse(separator[1]);
                choiceTmp.GetComponent<ChoiceController>().hasToPay = true;
                isInChoices = false;
            }
            i++;
        }

        while (!hasDecided)
        {
            yield return null;
        }
        choiceBox.SetActive(false);
        textSquare.gameObject.SetActive(true);
    }

    public void ChooseOption(string nextLine, bool hasCheck, bool hasToPay)
    {
        hasDecided = true;
        if (hasCheck)
        {
            int charMod;
            if (choiceDificulty == 1)
            {
                charMod = player.GetCharMod() * 3;
            }
            else if (choiceDificulty == 2)
            {
                charMod = player.GetCharMod() * 2;
            }
            else
            {
                charMod = player.GetCharMod();
            }
            if (charMod <= 0)
            {
                charMod = 0;
            }
            int maxRand = 12 - charMod;
            if (maxRand <= 0)
                maxRand = 1;
            string[] next = nextLine.Split('-');
            int rand = Random.Range(1, maxRand);


            if (rand == 1)
            {
                i = (int.Parse(next[0]) * 3) - 1;
            }
            else if (rand > 1)
            {
                i = (int.Parse(next[1]) * 3) - 1;
            }
        }
        else if (hasToPay)
        {
            string[] next = nextLine.Split('-');
            if (player.money >= price)
            {
                i = (int.Parse(next[1]) * 3) - 1;
                player.looseMoney(price);
            }
            else
            {
                i = (int.Parse(next[0]) * 3) - 1;
            }
        }
        else
        {
            i = (int.Parse(nextLine) * 3) - 1;
        }

    }

    private IEnumerator TextAnim(string text)
    {
        string msg = "";
        char[] characters = text.ToCharArray();

        foreach (char c in characters)
        {
            if (c == ';')
                msg += ",";
            else
                msg += c;
            textSquare.text = msg;
            yield return new WaitForSeconds(textAnimTime);
        }
    }

    public void ContinueDialogue()
    {
        canContinue = true;
        hasDecided = false;
        StopCoroutine("TextAnim");
    }

    public void DeleteOptions()
    {
        foreach (Transform child in choiceBox.transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }
    }
}
