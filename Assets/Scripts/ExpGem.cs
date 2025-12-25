using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("Gem Settings")]
    public float expValue = 10f;
    public float moveSpeed = 5f;
    public float followDistance = 3f;

    private Transform player;
    private bool isFollowing = false;

    void OnEnable()
    {
        isFollowing = false;
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }
    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < followDistance) 
        {
            isFollowing = true;
        }

        if (isFollowing)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            
            if (distance < 0.2f)
            {
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.AddExp(expValue);
                }
                gameObject.SetActive(false); 
            }
        }
    }
}
