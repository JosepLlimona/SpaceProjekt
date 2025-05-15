using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

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

    [SerializeField]
    TextAsset dialogueFile;

    [SerializeField]
    PlayerController player;

    bool canContinue = false;
    int i;
    public bool hasDecided = false;
    int choiceDificulty = 0; // 1 = Facil, 2 = Mitj, 3 = Dificil
    int price = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (dialogueFile == null)
        {
            Debug.LogError("No has ficat un arxiu");
            return;
        }

        string[] fs = dialogueFile.text.Split(new string[] { ",", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        StartCoroutine(ManageDialogue(fs));
    }

    private IEnumerator ManageDialogue(string[] script)
    {
        for (i = 0; i < script.Length; i++)
        {
            canContinue = false;
            StartCoroutine(TextAnim(script[i]));
            while (!canContinue)
            {
                yield return null;
            }
            Debug.Log("He sortit");
            canContinue = false;
            i++;
            switch (script[i])
            {
                case "C":
                    i++;
                    int next = int.Parse(script[i]);
                    break;
                case "O":
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
                        Debug.Log("Final del dialeg");
                    }
                    else if(ending > 0)
                    {
                        Debug.Log("Guanayt: " + ending);
                    }
                    else if(ending == -1)
                    {
                        Debug.Log("Entrant botiga");
                    }
                    else if (ending == -2)
                    {
                        Debug.Log("Entrant minijoc");
                    }
                    else if(ending == -3)
                    {
                        Debug.Log("Iniciant combat");
                    }
                    else if(ending == -4)
                    {
                        Debug.Log("Conseguint objecte");
                    }

                        yield break;
                default:
                    Debug.Log(script[i]);
                    break;
            }
        }
    }

    private IEnumerator ManageChoices(string[] script)
    {
        bool isInChoices = true;
        //i++;
        Debug.Log("Entro MC");
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
            else if (script[i].Contains("MB")){
                string[] separator = script[i].Split('-');
                price = int.Parse(separator[1]);
                Debug.Log("Preu: " + price);
                choiceTmp.GetComponent<ChoiceController>().hasToPay = true;
            }
            else if (script[i].Contains("FB"))
            {
                string[] separator = script[i].Split('-');
                price = int.Parse(separator[1]);
                Debug.Log("Preu: " + price);
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
            if(player.money >= price)
            {
                i = (int.Parse(next[1]) * 3) - 1;
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
            msg += c;
            textSquare.text = msg;
            yield return new WaitForSeconds(textAnimTime);
        }
    }

    public void ContinueDialogue()
    {
        canContinue = true;
        StopCoroutine("TextAnim");
    }
}
