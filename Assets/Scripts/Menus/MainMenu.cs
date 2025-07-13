using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameController gameController;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas infoCanvas;

    public void NewGame()
    {
        gameController.changeHistoryMoment();
    }
    public void RogueLiteNew()
    {
        gameController.DeleteFile();
        gameController.isInRogueLite = true;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void RogueLiteContinue()
    {
        if (gameController.isShipSaved())
        {

            gameController.isInRogueLite = true;
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }

    public void info()
    {
        mainCanvas.enabled = !mainCanvas.enabled;
        infoCanvas.enabled = !infoCanvas.enabled;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
