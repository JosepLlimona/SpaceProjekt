using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    public List<TextAsset> dialogueFile;
    public int i = 0;

    [SerializeField]
    DialogueController dialogueController;


    private void Start()
    {
        if (this.gameObject.name == "Zarvis")
        {
            GameObject.Find("GameController").GetComponent<GameController>().zarvis = this.gameObject;
        }
        if (dialogueController != null)
        {
            dialogueController = GameObject.Find("DialogueContoller").GetComponent<DialogueController>();
        }
        i = 0;
    }

    public void StartConversation()
    {
        dialogueController.dialogueFile = dialogueFile[i];
        dialogueController.StartDialog(this);
    }

    public void ChangeFile()
    {
        if(i<(dialogueFile.Count - 1))
        {
            i++;
        }
    }
}
