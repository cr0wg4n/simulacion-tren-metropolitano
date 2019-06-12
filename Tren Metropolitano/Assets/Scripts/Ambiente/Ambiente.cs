using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambiente : MonoBehaviour
{
    public Quaternion posInicialEstrella;
    public GameObject estrella;
    void Start()
    {
        posInicialEstrella = estrella.transform.localRotation;
    }
    void Update()
    {

    }
    public void noche() {
        estrella.transform.localRotation = posInicialEstrella;
    }
    public void dia() {
        estrella.transform.localRotation = posInicialEstrella;
        estrella.transform.localRotation *= Quaternion.Euler(0, 0, 180);
    }
}
