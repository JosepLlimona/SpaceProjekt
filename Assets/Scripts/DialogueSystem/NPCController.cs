using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    TextAsset dialogueFile;

    [SerializeField]
    DialogueController dialogueController;


    private void Start()
    {
        if(dialogueController != null)
        {
            dialogueController = GameObject.Find("DialogueContoller").GetComponent<DialogueController>();
        }
    }

    public void StartConversation()
    {
        dialogueController.dialogueFile = dialogueFile;
        dialogueController.StartDialog();
    }
}
