using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambiente : MonoBehaviour
{
    public GameObject estrella;
    void Start()
    {
        
    }
    void Update()
    {
    }
    public void rotarEstrella()
    {
        estrella.transform.localRotation *= Quaternion.Euler(0, 0, 180);
    }
}
