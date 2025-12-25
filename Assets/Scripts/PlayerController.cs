using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 3;
    public Material flashMaterial;
    public Material DefaultMaterial;

    [Header("Attack Settings")]
    public float shootDelay = 0.5f; 
    private float shootTimer = 0f;
    
    private static readonly int AnimMove = Animator.StringToHash("Move");
    private static readonly int AnimStop = Animator.StringToHash("Stop");
    private static readonly int AnimDie = Animator.StringToHash("Die");

    private Vector3 move;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private Charater m_character;
    private ObjectPool m_bulletPool;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_character = GetComponent<Charater>();
        m_bulletPool = GetComponent<ObjectPool>();
    }

    void Start()
    {
        // 게임 시작할 때 내 체력을 MaxHP만큼 채워라!
        if (m_character != null)
        {
            m_character.Initialize();
        }
    }
    
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        move = new Vector3(x, y, 0).normalized;

        if (move.magnitude > 0)
        {
            m_spriteRenderer.flipX = move.x < 0;
            m_animator.SetTrigger(AnimMove);
        }
        else 
        {
            m_animator.SetTrigger(AnimStop); 
        }

        shootTimer += Time.deltaTime; 
        if (shootTimer >= shootDelay)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    
    
    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 shootDir = mousePos - (transform.position + new Vector3(0, -0.5f));

        GameObject bullet = m_bulletPool.Get();
        if (bullet != null)
        {
            bullet.transform.position = transform.position + new Vector3(0, -0.5f);
            bullet.GetComponent<Bullet>().Direction = shootDir;
        }
    }

    private void FixedUpdate() => transform.Translate(move * (speed * Time.fixedDeltaTime));

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (m_character.Hit(1)) Flash();
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
        m_animator.SetTrigger(AnimDie);
        yield return new WaitForSeconds(0.875f);
        SceneManager.LoadScene("GameOverScene");
    }
}