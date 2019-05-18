using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usuario : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject parada;
    public float velocidad = 0.4f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, parada.transform.position, velocidad * Time.deltaTime);
    }
}
