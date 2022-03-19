using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject enemy3;
    public float interval { get; set; } = 1;

    private float timer;

    // Start is called before the first frame updat

    private void OnEnable()
    {
        timer = 0;
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    void SpawnEnemy()
    {
        int i = Random.Range(0, 10);
        int x = Random.Range(22, 24) % 23 - 11;
        if (i == 0)
            Instantiate(enemy3, new Vector3(x, Random.Range(0, 4.0f), 0), Quaternion.identity);
        else if (i < 5)
            Instantiate(enemy1, new Vector3(x, Random.Range(0, 4.0f), 0), Quaternion.identity);
        else
            Instantiate(enemy2, new Vector3(x, Random.Range(0, 4.0f), 0), Quaternion.identity);
    }
}
