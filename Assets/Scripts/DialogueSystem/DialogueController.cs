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

    bool canContinue = false;
    int i;
    public bool hasDecided = false;

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
                    Debug.Log("Final del dialeg");
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
            else if (script[i] == "MC")
            {
                choiceTmp.GetComponent<ChoiceController>().hasCheck = true;
            }
            else if (script[i] == "FC")
            {
                choiceTmp.GetComponent<ChoiceController>().hasCheck = true;
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

    public void ChooseOption(string nextLine, bool hasCheck)
    {
        hasDecided = true;
        if (hasCheck)
        {
            /*TODO Check system*/
            string[] next = nextLine.Split('-');
            int rand = Random.Range(1, 10);
            if (rand == 1)
            {
                i = (int.Parse(next[0]) * 3) - 1;
            }
            else if (rand > 1)
            {
                i = (int.Parse(next[1]) * 3) - 1;
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
