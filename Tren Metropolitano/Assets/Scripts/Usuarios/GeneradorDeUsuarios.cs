using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GeneradorDeUsuarios : MonoBehaviour
{
    private int personasAtendidas=0;
    private float dinero=0;
    private float dineroProm;
    private int ultimaHora;
    private int hora=6;
    private float minuto=0;
    private int minutoInt = 0;
    public bool trenEnEstacion = false;
    public GameObject estacion;
    public float velocidadDeUsuariosBase=20;
    private float velocidadActual;
    private int personasAbordo;
    public GameObject ancianos;
    public GameObject universitarios;
    public GameObject niños;
    public GameObject adultos;

    public GameObject ancianosSalidas;
    public GameObject universitariosSalidas;
    public GameObject niñosSalidas;
    public GameObject adultosSalidas;

    public Dropdown selectLlegadas;
    public Dropdown selectDistribucion;
    public Slider velocidadSlide;
    public List<GameObject> usuarios;
    public List<GameObject> posSalidas;

    private int numeroPersonasEstacion=0;
    public Text cantidad;
    public Text horaEstación;
    public Text horaDemanda;
    public Text dineroPromedio;
    public Text nombreEstacion;
    public GameObject tren;
    public GameObject salidaIzq;
    public GameObject salidaDerecha;

    private int basePersonasLlegadas = 28;
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

    private int minTimestamp = 0;
    Estadistica estadistica= new Estadistica(EstacionEstatica.NombreEstacion);

    void Start()
    {
        diferencialPersonas = 0;
        generarPersonasSegunDistribucion(basePersonasLlegadas);
        generarTren();
        posSalidas = generarVectoresdeSalida(200);
        nombreEstacion.text = EstacionEstatica.NombreEstacion;
    }

    void Update()
    {
        //hora de la estacion
        controlHora();
        //control de la demanda
        controlDeDemanda();
        //control de dinero
        actualizarDinero();
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
        generadorDePersonas();
        salidasAleatorias();

        minuto += (velocidadSlide.value*0.50f);

        //en el modulo esta cada cuantos minutos llega un tren
        if ((minutoInt % 25==0 && minutoInt != 0) && trenEnEstacion==false) {
           generarTren();
            minTimestamp = minutoInt;
            minuto++;
        }
    }
    void salidasAleatorias() {
        if (numeroPersonasEstacion > 10) {
            generarSalidasporTiempo(60);
        }
    }
    void generarSalidasporTiempo(int max) {
        int n = Random.Range(0,max);
        if (minutoInt % n==0) {
            minuto++;
            int sal = Random.Range(0, 5);
            generarSalidas(sal,true);
            numeroPersonasEstacion -= sal;
        }
    }
    void controlHora()
    {
        ultimaHora = hora;
        if (minuto > 60)
        {
            hora++;
            minuto = 0;
        }
        if (hora >= 22) //hora para finalizar 10PM
        {
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
    void actualizarPersonasLlegadas()
    {
        diferencialPersonas = basePersonasLlegadas - Mathf.RoundToInt(basePersonasLlegadas * velocidadSlide.value);
        generarPersonasSegunDistribucion(diferencialPersonas);
    }
    void generarPersonasSegunDistribucion(int n)
    {
        int valSelect = selectDistribucion.value;
        switch (valSelect)
        {
            case 0:
                generarPersonasExponencial(n);
                //Debug.Log("distribucion exponencial");
                break;
            case 1:
                generarPersonasPoisson(n);
                //Debug.Log("distribucion poisson");
                break;
        }
    }
    void generarTren() {
        personasAbordo = Random.Range(100, 200);
        GameObject trenGenerado = Instantiate(tren, salidaIzq.transform.position, Quaternion.identity);
        TrenControl trencito = trenGenerado.GetComponent<TrenControl>();
        trencito.parada = estacion;
        trencito.final = salidaDerecha;
        trencito.personas = personasAbordo;
        trenes.Add(trenGenerado);
    }
    void actualizarDinero() {
        if (ultimaHora != hora) {
            dineroProm = dinero;
            Dato dato = new Dato(hora, dineroProm, personasAtendidas);
            estadistica.historial.Add(dato);
            dineroPromedio.text = "" + dineroProm + " Bs.";
            dinero = 0;
            personasAtendidas = 0;
        }
    }
    List<GameObject> generarVectoresdeSalida(int n) {
        List<GameObject> res = new List<GameObject>();
        for (int i = 0; i < n; i++)
        {

            int rn = Random.Range(0, 4);
            float x = 0f;
            float y = 0f;
            switch (rn)
            {
                case 0:
                    x = Random.Range(-9f, 9f);
                    y = Random.Range(5f, 6f);
                    break;
                case 1:
                    x = Random.Range(9f, 10f);
                    y = Random.Range(-5f, 5f);
                    break;
                case 2:
                    x = Random.Range(-9f, 9f);
                    y = Random.Range(-5f, -6f);
                    break;
                case 3:
                    x = Random.Range(-9f, -10f);
                    y = Random.Range(-5f, 5f);
                    break;
                default:
                    break;
            }
            Vector2 vectorRandom = new Vector3(x, y);
            GameObject salida = new GameObject();
            salida.transform.position = vectorRandom;
            res.Add(salida);
        }
        return res;
    }
    void generarSalidas(int salidas, bool flag) {
        for (int i = 0; i < salidas; i++)
        {
            int rn = Random.Range(0, 200);
            int n = Random.Range(0, 4);
            switch (n)
            {
                case 0:
                    GameObject anc = Instantiate(ancianosSalidas, new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f)) , Quaternion.identity);
                    Usuario anci = anc.GetComponent<Usuario>();
                    anci.parada = posSalidas[rn];
                    anci.velocidad = 0.15f;
                    anci.aceleracion = 2f;
                    Destroy(anc, 15f);
                    if (flag)
                    {
                        dinero -= Tarifas.anciano;
                    }
                    break;
                case 1:
                    GameObject uni = Instantiate(universitariosSalidas, new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f)), Quaternion.identity);
                    Usuario univ = uni.GetComponent<Usuario>();
                    univ.parada = posSalidas[rn];
                    univ.velocidad = 0.3f;
                    univ.aceleracion = 2f;
                    Destroy(uni, 15f);
                    if (flag) {
                        dinero -= Tarifas.universitario;
                    }
                    break;
                case 2:
                    GameObject ni = Instantiate(niñosSalidas, new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f)), Quaternion.identity);
                    Usuario ninos = ni.GetComponent<Usuario>();
                    ninos.parada = posSalidas[rn];
                    ninos.velocidad = 0.2f;
                    ninos.aceleracion = 2f;
                    Destroy(ni, 15f);
                    if (flag)
                    {
                        dinero -= Tarifas.nino;
                    }
                    break;
                case 3:
                    GameObject adu = Instantiate(adultosSalidas, new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f)), Quaternion.identity);
                    Usuario adul = adu.GetComponent<Usuario>();
                    adul.parada = posSalidas[rn];
                    adul.velocidad = 0.25f;
                    adul.aceleracion = 2f;
                    Destroy(adu, 15f);
                    if (flag)
                    {
                        dinero -= Tarifas.adulto;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    void moverTrenDerecha() {
        int n = 0;
        //cada cuanto sale un tren esta en el modulo
        //if (trenEnEstacion==true && (minutoInt - minTimestamp == 15 || numeroPersonasEstacion>=200))
        if (trenEnEstacion == true && (minutoInt - minTimestamp) % 15 == 0)
        {
            foreach (var tren in trenes)
            {
                try
                {
                    TrenControl trenItem = tren.GetComponent<TrenControl>();
                    trenItem.modo = 1;
                    if (numeroPersonasEstacion >= 200)
                    {
                        trenItem.personas = 200;
                        numeroPersonasEstacion -= 200;
                        personasAtendidas += 200;
                    }
                    else {
                        trenItem.personas = numeroPersonasEstacion;
                        personasAtendidas += numeroPersonasEstacion;
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
    public void generarPersonasExponencial(int n)
    {
        probs = new List<double>();
        tiempos = new List<int>();
        
        double tasaDeLlegadas = 1.00/(double)n; // Pasajeros por unidad de tiempo
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
    }
    public void generarPersonasPoisson(int n)
    {
        probs = new List<double>();
        tiempos = new List<int>();
        
        double tasaDeLlegadas = 1.00 / (double)n; // Pasajeros por unidad de tiempo
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
    float periodosSeleccionado()
    {
        float res=0;
        int valSelect = selectLlegadas.value;
        switch (valSelect)
        {
            case 0:
                res = 1f;
                //Debug.Log("periodo laboral");
                break;
            case 1:
                res = 2f;
                //Debug.Log("periodo vacacional");
                break;
            case 2:
                res = 1f;
                //Debug.Log("dia festivo");
                break;
        }
        return res;
    }
    void controlDeDemanda() {
        foreach (var demanda in horarios.demanda)
        {
            enHora(demanda.HoraIni, demanda.MinutoIni, demanda.HoraFin, demanda.MinutoFin);
            if (demandaBandera)
            {
                int dif = demanda.TEntrePasajeros - Mathf.RoundToInt(demanda.TEntrePasajeros * velocidadSlide.value);
                float ajuste = periodosSeleccionado();
                dif = Mathf.RoundToInt (dif * ajuste);
                generarPersonasSegunDistribucion(dif);
                horaDemanda.color = Color.yellow;
                horaDemanda.text = "Hora pico";
                //Debug.Log("Demanda: " + dif);
            }
            else
            {
                actualizarPersonasLlegadas();
                horaDemanda.color = Color.green;
                horaDemanda.text = "Hora Normal";
                //Debug.Log("Sin demanda: " + diferencialPersonas);
            }
        }
    }
    void enHora(int horaI, int minI, int horaF, int minF)
    {
        if (hora == horaI && minutoInt == minI)
        {
            demandaBandera = true;
        }
        if (hora == horaF && minutoInt == minF)
        {
            demandaBandera = false;
        }
    }
    public void generadorDePersonas()
    {
        double rnd = (double)(Random.Range(0, 100)) / (double)100;
        double aux = 0;

        for (int i = 0; i < tiempos.Count; i++)
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "persona_univ":
                numeroPersonasEstacion++;
                dinero += Tarifas.universitario;
                break;
            case "persona_nin":
                numeroPersonasEstacion++;
                dinero += Tarifas.nino;
                break;
            case "persona_adul":
                numeroPersonasEstacion++;
                dinero += Tarifas.adulto;
                break;
            case "persona_abue":
                numeroPersonasEstacion++;
                dinero += Tarifas.anciano;
                break;
            default:
                break;
        }
        if (collision.gameObject.tag == "tren")
        {
            generarSalidas(personasAbordo, false);
            //Debug.Log("salidas");
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
    public void guardarCosto() {
        ModuloMemoria memoria = new ModuloMemoria();
        memoria.guardarNuevaEstadistica(estadistica);
        SceneManager.LoadScene("Graph");
    }
}
