using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Building : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] explodeEffect;

    private BuildType buildTyep;

    private enum BuildType{
        side,
        middle,
        inner
    };
    void Start()
    {
        if (gameObject.name == "city1" || gameObject.name == "city6")
            buildTyep = BuildType.side;
        else if(gameObject.name == "city2" || gameObject.name == "city5")
            buildTyep = BuildType.middle;
        else if (gameObject.name == "city3" || gameObject.name == "city4")
            buildTyep = BuildType.inner;
    }
    public void hittingByEnemy()
    { 
        StartCoroutine(doBlink(2, 40));
        if (buildTyep == BuildType.side)
        {
            for (int i = 0; i < 8; i++)
            {
                int a = Random.Range(0, 5);
                float newY;
                float newX;
                if (i < 2){
                    newY = Random.Range(0.5f, 1f);
                    newX = Random.Range(-0.7f, 0f);
                }
                else if( i < 4)
                {
                    newY = Random.Range(0.5f, 1f);
                    newX = Random.Range(0f, 0.7f);
                }
                else if (i < 6)
                {
                    newY = Random.Range(0f, 0.5f);
                    newX = Random.Range(-0.7f, 0f);
                }
                else
                {
                    newY = Random.Range(0f, 0.5f);
                    newX = Random.Range(0f, 0.7f);
                }
                Instantiate<GameObject>(explodeEffect[a], transform.position + new Vector3(newX, newY, 0), Quaternion.identity);
            }
           
        }
        else if(buildTyep == BuildType.middle)
        {
            for (int i = 0; i < 8; i++)
            {
                int a = Random.Range(0, 5);
                float newY;
                float newX;
                if (i < 2)
                {
                    newY = Random.Range(0.5f, 1f);
                    newX = Random.Range(-0.8f, 0f);
                }
                else if (i < 4)
                {
                    newY = Random.Range(0.5f, 1f);
                    newX = Random.Range(0f, 0.8f);
                }
                else if (i < 6)
                {
                    newY = Random.Range(1f, 1.5f);
                    newX = Random.Range(-0.8f, 0f);
                }
                else
                {
                    newY = Random.Range(1f, 1.5f);
                    newX = Random.Range(0f, 0.8f);
                }
                Instantiate<GameObject>(explodeEffect[a], transform.position + new Vector3(newX, newY, 0), Quaternion.identity);
            }
        }
        else if (buildTyep == BuildType.inner)
        {
            for (int i = 0; i < 8; i++)
            {
                int a = Random.Range(0, 5);
                float newY;
                float newX;
                if (i < 2)
                {
                    newY = Random.Range(1f, 1.5f);
                    newX = Random.Range(-0.5f, 0f);
                }
                else if (i < 4)
                {
                    newY = Random.Range(1f, 1.5f);
                    newX = Random.Range(0f, 0.5f);
                }
                else if (i < 6)
                {
                    newY = Random.Range(1.5f, 2f);
                    newX = Random.Range(-0.5f, 0f);
                }
                else
                {
                    newY = Random.Range(1.5f, 2f);
                    newX = Random.Range(0f, 0.5f);
                }
                Instantiate<GameObject>(explodeEffect[a], transform.position + new Vector3(newX, newY, 0), Quaternion.identity);
            }
        }
    }

    IEnumerator doBlink(float blinkTime, float blinkNum)
    {
        for (int i = 0; i < blinkNum; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(blinkTime / blinkNum);
        }
        Destroy(gameObject);
    }
}
