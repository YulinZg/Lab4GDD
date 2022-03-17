using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEffect : MonoBehaviour
{
    private void Start()
    {
        Invoke("destroySelf", 2.0f);
    }
    public void destroySelf()
    {
        Destroy(this.gameObject);
    }
}
