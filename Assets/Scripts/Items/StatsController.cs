using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour
{


    [SerializeField]
    PlayerController controller;

    [SerializeField]
    List<TMP_Text> statNumbers = new List<TMP_Text>(); // 1.Strenght 2.Dexterity 3.Constitution 4.Intelligence 5.Charisma 6.Luck
    [SerializeField]
    TMP_Text numberText;

    int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(controller == null)
        {
            GameObject.Find("Player").GetComponent<PlayerController>();
        }

        RefreshText();
    }

    public void GainPoint(int amount)
    {
        points += amount;
    }

    public void IncreaseStat(int idx)
    {
        if (points > 0)
        {
            int stat = int.Parse(statNumbers[idx].text);
            stat++;
            controller.SetStat(idx, stat);
            points--;
            RefreshText();
        }
    }

    public void DecreseStat(int idx)
    {
        int stat = int.Parse(statNumbers[idx].text);
        if(stat > 0)
        {
            stat--;
            controller.SetStat(idx, stat);
            points++;
            RefreshText();
        }
    }

    public void RefreshText()
    {
        for (int i = 0; i < statNumbers.Count; i++)
        {
            statNumbers[i].text = controller.GetStat(i).ToString();
        }
        numberText.text = points.ToString();
    }
}
