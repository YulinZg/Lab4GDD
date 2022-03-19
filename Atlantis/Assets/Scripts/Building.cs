using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int buildingNumber;
  
    private ExplosionController explosionController;
    private SpriteRenderer sprite;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        explosionController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplosionController>();
        if (transform.position.x == -6)
            buildingNumber = 0;
        else if (transform.position.x == -4)
            buildingNumber = 1;
        else if (transform.position.x == -2)
            buildingNumber = 2;
        else if (transform.position.x == 2)
            buildingNumber = 3;
        else if (transform.position.x == 4)
            buildingNumber = 4;
        else if (transform.position.x == 6)
            buildingNumber = 5;
    }

    public void Hitten()
    {
        if (isAlive)
        {
            isAlive = false;
            GameManagement.Instance.buildingNumbers.Push(buildingNumber);
            StartCoroutine(DoBlink(2, 40));
            switch (buildingNumber)
            {
                case 0:
                case 5:
                    explosionController.Explosion(transform.position + new Vector3(0, 0.4f, 0), 0.8f, 0.6f, 20, 2.0f);
                    break;
                case 1:
                case 4:
                    explosionController.Explosion(transform.position + new Vector3(0, 1.0f, 0), 0.9f, 0.4f, 20, 2.0f);
                    break;
                case 2:
                case 3:
                    explosionController.Explosion(transform.position + new Vector3(0, 1.4f, 0), 0.6f, 0.5f, 20, 2.0f);
                    break;
            }
        }
    }

    IEnumerator DoBlink(float blinkTime, float blinkNum)
    {
        if (--GameManagement.Instance.buildingCount == 0)
            GameManagement.Instance.Lose();
        for (int i = 0; i < blinkNum; i++)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(blinkTime / blinkNum);
        }
        Destroy(gameObject);
    }
}
