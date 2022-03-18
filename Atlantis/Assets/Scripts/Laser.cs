using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Building"))
        {
            collision.gameObject.GetComponent<Building>().Hitten();
            Destroy(gameObject);
        }
        else if (collision .gameObject.CompareTag("Turret"))
        {
            collision.gameObject.GetComponent<Turret>().Hitten();
            Destroy(gameObject);
        }
    }
}
