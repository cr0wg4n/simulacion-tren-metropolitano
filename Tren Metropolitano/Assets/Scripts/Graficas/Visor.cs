using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visor : MonoBehaviour
{
    public GameObject captura1;
    public GameObject captura2;
    public GameObject captura3;
    public GameObject captura4;
    private int counter = 0;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        controlCounter();
        switch (counter)
        {
            case 0:
                eliminar();
                Instantiate(captura1, new Vector2(0f, 0f), Quaternion.identity);
                break;
            case 1:
                eliminar();
                Instantiate(captura2, new Vector2(0f, 0f), Quaternion.identity);
                break;
            case 2:
                eliminar();
                Instantiate(captura3, new Vector2(0f, 0f), Quaternion.identity);
                break;
            case 3:
                eliminar();
                Instantiate(captura4, new Vector2(0f, 0f), Quaternion.identity);
                break;
            default:
                break;
        }
    }
    void eliminar() {
        Destroy(GameObject.FindWithTag("captura"));
    }
    public void prev() {
        counter--;
    }
    public void next() {
        counter++;
    }
    void controlCounter() {
        if (counter < 0) {
            counter = 0;
        }
        if (counter > 3) {
            counter = 3;
        }
    }

}
