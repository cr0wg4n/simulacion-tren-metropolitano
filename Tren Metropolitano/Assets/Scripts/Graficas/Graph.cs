using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public float MAX_X; //16
    public float MAX_Y; //6
    public Vector2 partida; // -8,-3

    public Graph(float MAX_X, float MAX_Y, Vector2 partida){
        this.partida = partida;
        this.MAX_X = MAX_X;
        this.MAX_Y = MAX_Y;
    }
    public List<List<Vector2>> generarListas(float[] valores , float reductor) {
        float maxValY = maxValorIteracion(valores);
        float offsetX = MAX_X / valores.Length;
        float offsetY = MAX_Y / (maxValY/reductor);
        List<List<Vector2>> largo = new List<List<Vector2>>();
       
        for (int i = 0; i < valores.Length; i++)
        {
            float x = partida.x;
            float y = partida.y;
            float maxLocalY = valorAbsoluto(MAX_Y, maxValY, valores[i]);
            int valorY = Mathf.RoundToInt(maxLocalY / offsetY);
            List<Vector2> ancho = new List<Vector2>();
            for (int j = 0; j < valorY; j++)
            {
                ancho.Add(new Vector2(x, y));
                y += offsetY;
            }
            largo.Add(ancho);
            partida.x += offsetX;
        }
        return largo;
    }
    float valorAbsoluto(float maxA, float maxL, float valorL) {
        float res = (maxA * valorL) / maxL;
        return res;
    }
    public float maxValorIteracion(float[] valY) {
        float res = valY[0];
        for (int i = 1; i < valY.Length; i++)
        {
            if (res <= valY[i]) {
                res = valY[i];
            }
        }
        return res;
    }
}
