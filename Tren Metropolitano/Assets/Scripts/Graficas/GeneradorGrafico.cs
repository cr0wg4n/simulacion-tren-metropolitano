using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorGrafico : MonoBehaviour
{
    public GameObject bolita;
    public Graph graficador;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        graficador = new Graph(15, 6, new Vector2(-7.5f, -3f));
        float[] vector = new float[100];
        for (int i = 0; i < vector.Length; i++)
        {
            float n = Random.Range(0f, 100f);
            vector[i] = n;
        }
        List<List<Vector2>> grafo = graficador.generarListas(vector, 5f);
        foreach (var x in grafo)
        {
            foreach (var y in x)
            {
                Instantiate(bolita, y, Quaternion.identity);
            }
        }
    }
    void DestroyAll(string tag)
    {
        GameObject[] instances = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < instances.Length; i++)
        {
            GameObject.Destroy(instances[i]);
        }
    }
}
