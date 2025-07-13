using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startCountdown());
    }

    private IEnumerator startCountdown()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
