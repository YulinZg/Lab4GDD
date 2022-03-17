using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;

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
            collision.gameObject.GetComponent<Enemy>().Hit();
            Destroy(gameObject);
        }
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
