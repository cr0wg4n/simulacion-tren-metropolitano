using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GeneradorGrafico : MonoBehaviour
{
    public GameObject bolita;
    public Graph graficador;
    public Text ejeX;
    public Text ejeY;
    public Text titulo;
    private List<Estadistica> datos;
    public GameObject textoFinal;
    private ModuloMemoria memoria;
    void Start()
    {
        memoria = new ModuloMemoria();
        datos = memoria.cargar();
    }

    // Update is called once per frame
    void Update()
    {
        graficador = new Graph(15, 6, new Vector2(-7.5f, -3f));

        List<float> vector = new List<float>();

        List<Dato> d = datos[0].historial;
        for (int i = 0; i < datos.Count; i++)
        {
            for (int j = 0; j < datos[i].historial.Count; j++)
            {
                float n = datos[i].historial[j].dinero;
                vector.Add (n);
            }
        }
        List<List<Vector2>> grafo = graficador.generarListas(vector, 35f);
        foreach (var x in grafo)
        {
            int n = 0;
            Vector2 res=new Vector2();
            foreach (var y in x)
            {
                n++;
                if (n == x.Count)
                {
                    textoFinal.GetComponentInChildren<Text>().text=""+y.x;
                    Destroy (Instantiate(textoFinal, res, Quaternion.identity),0.1f);
                }
                else {
                    if (n != x.Count - 1) {
                        Instantiate(bolita, y, Quaternion.identity);
                    }
                }
                res = y;
            }
        }
    }
    public void eliminarDatos() {
        memoria.eliminar();
        datos = new List<Estadistica>();
    }
}
