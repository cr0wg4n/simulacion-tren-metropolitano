using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneradorDeUsuarios : MonoBehaviour
{
    private int hora = 6;
    private float minuto = 0;
    private int minutoInt = 0;
    public bool trenEnEstacion = false;
    public GameObject estacion;
    public float velocidadDeUsuariosBase = 15;
    private float velocidadActual;
    public GameObject ancianos;
    public GameObject universitarios;
    public GameObject niños;
    public GameObject adultos;
    public InputField inputMuestra;
    public Slider velocidadSlide;
    public List<GameObject> usuarios;
    private int numeroPersonasEstacion = 0;
    public Text cantidad;
    public Text horaEstación;
    public GameObject tren;
    public GameObject salidaIzq;
    public GameObject salidaDerecha;
    private int basePersonasLlegadas = 40;
    private int diferencialPersonas;
    //variables para colas
    public int tPromedioEntreLlegadas = 0;
    private int tPromedioEntreLlegadasBase = 100;

    //tabla exponencial (ejey)
    public List<double> probs;
    public List<int> tiempos;
    public List<GameObject> trenes;
    private bool demandaBandera = false;
    Horarios horarios = new Horarios();
    //tren timestamp
    private int minTimestamp=0;
    void controlHora() {
        if (minuto > 60) {
            hora++;
            minuto = 0;
        }
        //controla la hora de trabajo del tren
        if (hora >= 22) {
            hora = 0;
        }
        if (hora >= 5 && hora < 19)
        {
            this.GetComponent<Ambiente>().dia();
        }
        else
        {
            this.GetComponent<Ambiente>().noche();
        }
    }
    void actualizarPersonasLlegadas() {
        diferencialPersonas = basePersonasLlegadas - Mathf.RoundToInt (basePersonasLlegadas * velocidadSlide.value);
        generarPersonas(diferencialPersonas);
    }
    void Start()
    {
        diferencialPersonas = 0;
        generarPersonas(basePersonasLlegadas);
        generarTren();
    }

    void Update()
    {
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

        double rnd = (double)(Random.Range(0, 100))/(double)100;
        double aux = 0;

        for (int i=0 ; i<tiempos.Count; i++)
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
        minuto += (velocidadSlide.value*0.50f);

        //en el modulo esta cada cuantos minutos llega un tren
        if ((minutoInt % 25==0 && minutoInt != 0) && trenEnEstacion==false) {
           generarTren();
            minTimestamp = minutoInt;
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
        //if (trenEnEstacion==true && (minutoInt - minTimestamp == 15 || numeroPersonasEstacion>=200))
        if (trenEnEstacion==true && (minutoInt - minTimestamp) % 15== 0)
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
            Debug.Log(tiempos[i] + " - " + probs[i]);
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
            enHora(demanda.HoraIni, demanda.MinutoIni, demanda.HoraFin, demanda.MinutoFin);
            if (demandaBandera)
            {
                int dif = demanda.TEntrePasajeros - Mathf.RoundToInt(demanda.TEntrePasajeros * velocidadSlide.value);
                generarPersonas(dif);
                Debug.Log("Demanda: " + dif);
            } else {
                actualizarPersonasLlegadas();
                Debug.Log("Sin demanda: " + diferencialPersonas);
            }
        }
    }

    void enHora(int horaI, int minI, int horaF, int minF) {
        if (hora == horaI && minutoInt == minI)
        {
            demandaBandera = true;
        }
        if (hora == horaF && minutoInt == minF)
        {
            demandaBandera = false;
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
}
