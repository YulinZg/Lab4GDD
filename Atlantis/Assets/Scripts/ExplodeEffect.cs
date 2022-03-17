using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEffect : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(DestroySelf), 0.5f);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
