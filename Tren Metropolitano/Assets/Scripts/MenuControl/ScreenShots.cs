using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShots : MonoBehaviour
{
    [SerializeField]
    GameObject blink;
    public void tomarScreen()
    {
        StartCoroutine("captura");
    }
    IEnumerator captura() {
        string timestamp = System.DateTime.Now.ToString("dd-MM-yy-HH-mm-ss");
        string archivo = "Simulacion" + timestamp +".png";
        string directorio = archivo;
        ScreenCapture.CaptureScreenshot("../../../../DCIM/"+directorio);
        yield return new WaitForEndOfFrame();
       Destroy (Instantiate(blink, new Vector2(0f, 0f), Quaternion.identity),0.15f);
    }
}
