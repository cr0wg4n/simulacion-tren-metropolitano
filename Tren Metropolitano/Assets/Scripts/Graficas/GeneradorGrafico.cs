using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorGrafico : MonoBehaviour
{
    public GameObject bolita;
    public Graph graficador;
    void Start()
    {
        graficador = new Graph(16,6, new Vector2(-8f,-3f));
        float[] vector = new float[] { 12f, 10f, 5f, 1f, 13f, 100f };
        List<List<Vector2>> grafo = graficador.generarListas(vector, 100f);
        foreach (var x in grafo)
        {
            foreach (var y in x)
            {
                Instantiate(bolita, y , Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
