using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameController gameController;
    [SerializeField]
    SceneController sceneController;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas infoCanvas;

    public void RogueLiteNew()
    {
        gameController.DeleteFile();
        gameController.isInRogueLite = true;
        sceneController.LoadSceneSingle(1);
    }
    public void RogueLiteContinue()
    {
        if (gameController.isShipSaved())
        {

            gameController.isInRogueLite = true;
            sceneController.LoadSceneSingle(1);
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
