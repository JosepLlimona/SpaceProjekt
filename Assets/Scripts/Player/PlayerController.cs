using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Movment")]
    [SerializeField] float speed = 1f;
    [SerializeField] bool canRun = true;
    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;
    [SerializeField] Animator playerAnim;
    Rigidbody2D rb;
    Vector2 dir = Vector2.zero;
    bool isRunning = false;
    bool canDash = true;
    bool isDashing = false;

    [Header("Stats")]
    [SerializeField] float health = 10;
    [SerializeField] int maxHealth = 10;
    [SerializeField] Slider healthBar;
    [SerializeField] int strenght = 10;
    [SerializeField] int dexterity = 10;
    [SerializeField] int constitution = 10;
    [SerializeField] int inteligence = 10; // Ni polles per a que utilitzar-ho
    [SerializeField] int charisma = 10;
    [SerializeField] int luck = 10;

    bool isCovered = false;
    [Header("Combat")]
    [SerializeField] GameObject bullet;
    [SerializeField] bool canShoot = false;
    [SerializeField] bool canMele = false;
    [SerializeField] GameObject bulletSpawn;
    [SerializeField] GameObject bulletPivot;
    [SerializeField] GameObject sword;
    [SerializeField] float shootTime = 0.5f;
    [SerializeField] SwordController swordController;
    float weaponDamage = 1f;
    Vector2 mousePos;
    public float defense = 0;

    public int money = 0;

    bool canTalk = false;
    NPCController npc;
    public bool isTalking = false;

    [Header("Miscellaneous")]
    [SerializeField]
    InventoryController inventory;
    [SerializeField] Animator inventoryAnim;
    public bool canAct = true;



    // Start is called before the first frame update
    void Start()
    {
        isCovered = false;
        rb = GetComponent<Rigidbody2D>();
        shootTime = 0.5f - GetModifier(dexterity) / 20;
        if (shootTime <= 0)
        {
            shootTime = 0.1f;
        }

        maxHealth = 10 + GetModifier(constitution);
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        if (inventory == null)
        {
            inventory = GameObject.Find("Inventory").GetComponent<InventoryController>();
        }

        inventory.changeMoneyAmount(money);
        inventory.player = this;
    }

    private void FixedUpdate()
    {
        if (canAct)
        {
            if (isDashing) { return; }
            rb.velocity = dir * speed;
            if (rb.velocity != Vector2.zero)
            {
                playerAnim.SetFloat("X", rb.velocity.x);
                playerAnim.SetFloat("Y", rb.velocity.y);
                playerAnim.SetBool("isWalking", true);
            }
            else
            {
                playerAnim.SetBool("isWalking", false);
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (canAct)
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
        if (canRun && canAct)
        {
            if (context.performed)
            {
                speed = 2f;
                isRunning = true;
            }
            if (context.canceled)
            {
                if (isRunning)
                {
                    speed = 1f;
                }
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !isTalking && canAct)
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
                int luckMod = GetModifier(luck);
                int rand = Random.Range(1, 10);
                float finalDamage = weaponDamage;
                finalDamage += GetModifier(strenght) / 2; // Afegeix modificador de destresa
                if (rand <= (2 + luckMod)) // 20% + modificador de sort per probabilitat de critic
                {
                    finalDamage *= 2; // Critic x2
                }
                if (finalDamage <= 0)
                {
                    finalDamage = 0.5f;
                }
                swordController.SetDamage(finalDamage);
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
        int luckMod = GetModifier(luck);
        int rand = Random.Range(1, 10);
        float finalDamage = weaponDamage;
        finalDamage += GetModifier(dexterity) / 2; // Afegeix modificador de destresa
        if (rand <= (2 + luckMod)) // 20% + modificador de sort per probabilitat de critic
        {
            finalDamage *= 2; // Critic x2
        }
        if (finalDamage <= 0)
        {
            finalDamage = 0.5f;
        }
        canShoot = false;
        GameObject tmpBullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        Vector2 dir = (mousePos - (Vector2)bulletSpawn.transform.position).normalized;
        tmpBullet.GetComponent<BulletController>().dir = dir;
        Debug.Log("Final Damage: " + finalDamage);
        tmpBullet.GetComponent<BulletController>().SetDamage(finalDamage);
        yield return new WaitForSeconds(shootTime);
        canShoot = true;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !isRunning && canDash && canAct)
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
            if (inventoryAnim.GetBool("IsInStats"))
            {
                StartCoroutine(ChangeToInventory());
            }
            else if (inventoryAnim.GetBool("IsInventory"))
            {
                inventoryAnim.SetBool("IsInventory", false);
                inventoryAnim.SetBool("ChanItS", false);
                inventoryAnim.SetBool("ChanStI", false);
                canAct = true;
            }
            else
            {
                inventoryAnim.SetBool("IsInventory", true);
                canAct = false;
            }
        }
    }

    public void OpenStats(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryAnim.GetBool("IsInventory"))
            {
                StartCoroutine(ChangeToStats());
            }
            else if (inventoryAnim.GetBool("IsInStats"))
            {
                inventoryAnim.SetBool("IsInStats", false);
                inventoryAnim.SetBool("ChanItS", false);
                inventoryAnim.SetBool("ChanStI", false);
                canAct = true;
            }
            else
            {
                inventoryAnim.SetBool("IsInStats", true);
                canAct = false;
            }
        }
    }

    public void CTSButton()
    {
        StartCoroutine(ChangeToStats());
    }

    public void CTIButton()
    {
        StartCoroutine(ChangeToInventory());
    }

    private IEnumerator ChangeToStats()
    {
        inventoryAnim.SetBool("ChanStI", false);
        inventoryAnim.SetBool("ChanItS", true);
        inventoryAnim.SetBool("IsInStats", true);

        yield return new WaitForSeconds(1f);
        inventoryAnim.SetBool("IsInventory", false);
    }

    private IEnumerator ChangeToInventory()
    {
        inventoryAnim.SetBool("ChanItS", false);
        inventoryAnim.SetBool("ChanStI", true);
        inventoryAnim.SetBool("IsInventory", true);

        yield return new WaitForSeconds(1f);
        inventoryAnim.SetBool("IsInStats", false);
    }

    public void Talk(InputAction.CallbackContext context)
    {
        if (context.performed && canTalk)
        {
            isTalking = true;
            canAct = false;
            Debug.Log("Estic parlant");
            npc.StartConversation();
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

        if (collision.tag == "EnemyBullet")
        {
            LoseHealth(1);
        }

        if (collision.tag == "NPC")
        {
            canTalk = true;
            npc = collision.GetComponent<NPCController>();
        }
    }

    private void LoseHealth(int amountLost)
    {
        if (isCovered || isDashing)
            return;
        health -= (amountLost - defense);
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

        if (collision.tag == "NPC")
        {
            canTalk = false;
        }
    }

    private int GetModifier(int stat)
    {
        return (stat - 10);
    }

    public int GetStat(int stat)
    {
        switch (stat)
        {
            case 0: return strenght;
            case 1: return dexterity;
            case 2: return constitution;
            case 3: return inteligence;
            case 4: return charisma;
            case 5: return luck;
            default:
                return 0;
        }
    }

    public void SetStat(int stat, int value)
    {
        switch (stat)
        {
            case 0:
                strenght = value;
                break;
            case 1:
                dexterity = value;
                break;
            case 2:
                constitution = value;
                break;
            case 3:
                inteligence = value;
                break;
            case 4:
                charisma = value;
                break;
            case 5:
                luck = value;
                break;
            default:
                break;
        }
    }

    public int GetCharMod()
    {
        return GetModifier(charisma);
    }

    public void GainMoney(int amount)
    {
        money += amount;
        inventory.changeMoneyAmount(money);
    }

    public void looseMoney(int amount)
    {
        money -= amount;
        inventory.changeMoneyAmount(money);
    }

    public void EquipWeapon(string type, float damage)
    {
        if (type == "Sword")
        {
            canMele = true;
        }
        else if (type == "Gun")
        {
            canShoot = true;
        }
        weaponDamage = damage;
    }

    public void UnequipWeapon()
    {
        canMele = false;
        canShoot = false;
    }

    public void changeDefense(float amount)
    {
        defense += amount;
        Debug.Log("Defensa: " + defense);
    }
}
