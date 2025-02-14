using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Movment")]
    [SerializeField] float speed = 5f;
    [SerializeField] bool canRun = true;
    Rigidbody2D rb;
    Vector2 dir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
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
            if (context.started)
            {
                Debug.Log("Started");
                speed = 10f;
            }
            if (context.canceled)
            {
                Debug.Log("Canceled");
                speed = 5f;
            }
        }
    }
}
