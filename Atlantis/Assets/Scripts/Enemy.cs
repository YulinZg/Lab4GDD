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
    [SerializeField] private GameObject laser;

    private int dir;
    private bool isAlive = true;
    private ExplosionController explosionController;

    // Start is called before the first frame update
    void Start()
    {
        explosionController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplosionController>();
        if (transform.position.x < 0)
            dir = 1;
        else
            dir = -1;
        transform.localScale = new Vector3(transform.localScale.x * dir, transform.localScale.y, transform.localScale.x);
        int a = Random.Range(0, 10);
        if (a == 0)
        {
            GameObject laserInstance = Instantiate(laser, new Vector3(transform.position.x,  transform.position.y - (transform.position.y + 6.0f) * 0.5f, 0), transform.rotation);
            laserInstance.transform.parent = transform;
            laserInstance.transform.Rotate(0, 0, -90.0f);
            laserInstance.transform.localScale = new Vector3(transform.position.y + 7.0f, laser.transform.localScale.y, 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
            transform.position += dir * speed * Time.deltaTime * Vector3.right;
        if (transform.position.x > 11.5f || transform.position.x < -11.5f)
            Destroy(gameObject);
    }

    public void Hit()
    {
        hp--;
        if (hp == 0)
        {
            switch (enemyType)
            {
                case EnemyType.small:
                    explosionController.Explosion(transform.position, 0.4f, 0.2f, 4, 0.2f);
                    break;
                case EnemyType.normal:
                    explosionController.Explosion(transform.position, 0.6f, 0.3f, 4, 0.2f);
                    break;
                case EnemyType.big:
                    explosionController.Explosion(transform.position, 1.0f, 0.5f, 12, 0.2f);
                    break;
            }
            isAlive = false;
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            StartCoroutine(DoBlink(0.2f, 4));
        }
        else
            StartCoroutine(DoBlink(0.1f, 2));
    }

    IEnumerator DoBlink(float blinkTime, float blinkNum)
    {
        for (int i = 0; i < blinkNum; i++)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(blinkTime / blinkNum);
        }
        if (hp == 0)
            Destroy(gameObject);
    }
}
