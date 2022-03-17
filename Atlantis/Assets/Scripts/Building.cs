using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject[] explodeEffect;
    [SerializeField] private BuildType buildType;

    private enum BuildType
    {
        side,
        middle,
        inner
    };


    public void HittingByEnemy()
    { 
        StartCoroutine(DoBlink(2, 40));
        if (buildType == BuildType.side)
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
                Instantiate(explodeEffect[a], transform.position + new Vector3(newX, newY, 0), Quaternion.identity);
            }
           
        }
        else if(buildType == BuildType.middle)
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
                Instantiate(explodeEffect[a], transform.position + new Vector3(newX, newY, 0), Quaternion.identity);
            }
        }
        else if (buildType == BuildType.inner)
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
                Instantiate(explodeEffect[a], transform.position + new Vector3(newX, newY, 0), Quaternion.identity);
            }
        }
    }

    IEnumerator DoBlink(float blinkTime, float blinkNum)
    {
        for (int i = 0; i < blinkNum; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(blinkTime / blinkNum);
        }
        Destroy(gameObject);
    }
}
