using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private enum BuildType
    {
        side,
        middle,
        inner
    };
    [SerializeField] private BuildType buildType;
  
    private ExplosionController explosionController;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        explosionController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplosionController>();
        if (transform.position.x == -6 || transform.position.x == 6)
            buildType = BuildType.side;
        else if (transform.position.x == -4 || transform.position.x == 4)
            buildType = BuildType.middle;
        else if (transform.position.x == -2 || transform.position.x == 2)
            buildType = BuildType.inner;
    }

    public void Hitten()
    {
        GameManagement.Instance.buildingsPos.Enqueue(gameObject.transform.position);
        //GameManagement.Instance.index.Enqueue(GameManagement.Instance.buildings.IndexOf(gameObject));
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(DoBlink(2, 40));
        switch (buildType)
        {
            case BuildType.side:
                explosionController.Explosion(transform.position + new Vector3(0, 0.4f, 0), 0.8f, 0.6f, 20, 2.0f);
                break;
            case BuildType.middle:
                explosionController.Explosion(transform.position + new Vector3(0, 1.0f, 0), 0.9f, 0.4f, 20, 2.0f);
                break;
            case BuildType.inner:
                explosionController.Explosion(transform.position + new Vector3(0, 1.4f, 0), 0.6f, 0.5f, 20, 2.0f);
                break;
        }


    }

    IEnumerator DoBlink(float blinkTime, float blinkNum)
    {
        for (int i = 0; i < blinkNum; i++)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(blinkTime / blinkNum);
        }
        if (--GameManagement.Instance.buildingNumber == 0)
            GameManagement.Instance.lose();   
        Destroy(gameObject);
    }
}
