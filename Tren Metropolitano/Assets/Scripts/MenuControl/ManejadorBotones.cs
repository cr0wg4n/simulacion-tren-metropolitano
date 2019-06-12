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
    public void runEstCentral() {
        SceneManager.LoadScene("est_central");
    }
    public void backToMenu() {
        SceneManager.LoadScene("Menu");
    }
    public void paradaEscena() {
        SceneManager.LoadScene("est_paradas");
    }
}
