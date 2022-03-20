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
    [SerializeField] private float maxSpeed;
    [SerializeField] private int hp = 1;
    [SerializeField] private int score;
    [SerializeField] private GameObject laser;
    [SerializeField] private float decreaseTime = 1.0f;

    private float timer = 0f;
    private int dir;
    private bool isAlive = true;
    private ExplosionController explosionController;

    private AudioSource[] audioSources;
    [SerializeField] private AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        explosionController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplosionController>();
        if (transform.position.x < 0)
            dir = 1;
        else
            dir = -1;
        transform.localScale = new Vector3(transform.localScale.x * dir, transform.localScale.y, transform.localScale.x);
        int i = Random.Range(0, 10);
        float a = 0;
        a += GameManagement.Instance.wave * 0.5f;
        if (a > 4.0f)
            a = 4.0f;
        if (i <= a)
        {
            GameObject laserInstance = Instantiate(laser, new Vector3(transform.position.x,  transform.position.y - (transform.position.y + 7.0f) * 0.5f, 0), transform.rotation);
            laserInstance.transform.parent = transform;
            laserInstance.transform.Rotate(0, 0, -90.0f);
            laserInstance.transform.localScale = new Vector3(transform.position.y + 10.0f, laser.transform.localScale.y, 1.0f);
        }
        speed += GameManagement.Instance.wave * 0.5f;
        if (speed > maxSpeed)
            speed = maxSpeed;

        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            transform.position += dir * speed * Time.deltaTime * Vector3.right;
            timer += Time.deltaTime;
            if(timer >= decreaseTime)
            {
                score = (int)(score * 0.9f);
                timer = 0;
            }
        }   
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
            if (transform.childCount > 0)
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
            StartCoroutine(DoBlink(0.5f, 10));
            GameManagement.Instance.IncreaseScore(score);
            audioSources[0].clip = audioClips[0];
            audioSources[0].Play();
        }
        else
            StartCoroutine(DoBlink(0.2f, 4));
        audioSources[1].clip = audioClips[1];
        audioSources[1].Play();
    }

    IEnumerator DoBlink(float blinkTime, float blinkNum)
    {
        for (int i = 0; i < blinkNum; i++)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(blinkTime / blinkNum);
        }
        if (hp == 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Invoke(nameof(Disappear), 1.0f);
        }
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
