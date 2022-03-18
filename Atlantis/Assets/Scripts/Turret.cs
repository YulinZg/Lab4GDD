using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] public bool isMiddle = false;
    [SerializeField] private bool isLeft = false;
    [SerializeField] private bool isRight = false;
    [SerializeField] private float coolDown;

    //private bool alt = false;
    //private float inputTimer;
    private int count = 0;
    private float shootTimer;
    private bool canShoot = false;
    private bool leftAlt = false;
    private bool rightAlt = false;
    private bool leftIsDown = false;
    private bool rightIsDown = false;
    private bool isAlive = true;
    private ExplosionController explosionController;

    // Start is called before the first frame update
    void Start()
    {
        explosionController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplosionController>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        if (count >= 1)
            shootTimer += Time.deltaTime;
        if (shootTimer >= coolDown)
        {
            count = 0;
            shootTimer = 0;
        }
    }

    void Shoot()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputTimer = 0;
            alt = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            alt = false;
            if (inputTimer <= 0.2f)
                if (isMiddle)
                    SpawnBullet();
        }
        if (alt)
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 0.2f)
            {
                if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && isLeft)
                    SpawnBullet();
                if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && isRight)
                    SpawnBullet();
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            leftIsDown = true;
            leftAlt = true;
            rightAlt = false;
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            leftIsDown = false;
            leftAlt = false;
            if (rightIsDown)
                rightAlt = true;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rightIsDown = true;
            rightAlt = true;
            leftAlt = false;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rightIsDown = false;
            rightAlt = false;
            if (leftIsDown)
                leftAlt = true;
        }

        if (isLeft)
            if (leftAlt)
                canShoot = true;
            else
                canShoot = false;
        else if (isRight)
            if (rightAlt)
                canShoot = true;
            else
                canShoot = false;
        else if (isMiddle)
            if (!leftAlt && !rightAlt)
                canShoot = true;
            else
                canShoot = false;

        if (Input.GetKeyDown(KeyCode.Space) && canShoot && isAlive)
            SpawnBullet();
    }

    void SpawnBullet()
    {
        if (count < 2)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
            bulletInstance.transform.parent = transform;
            bulletInstance.transform.Rotate(0, 0, -90.0f);
            count++;
        }
    }

    public void Hitten()
    {
        isAlive = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in buildings)
        {
            building.GetComponent<BoxCollider2D>().enabled = true;
        }
        StartCoroutine(DoBlink(2, 40));
        explosionController.Explosion(transform.position + new Vector3(0, -0.3f, 0), 0.8f, 0.7f, 20, 2.0f);
    }

    IEnumerator DoBlink(float blinkTime, float blinkNum)
    {
        for (int i = 0; i < blinkNum; i++)
        {
            foreach (Transform child in transform)
                child.gameObject.GetComponent<SpriteRenderer>().enabled = !child.gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(blinkTime / blinkNum);
        }
        Destroy(gameObject);
    }
}
