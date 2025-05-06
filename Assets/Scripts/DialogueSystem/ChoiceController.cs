using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceController : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{

    [SerializeField]
    GameObject borders;

    public string chocieText = "";
    public string nextLine;
    public DialogueController dialogueController;
    public bool hasCheck = false;

    private void Start()
    {
        borders.SetActive(false);

        GetComponentInChildren<TMP_Text>().text = chocieText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        borders.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        borders.SetActive(false);
    }

    public void Choose()
    {
        Debug.Log("Entro");
        dialogueController.ChooseOption(nextLine, hasCheck);
    }
}
