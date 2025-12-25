using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject player;
    public float spawnTerm = 5;
    public float fasterEverySpawn = 0.05f;
    public float minSpawnTerm = 1;
    public TextMeshProUGUI scoreText;

    private float timeAfterLastSpawn;
    private float score;
    private ObjectPool enemyPool;
    public ObjectPool gemPool;

    void Awake()
    {
        enemyPool = GetComponent<ObjectPool>();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Update()
    {
        timeAfterLastSpawn += Time.deltaTime;
        score += Time.deltaTime;
        
        if (timeAfterLastSpawn > spawnTerm)
        {
            timeAfterLastSpawn -= spawnTerm;
            SpawnEnemy();
            spawnTerm = Mathf.Max(minSpawnTerm, spawnTerm - fasterEverySpawn);
        }
        scoreText.text = ((int)score).ToString();
    }

    void SpawnEnemy()
    {
        float x = Random.Range(-9f, 9f);
        float y = Random.Range(-4.5f, 4.5f);

        GameObject obj = enemyPool.Get();
        if (obj != null)
        {
            obj.transform.position = new Vector3(x, y, 0);
            obj.GetComponent<EnemyController>().Spawn(player);
        }
    }
}