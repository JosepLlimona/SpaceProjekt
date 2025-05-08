using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    [Header ("Stats")]
    [SerializeField]
    int health = 100;

    [SerializeField]
    Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Sword" || collision.tag == "Bullet")
        {
            LoseHealth(10);
        }
    }

    private void LoseHealth(int amountLost)
    {
        Debug.Log("Lossing Health");
        health -= amountLost;
        healthBar.value = health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
