using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usuario : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject parada;
    public float velocidad = 0.4f;
    public float aceleracion = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, parada.transform.position, aceleracion*velocidad * Time.deltaTime);
        if (parada.transform.position.x == transform.position.x && parada.transform.position.y == transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
