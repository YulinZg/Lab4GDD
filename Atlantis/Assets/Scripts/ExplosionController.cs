using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject[] effects;

    public void Explosion(Vector3 position, float width, float hight, float time)
    {
        for (int i = 0; i < 8; i++)
        {
            int a = Random.Range(0, effects.Length);
            float newY;
            float newX;
            if (i < 2)
            {
                newY = Random.Range(-width, 0);
                newX = Random.Range(0, hight);
            }
            else if (i < 4)
            {
                newY = Random.Range(0, -width);
                newX = Random.Range(0, hight);
            }
            else if (i < 6)
            {
                newY = Random.Range(0, -width);
                newX = Random.Range(-hight, 0);
            }
            else
            {
                newY = Random.Range(-width, 0);
                newX = Random.Range(-hight, 0);
            }
            Instantiate(effects[a], position + new Vector3(newX, newY, 0), Quaternion.identity);
        }
    }
}
