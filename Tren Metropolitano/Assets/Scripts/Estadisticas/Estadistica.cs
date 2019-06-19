using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Estadistica
{
    public string nombreEstacion;
    public List<Dato> historial;
    public Estadistica(string nombre) {
       this.nombreEstacion = nombre;
       this.historial = new List<Dato>();
    }
}

[System.Serializable]
public class Dato {
    public int hora;
    public float dinero;
    public int personas;
    public Dato(int hora, float dinero)
    {
        this.hora = hora;
        this.dinero = dinero;
    }
    public Dato(int hora, float dinero, int personas)
    {
        this.hora = hora;
        this.personas = personas;
        this.dinero = dinero;
    }
}
