using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneradorDeUsuarios : MonoBehaviour
{
    public GameObject estacion;
    public float velocidadDeUsuariosBase=20;
    private float velocidadActual;
    public GameObject ancianos;
    public GameObject universitarios;
    public GameObject niños;
    public GameObject adultos;
    public InputField inputMuestra;
    public Slider velocidadSlide;
    public List<GameObject> usuarios;
    private int numeroPersonasEstacion=0;
    public Text cantidad;
    public GameObject tren;
    public GameObject salidaIzq;
    public GameObject salidaDerecha;
    //instancias del tren
    private GameObject InstanciaTren;
    private TrenControl trencito;

    //variables para colas
    public int tPromedioEntreLlegadas = 0;
    //tabla exponencial (ejey)
    public List<double> probs;
    public List<int> tiempos;

    int number = 0;
    bool couroutineStarted = false;

    void Start()
    {
        InstanciaTren = Instantiate(tren, salidaIzq.transform.position, Quaternion.identity);
        trencito = InstanciaTren.GetComponent<TrenControl>();
        trencito.parada = estacion;
        trencito.final = salidaDerecha;
    }
    void Update()
    {
        velocidadActual = velocidadDeUsuariosBase * velocidadSlide.value;
        actualizarVelocidad();
        cantidad.text = "Personas en la Estación: "+numeroPersonasEstacion;
        moverTrenDerecha();

        double rnd = (double)(Random.Range(0, 100))/(double)100;

        double aux = 0;
        for (int i=0 ; i<tiempos.Count ; i++)
        {
            if (rnd > aux && rnd < probs[i])
            {
                int tipoPersonas = Random.Range(0, 4);
                switch (tipoPersonas)
                {
                    case 0:
                        GameObject anc = Instantiate(ancianos, randomizePosition(), Quaternion.identity);
                        Usuario anci = anc.GetComponent<Usuario>();
                        anci.parada = estacion;
                        anci.velocidad = 0.15f;
                        usuarios.Add(anc);
                        break;
                    case 1:
                        GameObject uni = Instantiate(universitarios, randomizePosition(), Quaternion.identity);
                        Usuario univ = uni.GetComponent<Usuario>();
                        univ.parada = estacion;
                        univ.velocidad = 0.3f;
                        usuarios.Add(uni);
                        break;
                    case 2:
                        GameObject ni = Instantiate(niños, randomizePosition(), Quaternion.identity);
                        Usuario ninos = ni.GetComponent<Usuario>();
                        ninos.parada = estacion;
                        ninos.velocidad = 0.2f;
                        usuarios.Add(ni);
                        break;
                    case 3:
                        GameObject adu = Instantiate(adultos, randomizePosition(), Quaternion.identity);
                        Usuario adul = adu.GetComponent<Usuario>();
                        adul.parada = estacion;
                        adul.velocidad = 0.25f;
                        usuarios.Add(adu);
                        break;
                    default:
                        break;
                }

                // AQUI DELAY CON: tiempos[i]
            }
            aux = probs[i];
        }
    }

    void moverTrenDerecha() {
        if (numeroPersonasEstacion >= 200) {
            trencito.modo = 1;
        }
    }
    Vector3 randomizePosition() {
        float x=Random.Range(-10.0f,10.0f);
        int r = Random.Range(0, 2);
        float y = 0f;
        if (r == 1)
        {
            y = Random.Range(-6.0f, -1.3f);
        }
        else {
            y = Random.Range(1.3f, 6.0f);
        }
        Vector3 res = new Vector3(x,y,0.0f);
        return res;
    }
    void actualizarVelocidad()
    {
        int n=0;
            foreach (var usuario in usuarios)
            {
                try
                {
                    Usuario usuarioDelTren = usuario.GetComponent<Usuario>();
                    usuarioDelTren.aceleracion = velocidadActual;
                }
                catch (System.Exception)
                {
                    usuarios.RemoveAt(n);
                    throw;
                }
                n++;
            }
    }
    public void generarPersonas()
    {
        probs = new List<double>();
        tiempos = new List<int>();

        tPromedioEntreLlegadas = int.Parse(inputMuestra.text);
        double tasaDeLlegadas = 1.00/(double)tPromedioEntreLlegadas; // Pasajeros por unidad de tiempo
        double prob = 0;
        for(int i=1; i<=100; i++) //generando tabla exponencial con x -> tiempos entre llegadas, y -> probabilidades del tiempo
        {
            prob = tasaDeLlegadas * Mathf.Pow((float)2.71828, (float)tasaDeLlegadas * i);
            if (prob <= 1)
            {
                tiempos.Add(i);
                probs.Add(prob);
            }
        }

        for (int i = 0; i < tiempos.Count; i++)
        {
            print(tiempos[i] + " - " + probs[i]);
        }

        //string personas = inputMuestra.text;
        //int n = int.Parse(personas);
        //for (int i = 0; i < n; i++)
        //{
        //    int tipoPersonas = Random.Range(0, 4);
        //    switch (tipoPersonas)
        //    {
        //        case 0:
        //            GameObject anc = Instantiate(ancianos, randomizePosition(), Quaternion.identity);
        //            Usuario anci = anc.GetComponent<Usuario>();
        //            anci.parada = estacion;
        //            anci.velocidad = 0.15f;
        //            usuarios.Add(anc);
        //            break;
        //        case 1:
        //            GameObject uni = Instantiate(universitarios, randomizePosition(), Quaternion.identity);
        //            Usuario univ = uni.GetComponent<Usuario>();
        //            univ.parada = estacion;
        //            univ.velocidad = 0.3f;
        //            usuarios.Add(uni);
        //            break;
        //        case 2:
        //            GameObject ni = Instantiate(niños, randomizePosition(), Quaternion.identity);
        //            Usuario ninos = ni.GetComponent<Usuario>();
        //            ninos.parada = estacion;
        //            ninos.velocidad = 0.2f ;
        //            usuarios.Add(ni);
        //            break;
        //        case 3:
        //            GameObject adu = Instantiate(adultos, randomizePosition(), Quaternion.identity);
        //            Usuario adul = adu.GetComponent<Usuario>();
        //            adul.parada = estacion;
        //            adul.velocidad = 0.25f;
        //            usuarios.Add(adu);
        //            break;
        //        default:
        //            break;
        //    }

        //}
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "persona")
        {
            numeroPersonasEstacion++;
        }
    }
}
