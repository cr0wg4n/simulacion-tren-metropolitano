using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horarios 
{
    public Tiempo[] demanda = new Tiempo[] {
       new Tiempo(7,30,8,0,12),
       new Tiempo(12,0,13,0,13),
       new Tiempo(18,30,19,30,14)
    };
    public int minutosEntreSalidas = 10;
    public string[] dias = new string[] {
        "Lunes",
        "Martes",
        "Miercoles",
        "Jueves",
        "Viernes",
        "Sábado"
    };
    public string[] meses = new string[] {
        "Enero",
        "Febrero",
        "Marzo",
        "Abril",
        "Mayo",
        "Junio",
        "Julio",
        "Agosto",
        "Septiembre",
        "Octubre",
        "Noviembre",
        "Diciembre"
    };
    public Horarios() {

    }
    public string formatoHora(int h,int m) {
        string hr = "";
        string min = "";
        string res = "";
        if (h < 10)
        {
            hr = "0" + h;
        }
        else {
            hr = "" + h;
        }
        if (m < 10)
        {
            min = "0" + m;
        }
        else {
            min = "" + m;
        }
        if (h >= 12 && h<24) {
            res = hr + ":" + min + " PM";
        } else if (h>=0 && h < 12) {
            res = hr + ":" + min + " AM";
        }
        return res;
    }
 }
public class Tiempo {
    private int horaIni;
    private int minutoIni;
    private int horaFin;
    private int minutoFin;
    private int tEntrePasajeros;

    public Tiempo(int horaIni, int minutoIni) {
        this.horaIni = horaIni;
        this.minutoIni = minutoIni;
    }
    public Tiempo(int horaIni, int minutoIni,int horaFin,int minutoFin)
    {
        this.horaIni = horaIni;
        this.minutoIni = minutoIni;
        this.horaFin = horaFin;
        this.minutoFin = minutoFin;
    }
    public Tiempo(int horaIni, int minutoIni, int horaFin, int minutoFin, int tEntrePasajeros)
    {
        this.horaIni = horaIni;
        this.minutoIni = minutoIni;
        this.horaFin = horaFin;
        this.minutoFin = minutoFin;
        this.tEntrePasajeros = tEntrePasajeros;
    }
    public int HoraIni {
        get { return horaIni; }
        set { horaIni = value; }
    }
    public int MinutoIni {
        get { return minutoIni; }
        set { minutoIni = value; }
    }
    public int HoraFin
    {
        get { return horaFin; }
        set { horaFin = value; }
    }
    public int MinutoFin
    {
        get { return minutoFin; }
        set { minutoFin = value; }
    }
    public int TEntrePasajeros {
        get { return tEntrePasajeros; }
        set { tEntrePasajeros = value; }
    }
}
