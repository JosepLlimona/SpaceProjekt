using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    Rigidbody2D rb;
    float speed = 5f;
    public Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(destroyBullet());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
