using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreHistoryManager : MonoBehaviour
{
    public GameController gameController;
    void Start()
    {
        if(gameController == null)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
    }

    public void ChooseStarter(int starter)
    {
        gameController.ChooseStarter(starter);
    }
}
