using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5;
    public float damage = 1;
    Vector2 direction;
    public Vector2 Direction
    {
        set => direction = value.normalized;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}
