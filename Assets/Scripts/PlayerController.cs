using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3;

    Vector3 move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move = Vector3.zero;
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            move += new Vector3(-1, 0, 0);
        }
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move += new Vector3(1, 0, 0);
        }
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            move += new Vector3(0, 1, 0);
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            move += new Vector3(0, -1, 0);
        }

        move = move.normalized;
        if (move.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (move.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(move * speed * Time.fixedDeltaTime);
    }
}
