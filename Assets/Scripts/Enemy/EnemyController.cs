using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    Rigidbody2D rb;

    [Header("Stats")]
    [SerializeField]
    float health = 10;

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
    bool isCovered = false;
    Vector3 coverPos;
    GameObject coverObject;
    Vector2 last = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        iaController = GameObject.Find("IAController").GetComponent<IAController>();
        iaController.AddEnemy(this);
        healthBar.maxValue = health;
    }


    public void StartHostile()
    {
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
                isCovered = true;
                transform.localScale = new Vector3(1f, 0.8f, 1f);
                StartCoroutine(KnowAction());
            }
            last = movePos;
        }

        if (isCovered)
        {
            GameObject player = GameObject.Find("Player");

            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
            if (hit.transform.tag == "Player")
            {
                bool canChange = iaController.AskIfAbailableCover();
                if (canChange)
                {
                    StopAllCoroutines();
                    actual = EnemyActions.Wait;
                    iaController.VacateCover(coverObject);
                    StartCoroutine(KnowAction());
                }
                else { isCovered = false; }
            }
        }
    }

    private IEnumerator KnowAction()
    {
        if (actual == EnemyActions.Wait || actual == EnemyActions.WaitBAttack)
        {
            canAct = iaController.AskToAct(this);
        }
        if (!canAct)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(KnowAction());
        }
        else
        {
            actual = iaController.AskAction(actual);
        }
        yield return StartCoroutine(Act());
    }

    private IEnumerator Act()
    {
        if (isMoving)
            yield break;
        if (actual == EnemyActions.Wait)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(KnowAction());
        }
        else if (actual == EnemyActions.GettingCover && !isMoving)
        {
            coverObject = iaController.AskCover();
            if (coverObject == null)
            {
                actual = EnemyActions.Wait;
                iaController.FinishTurn(this);
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
            yield return new WaitForSeconds(5f);
            StartCoroutine(KnowAction());
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
        isCovered = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
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
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        iaController.FinishTurn(this);
        isCovered = true;
        transform.localScale = new Vector3(1f, 0.8f, 1f);
        StartCoroutine(KnowAction());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            float damage = collision.GetComponent<BulletController>().GetDamage();
            LoseHealth(damage);
        }
        else if(collision.tag == "Sword")
        {
            float damage = collision.GetComponent<SwordController>().GetDamage();
            LoseHealth(damage);
        }
    }

    private void LoseHealth(float amountLost)
    {
        if (isCovered)
            return;
        Debug.Log(gameObject.name + ": Lossing Health: " + amountLost);
        health -= amountLost;
        healthBar.value = health;
        if (health <= 0)
        {
            iaController.VacateCover(coverObject);
            iaController.DestoryEnemy(this);
            Destroy(gameObject);
        }
    }
}
