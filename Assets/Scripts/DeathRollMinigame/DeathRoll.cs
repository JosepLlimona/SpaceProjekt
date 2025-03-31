using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathRoll : MonoBehaviour
{

    [SerializeField]
    private TMP_Text winnerText;
    [SerializeField]
    private TMP_Text actionList;
    [SerializeField]
    private TMP_Text rollNum;
    [SerializeField]
    private Button rollButton;

    private int MaxNumber = 1000;
    private int currentMax = 1000;
    private bool canRoll = true;
    public void Roll()
    {
        if (canRoll)
        {
            string actionText;
            string tempText;
            int num = Random.Range(1, currentMax);
            currentMax = num;
            rollNum.text = $"1-{num}";

            actionText = $"Has tret un {num}\n";

            tempText = actionList.text;
            tempText += actionText;
            actionList.text = tempText;
            if (num == 1)
            {
                actionText = "Has perdut";
                tempText = actionList.text;
                tempText += actionText;
                actionList.text = tempText;

                winnerText.text = "LOSER";
                return;
            }
            StartCoroutine(IATurn());
        }
    }

    private IEnumerator IATurn()
    {
        canRoll = false;
        rollButton.interactable = false;
        yield return new WaitForSeconds(0.5f);
        string actionText;
        string tempText;
        int num = Random.Range(1, currentMax);
        currentMax = num;
        rollNum.text = $"1-{num}";

        actionText = $"La banca ha tret un {num}\n";

        tempText = actionList.text;
        tempText += actionText;
        actionList.text = tempText;
        if (num == 1)
        {
            actionText = "La banca ha perdut";
            tempText = actionList.text;
            tempText += actionText;
            actionList.text = tempText;

            winnerText.text = "Winner";
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            canRoll = true;
            rollButton.interactable = true;
        }

    }
}
