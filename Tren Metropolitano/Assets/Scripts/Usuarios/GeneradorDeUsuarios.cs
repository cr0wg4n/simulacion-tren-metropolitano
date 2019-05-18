using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneradorDeUsuarios : MonoBehaviour
{
    public GameObject estacion;
    public int velocidadGeneracion=1;
    public GameObject ancianos;
    public GameObject universitarios;
    public GameObject niños;
    public GameObject adultos;
    public InputField inputMuestra;
    void Start()
    {
        
                   
    }
    void Update()
    {
        
    }
    Vector3 randomizePosition() {
        float x=Random.Range(-9.0f,9.0f);
        float y = Random.Range(-5.0f,5.0f);
        Vector3 res = new Vector3(x,y,0.0f);
        return res;
    }
    public void generarPersonas() {
        string personas = inputMuestra.text;
        int n = int.Parse(personas);
        for (int i = 0; i < n; i++)
        {
            int tipoPersonas = Random.Range(0, 4);
            switch (tipoPersonas)
            {
                case 0:
                    GameObject anc = Instantiate(ancianos, randomizePosition(), Quaternion.identity);
                    Usuario anci = anc.GetComponent<Usuario>();
                    anci.parada = estacion;
                    anci.velocidad = 0.1f * velocidadGeneracion;
                    break;
                case 1:
                    GameObject uni = Instantiate(universitarios, randomizePosition(), Quaternion.identity);
                    Usuario univ = uni.GetComponent<Usuario>();
                    univ.parada = estacion;
                    univ.velocidad = 0.3f * velocidadGeneracion;
                    break;
                case 2:
                    GameObject ni = Instantiate(niños, randomizePosition(), Quaternion.identity);
                    Usuario ninos = ni.GetComponent<Usuario>();
                    ninos.parada = estacion;
                    ninos.velocidad = 0.2f * velocidadGeneracion;
                    break;
                case 3:
                    GameObject adu = Instantiate(adultos, randomizePosition(), Quaternion.identity);
                    Usuario adul = adu.GetComponent<Usuario>();
                    adul.parada = estacion;
                    adul.velocidad = 0.25f * velocidadGeneracion;
                    break;
                default:
                    break;
            }

        }
    }
}
