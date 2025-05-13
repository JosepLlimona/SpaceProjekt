using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Movment")]
    [SerializeField] float speed = 5f;
    [SerializeField] bool canRun = true;
    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;
    [SerializeField] Animator inventoryAnim;
    Rigidbody2D rb;
    Vector2 dir = Vector2.zero;
    bool isRunning = false;
    bool canDash = true;
    bool isDashing = false;

    [Header("Stats")]
    [SerializeField] int health = 100;
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar;

    bool isCovered = false;
    [Header("Combat")]
    [SerializeField] GameObject bullet;
    [SerializeField] bool canShoot;
    [SerializeField] bool canMele;
    [SerializeField] GameObject bulletSpawn;
    [SerializeField] GameObject bulletPivot;
    [SerializeField] GameObject sword;
    [SerializeField] float shootTime = 0.5f;
    Vector2 mousePos;



    // Start is called before the first frame update
    void Start()
    {
        isCovered = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDashing) { return; }
        rb.velocity = dir * speed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        mousePos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

        float lookAngle = AngleBetweenTwoPoints(bulletPivot.transform.position, mousePos) + 90;
        bulletPivot.transform.eulerAngles = new Vector3(0, 0, lookAngle);
        sword.transform.eulerAngles = new Vector3(0, 0, lookAngle);
    }
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    public void Run(InputAction.CallbackContext context)
    {
        if (canRun)
        {
            if (context.performed)
            {
                speed = 10f;
                isRunning = true;
            }
            if (context.canceled)
            {
                if (isRunning)
                {
                    speed = 5f;
                }
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canShoot && canMele)
            {
                Debug.LogError("Ets burro, nomes pots tenir un tipus d'atac");
                return;
            }
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
            else if (canMele)
            {
                Vector2 direction = mousePos - (Vector2)transform.transform.position;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0)
                        sword.GetComponent<Animator>().SetTrigger("AttackRight");
                    else
                        sword.GetComponent<Animator>().SetTrigger("AttackLeft");
                }
                else
                {
                    if (direction.y > 0)
                        sword.GetComponent<Animator>().SetTrigger("Attack");
                    else
                        sword.GetComponent<Animator>().SetTrigger("AttackDown");
                }
            }
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        GameObject tmpBullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        Vector2 dir = (mousePos - (Vector2)bulletSpawn.transform.position).normalized;
        tmpBullet.GetComponent<BulletController>().dir = dir;
        yield return new WaitForSeconds(shootTime);
        canShoot = true;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !isRunning && canDash)
        {
            StartCoroutine(Dash());
        }
        if (isRunning)
        {
            isRunning = false;
        }
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventoryAnim.SetBool("IsInventory", !inventoryAnim.GetBool("IsInventory"));
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        gameObject.layer = 7;

        if (dir != Vector2.zero)
        {
            rb.velocity = dir * dashingPower;
        }
        else
        {
            rb.velocity = Vector2.up * dashingPower;
        }
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        gameObject.layer = 0;
    }

    public void Heal(int healing)
    {
        health += healing;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthBar.value = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cover")
        {
            Debug.Log("Entro");
            isCovered = true;
            transform.localScale = new Vector3(1f, 0.8f, 1f);
        }

        if(collision.tag == "EnemyBullet")
        {
            LoseHealth(10);
        }
    }

    private void LoseHealth(int amountLost)
    {
        if (/*isCovered || isDashing*/ true)
            return;
        health -= amountLost;
        healthBar.value = health;
        if (health <= 0)
        {
            //GAME OVER
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Cover")
        {
            isCovered = false;
            transform.localScale = new Vector3(1f, 1f, 1f);

        }
    }
}
