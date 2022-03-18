using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject[] effects;

    public void Explosion(Vector3 position, float width, float hight, int effectNum, float duration)
    {
        StartCoroutine(Explod(position, width, hight, effectNum, duration));
    }

    IEnumerator Explod(Vector3 position, float width, float hight, int effectNum, float duration)
    {
        for (int i = 0; i < effectNum; i++)
        {
            int a = Random.Range(0, effects.Length);
            float newX;
            float newY;
            if (i < effectNum * 0.25)
            {
                newX = Random.Range(-width, 0);
                newY = Random.Range(0, hight);
            }
            else if (i < effectNum * 0.5)
            {
                newX = Random.Range(0, width);
                newY = Random.Range(0, hight);
            }
            else if (i < effectNum * 0.75)
            {
                newX = Random.Range(0, width);
                newY = Random.Range(-hight, 0);
            }
            else
            {
                newX = Random.Range(-width, 0);
                newY = Random.Range(-hight, 0);
            }
            Instantiate(effects[a], position + new Vector3(newX, newY, 0), Quaternion.identity);
            yield return new WaitForSeconds(duration / effectNum);
        }
    }
}
