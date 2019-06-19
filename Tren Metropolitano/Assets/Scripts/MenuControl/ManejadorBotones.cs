using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManejadorBotones : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void estacionCentral() {
        EstacionEstatica.NombreEstacion = "Estacion Central";
        SceneManager.LoadScene("est_central");
    }
    public void estacionVerde()
    {
        EstacionEstatica.NombreEstacion = "Estacion Verde";
        SceneManager.LoadScene("est_central");
    }
    public void estacionRoja()
    {
        EstacionEstatica.NombreEstacion = "Estacion Roja";
        SceneManager.LoadScene("est_central");
    }
    public void estacionAmarilla()
    {
        EstacionEstatica.NombreEstacion = "Estacion Amarilla";
        SceneManager.LoadScene("est_central");
    }

    public void backToMenu() {
        SceneManager.LoadScene("Menu");
    }
    public void rutas() {
        SceneManager.LoadScene("rutas");
    }
    public void paradaRoja() {
        EstacionEstatica.NombreEstacion = "Parada Roja";
        SceneManager.LoadScene("est_paradas");
    }
    public void paradaAmarilla()
    {
        EstacionEstatica.NombreEstacion = "Parada Amarilla";
        SceneManager.LoadScene("est_paradas");
    }
    public void paradaVerde()
    {
        EstacionEstatica.NombreEstacion = "Parada Verde";
        SceneManager.LoadScene("est_paradas");
    }
}
