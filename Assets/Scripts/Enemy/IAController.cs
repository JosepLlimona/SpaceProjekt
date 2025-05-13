using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    private List<EnemyController> enemies = new List<EnemyController>();
    private List<GameObject> covers = new List<GameObject>();
    public List<bool> availableCovers = new List<bool>();

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] coversIM = GameObject.FindGameObjectsWithTag("Cover");
        foreach (GameObject co in coversIM)
        {
            covers.Add(co);
            availableCovers.Add(false);
        }
        Debug.Log("Nº Covers: " + covers.Count);
    }

    public void StartCombat()
    {
        for (int i = 0; i<enemies.Count; i++)
        {
            enemies[i].StartHostile();
        }
    }

    public void AddEnemy(EnemyController enemy)
    {
        Debug.Log("Afegint " + enemy.gameObject.name);
        enemies.Add(enemy);
        Debug.Log("Nº Enemies: " + enemies.Count);
    }

    public void FinishTurn(EnemyController enemy)
    {
        if (enemies.Count > 0 && enemies[0] == enemy)
        {
            EnemyController tmp = enemies[0];
            enemies.RemoveAt(0);
            enemies.Add(tmp);
            Debug.Log(enemy.gameObject.name + ": Finished turn and rotated list.");
        }
    }

    public bool AskToAct(EnemyController enemy)
    {
        Debug.Log("Pregunta: " + enemy.gameObject.name + ", Primer llista: " + enemies[0].gameObject.name);
        if (enemy == enemies[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public EnemyActions AskAction(EnemyActions actualAction)
    {
        if (actualAction == EnemyActions.GettingCover)
        {
            return EnemyActions.Attacking;
        }
        else if (actualAction == EnemyActions.Attacking)
        {
            return EnemyActions.WaitBAttack;
        }
        else if (actualAction == EnemyActions.Wait)
        {
            return EnemyActions.GettingCover;
        }
        else if (actualAction == EnemyActions.WaitBAttack)
        {
            return EnemyActions.Attacking;
        }
        else
        {
            return EnemyActions.Wait;
        }
    }

    public GameObject AskCover()
    {
        if (covers.Count <= 0)
            return null;
        for (int i = 0; i < covers.Count; i++)
        {
            if (!availableCovers[i])
            {
                availableCovers[i] = true;
                return covers[i];
            }
        }

        return null;
    }

    public void VacateCover(GameObject cover)
    {
        for (int i = 0; i < covers.Count; i++)
        {
            if (cover == covers[i])
            {
                availableCovers[i] = false;
            }
        }
    }

    public bool AskIfAbailableCover()
    {
        for (int i = 0; i < covers.Count; i++)
        {
            if (!availableCovers[i])
            {
                return true;
            }
        }
        return false;
    }

    public void DestoryEnemy(EnemyController enemy)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemy == enemies[i])
            {
                enemies.RemoveAt(i);
                return;
            }
        }
    }
}
