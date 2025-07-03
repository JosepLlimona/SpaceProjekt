using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    TextAsset[] dialogueFile;
    int i = 0;

    [SerializeField]
    DialogueController dialogueController;


    private void Start()
    {
        if(dialogueController != null)
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
        if(i<(dialogueFile.Length - 1))
        {
            i++;
        }
    }
}
