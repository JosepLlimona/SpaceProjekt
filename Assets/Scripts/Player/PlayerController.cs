using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log(dir);
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (canRun)
        {
            if (context.performed)
            {
                Debug.Log("Started run");
                speed = 10f;
                isRunning = true;
            }
            if (context.canceled)
            {
                if (isRunning)
                {
                    Debug.Log("Canceled run");
                    speed = 5f;
                }
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !isRunning && canDash)
        {
            Debug.Log("Performed dash");
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
    }

    public void Heal(int healing)
    {
        health += healing;
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }
}
