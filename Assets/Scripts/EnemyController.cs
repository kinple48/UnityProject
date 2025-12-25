using System;
using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    enum State { Spawning, Moving, Dying }
    private static readonly int AnimSpawn = Animator.StringToHash("Spawn");
    private static readonly int AnimDie = Animator.StringToHash("Die");

    public float speed = 2;
    public Material flashMaterial;
    public Material DefaultMaterial;
    public ObjectPool gemPool;
    
    GameObject target;
    private State state;

    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private Collider2D m_collider;
    private Charater m_character;

    [Header("Data Settings")]
    public EnemyData data;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<Collider2D>();
        m_character = GetComponent<Charater>();
    }

    public void Spawn(GameObject target)
    {
        this.target = target;
        state = State.Spawning;

        if (data != null)
        {
            m_character.MaxHP = data.maxHP;
            speed = data.speed;
        }

        m_character.Initialize(); 
        m_animator.SetTrigger(AnimSpawn);
        m_collider.enabled = false;
        StartCoroutine(StartMovingRoutine());
    }

    IEnumerator StartMovingRoutine()
    {
        yield return new WaitForSeconds(1f);
        m_collider.enabled = true;
        state = State.Moving;
    }
    private void FixedUpdate()
    {
        if (state == State.Moving && target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.fixedDeltaTime, Space.World);
            m_spriteRenderer.flipX = direction.x < 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Dying) return;
        if (collision.CompareTag("Bullet"))
        {
            float d = collision.gameObject.GetComponent<Bullet>().damage;
            if (m_character.Hit(d)) Flash();
            else Die();
        }
    }

    void Flash() => StartCoroutine(FlashRoutine());

    IEnumerator FlashRoutine()
    {
        m_spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.2f);
        m_spriteRenderer.material = DefaultMaterial;
    }

    void Die() => StartCoroutine(DieRoutine());

    IEnumerator DieRoutine()
    {
        state = State.Dying;
        if (m_collider != null) m_collider.enabled = false;
        m_animator.SetTrigger(AnimDie);
        SpawnExpGem();
        yield return new WaitForSeconds(1.7f);
        gameObject.SetActive(false);
    }
    void SpawnExpGem()
    {
        if (GameManager.Instance != null && GameManager.Instance.gemPool != null)
        {
            GameObject gem = GameManager.Instance.gemPool.Get();
            if (gem != null)
            {
                gem.transform.position = transform.position;
            }
        }
    }
}