using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class GeneradorDeUsuarios : MonoBehaviour
{
    private int hora=6;
    private float minuto=0;
    private int minutoInt = 0;
    public bool trenEnEstacion = false;
    public GameObject estacion;
    public float velocidadDeUsuariosBase=20;
    private float velocidadActual;
    public GameObject ancianos;
    public GameObject universitarios;
    public GameObject niños;
    public GameObject adultos;
    //public InputField inputMuestra;
    public Dropdown selectDistribución;
    public Dropdown selectLlegadas;
    public Slider velocidadSlide;
    public List<GameObject> usuarios;
    private int numeroPersonasEstacion=0;
    public Text cantidad;
    public Text horaEstación;
    public GameObject tren;
    public GameObject salidaIzq;
    public GameObject salidaDerecha;
    Timer timer = new Timer();

    //variables para colas
    public int tPromedioEntreLlegadas = 0;
    private int tPromedioEntreLlegadasBase = 100;

    //tabla exponencial (ejey)
    public List<double> probs;
    public List<int> tiempos;
    public List<GameObject> trenes;

    Horarios horarios = new Horarios();

    void controlHora() {
        if (minuto > 60) {
            hora++;
            minuto = 0;
        }
        if (hora >= 24) {
            hora = 0;
        }
    }

    void Start()
    {
        if (selectDistribución.value == 1)
        {
            generarPersonasExponencial();
        } else if(selectDistribución.value == 2)
        {
            generarPersonasPoisson();
        }
        generarTren();
    }

    void Update()
    {
        //print(selectLlegadas.value);

        //hora de la estacion
        controlHora();
        //control de la demanda
        controlDeDemanda();
        minutoInt = Mathf.RoundToInt(minuto);
        horaEstación.text = horarios.formatoHora(hora,minutoInt);
        //actualizacion de la velocidad
        velocidadActual = velocidadDeUsuariosBase * velocidadSlide.value;
        actualizarVelocidadPersonas();
        actualizarVelocidadTren();
        //conteo de pesonas en la estación
        cantidad.text = "Personas en la Estación: "+numeroPersonasEstacion;
        //tren pendiente a llenarse
        moverTrenDerecha();
        

        if (selectDistribución.value == 1)
        {
            print("exponencial");
            // GENERADOR EXPONENCIAL
            generarPersonasExponencial();
            generadorExponencial();
        }
        else if (selectDistribución.value == 2)
        {
            print("poison");
            // GENERADOR POISSON
            generarPersonasPoisson();
            generadorPoisson();
        }


        minuto += (velocidadSlide.value*0.50f);

        //en el modulo esta cada cuantos minutos llega un tren
        if ((minutoInt % 25==0 && minutoInt != 0) && trenEnEstacion==false) {
           generarTren();
           minuto ++;
        }
    }
    void generarTren() {
        GameObject trenGenerado = Instantiate(tren, salidaIzq.transform.position, Quaternion.identity);
        TrenControl trencito = trenGenerado.GetComponent<TrenControl>();
        trencito.parada = estacion;
        trencito.final = salidaDerecha;
        trenes.Add(trenGenerado);
    }
    void moverTrenDerecha() {
        int n = 0;
        //cada cuanto sale un tren esta en el modulo
        if (trenEnEstacion==true && (minutoInt % 20==0 || numeroPersonasEstacion>=200))
        {
            foreach (var tren in trenes)
            {
                try
                {
                    TrenControl trenItem = tren.GetComponent<TrenControl>();
                    trenItem.modo = 1;
                    if (numeroPersonasEstacion >= 200)
                    {
                        numeroPersonasEstacion -= 200;
                    }
                    else {
                        numeroPersonasEstacion = 0;
                    }
                    minuto += 1;
                }
                catch (System.Exception)
                {
                        trenes.RemoveAt(n);
                        throw;
                }
                n++;
            }
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
    void actualizarVelocidadPersonas()
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
    void actualizarVelocidadTren()
    {
        int n = 0;
        foreach (var tren in trenes)
        {
            try
            {
                TrenControl trenControl = tren.GetComponent<TrenControl>();
                trenControl.aceleracion = velocidadActual;
            }
            catch (System.Exception)
            {
                trenes.RemoveAt(n);
                throw;
            }
            n++;
        }
    }

    public void generarPersonasExponencial()
    {
        probs = new List<double>();
        tiempos = new List<int>();

        int valSelect = selectLlegadas.value;
        switch (valSelect)
        {
            case 0:
                tPromedioEntreLlegadas = 23;
                break;
            case 1:
                tPromedioEntreLlegadas = 45;
                break;
            case 2:
                tPromedioEntreLlegadas = 2;
                break;
        }

        double tasaDeLlegadas = 1.00/(double)tPromedioEntreLlegadas; // Pasajeros por unidad de tiempo
        double prob = 0;
        for(int i=1; i<=100; i++) //generando tabla exponencial con x -> tiempos entre llegadas, y -> probabilidades del tiempo
        {
            prob = tasaDeLlegadas * Mathf.Pow((float)2.71828, (float)(-tasaDeLlegadas) * i);
            if (prob <= 1)
            {
                tiempos.Add(i);
                probs.Add(prob);
                //print("print exponencial: " + tiempos[i-1] + " - " + probs[i-1]);
            }
        }
        //for (int i = 0; i < tiempos.Count; i++)
        //{
            //Debug.Log(tiempos[i] + " - " + probs[i]);
        //}
       
    }

    public void generarPersonasPoisson()
    {
        probs = new List<double>();
        tiempos = new List<int>();
        
        switch (tPromedioEntreLlegadas)
        {
            case 0:
                tPromedioEntreLlegadas = 23;
                break;
            case 1:
                tPromedioEntreLlegadas = 45;
                break;
            case 2:
                tPromedioEntreLlegadas = 2;
                break;
        }

        double tasaDeLlegadas = 1.00 / (double)tPromedioEntreLlegadas; // Pasajeros por unidad de tiempo
        double prob = 0;
        for (int i = 1; i <= 100; i++) //generando tabla exponencial con x -> tiempos entre llegadas, y -> probabilidades del tiempo
        {
            prob = tasaDeLlegadas * ( Mathf.Pow((float)2.71828, (float)(-i)) * ((float)5 * Mathf.Pow((float)i, (float)2)) ) / 2;
            if (prob <= (float)1)
            {
                tiempos.Add(i);
                probs.Add(prob);
            }
        }
    }

    public void generarPersonas(int n)
    {
        probs = new List<double>();
        tiempos = new List<int>();
        double tasaDeLlegadas = 1.00 / (double) n; // Pasajeros por unidad de tiempo
        double prob = 0;
        for (int i = 1; i <= 100; i++) //generando tabla exponencial con x -> tiempos entre llegadas, y -> probabilidades del tiempo
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
            //Debug.Log(tiempos[i] + " - " + probs[i]);
        }

    }
    void controlDeDemanda() {
        foreach (var demanda in horarios.demanda)
        {
            if (demanda.HoraIni == hora && demanda.MinutoIni == minutoInt)
            {
                generarPersonas(demanda.TEntrePasajeros);
                //Debug.Log("demanda");
            }
            if (demanda.HoraFin == hora && demanda.MinutoFin == minutoInt) {
                generarPersonas(40);
                //Debug.Log("sin demanda");
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "persona")
        {
            numeroPersonasEstacion++;
        }
        if (collision.gameObject.tag == "tren") {
            trenEnEstacion = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "tren")
        {
            trenEnEstacion = false;
        }
    }

    public void generadorExponencial()
    {
        double rnd = (double)(Random.Range(0, 100)) / (double)100;
        double aux = 0;

        for (int i = 0; i < tiempos.Count; i++)
        {
            //print(aux + " - " + rnd + " - " + probs[i]);
            if (rnd > aux && rnd < probs[i]*1.4)
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

                // Salidas del tren
                double rndS = (double)(Random.Range(0, 100)) / (double)100;
                if (rndS < 0.05 && numeroPersonasEstacion > 1)
                {
                    numeroPersonasEstacion--;
                }

                // AQUI DELAY CON: tiempos[i]
            }
            aux = probs[i];
        }
    }

    public void generadorPoisson()
    {
        double rnd = (double)(Random.Range(0, 100)) / (double)100;
        double aux = 0;

        for (int i = 0; i < tiempos.Count; i++)
        {
            if (rnd > aux && rnd < probs[i]*7)
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

                // Salidas del tren
                double rndS = (double)(Random.Range(0, 100)) / (double)100;
                if (rndS < 0.05 && numeroPersonasEstacion > 1)
                {
                    numeroPersonasEstacion--;
                }
            }
            aux = probs[i]*100;

            // AQUI DELAY CON: tiempos[i]
        }
    }
}
