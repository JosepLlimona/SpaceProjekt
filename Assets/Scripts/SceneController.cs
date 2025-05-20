using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController _instance;
    public static SceneController Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneSingle(int scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void LoadSceneAdditive(int scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }
}
