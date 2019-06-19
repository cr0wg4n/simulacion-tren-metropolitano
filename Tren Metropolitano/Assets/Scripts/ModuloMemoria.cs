using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ModuloMemoria
{
    public string path = "/stadistics.gd";
    public void guardar(List<Estadistica> historial) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + path);
        bf.Serialize(file, historial);
        file.Close();
    }
    public List<Estadistica> cargar()
    {
        List<Estadistica> res = new List<Estadistica>();
        if (File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);
            res = (List<Estadistica>)bf.Deserialize(file);
            file.Close();
        }
        return res;
    }
    public void guardarNuevaEstadistica(Estadistica nuevo) {
        List<Estadistica> antiguo = cargar();
        antiguo.Add(nuevo);
        guardar(antiguo);
    }
    public void eliminar() {
        File.Delete(Application.persistentDataPath + path);
    }
}
