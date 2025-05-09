using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    Rigidbody2D rb;

    [Header("Stats")]
    [SerializeField]
    int health = 100;

    [Header("Objects")]
    [SerializeField]
    Slider healthBar;
    [SerializeField]
    GameObject bullet;

    bool canAct = false;
    IAController iaController;
    EnemyActions actual = EnemyActions.Wait;
    bool isMoving = false;
    bool isAttacking = false;
    Vector3 coverPos;
    Vector2 last = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name + " Iniciant");
        rb = GetComponent<Rigidbody2D>();
        iaController = GameObject.Find("IAController").GetComponent<IAController>();
        iaController.AddEnemy(this);
        StartCoroutine(KnowAction());
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 movePos = Vector2.MoveTowards(transform.position, coverPos, 0.2f);
            rb.MovePosition(movePos);
            if (movePos == last)
            {
                isMoving = false;
                StartCoroutine(KnowAction());
                Debug.Log(gameObject.name + ": He arribat");
            }
            last = movePos;
        }
    }

    private IEnumerator KnowAction()
    {
        Debug.Log(gameObject.name + ": Preguntant");
        if (actual == EnemyActions.Wait || actual == EnemyActions.WaitBAttack)
        {
            canAct = iaController.AskToAct(this);
        }
        if (!canAct)
        {
            Debug.Log(gameObject.name + ": No actuo");
            yield return new WaitForSeconds(2f);
            StartCoroutine(KnowAction());
        }
        else
        {
            actual = iaController.AskAction(actual);
            Debug.Log(gameObject.name + ": Actuant = " + actual);
        }
        yield return StartCoroutine(Act());
    }

    private IEnumerator Act()
    {
        if (isMoving)
            yield break;
        if (actual == EnemyActions.Wait)
            yield break;
        else if (actual == EnemyActions.GettingCover && !isMoving)
        {
            GameObject coverObject = iaController.AskCover();
            Debug.Log(gameObject.name + ": Covrint a " + coverObject.name); 
            if (coverObject == null)
            {
                actual = EnemyActions.Wait;
                yield break;
            }
            isMoving = true;
            coverPos = GetCoverPosition(coverObject);
        }
        else if (actual == EnemyActions.Attacking && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
        else if (actual == EnemyActions.WaitBAttack)
        {
            Debug.Log(gameObject.name + ": Esperant despres d'atacar");
            yield return new WaitForSeconds(5f);
            iaController.FinishTurn(this);
            StartCoroutine(KnowAction());
            Debug.Log(gameObject.name + ": Vaig a preguntar");
        }
    }

    private Vector3 GetCoverPosition(GameObject cover)
    {
        GameObject player = GameObject.Find("Player");

        float distance = cover.transform.position.y - player.transform.position.y;
        if (distance > 0)
        {
            return cover.transform.Find("EnemyPosN").transform.position;
        }
        else
        {
            return cover.transform.Find("EnemyPosS").transform.position;
        }
    }

    private IEnumerator Attack()
    {
        Vector2 bulletSpawn = transform.position;
        GameObject player = GameObject.Find("Player");
        if ((player.transform.position.y - transform.position.y) < 0)
        {
            bulletSpawn = transform.position;
            bulletSpawn.y -= 0.7f;
        }
        else
        {
            bulletSpawn.y += 0.7f;
        }
        GameObject tmpBullet = Instantiate(bullet, bulletSpawn, Quaternion.identity);
        Vector2 dir = (player.transform.position - this.transform.position).normalized;
        tmpBullet.GetComponent<BulletController>().dir = dir;
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        iaController.FinishTurn(this);
        StartCoroutine(KnowAction());
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
        Debug.Log(gameObject.name + ": Lossing Health");
        health -= amountLost;
        healthBar.value = health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
