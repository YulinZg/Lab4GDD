using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum EnemyType
    {
        small,
        normal,
        big
    };

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float speed;
    [SerializeField] private int hp = 1;
    [SerializeField] private int score;


    private int dir;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x < 0)
            dir = 1;
        else
            dir = -1;
        transform.localScale = new Vector3(transform.localScale.x * dir, transform.localScale.y, transform.localScale.x);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime * Vector3.right;
        if (transform.position.x > 11.5f || transform.position.x < -11.5f)
            Destroy(gameObject);
    }

    public void Hit()
    {
        hp--;
        if (hp == 0)
            Destroy(gameObject);
    }
}
