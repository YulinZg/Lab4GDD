using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject[] hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Disappear), 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += speed * Time.deltaTime * Vector3.up;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            int i = Random.Range(0, 5);
            collision.gameObject.GetComponent<Enemy>().Hit();
            Instantiate(hitEffect[i], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
