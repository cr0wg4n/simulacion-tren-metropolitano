using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrenControl : MonoBehaviour
{
    public GameObject parada;
    public GameObject final;
    public int modo=0;

    public float velocidad = 2f;
    public float aceleracion = 1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (modo)
        {
            case 0:
                transform.position = Vector2.MoveTowards(transform.position, parada.transform.position, aceleracion * velocidad * Time.deltaTime);
                break;
            case 1:
                transform.position = Vector2.MoveTowards(transform.position, final.transform.position, aceleracion * velocidad * Time.deltaTime);
                break;
            default:
                break;
        }
    }
}
