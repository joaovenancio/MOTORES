using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //caminho onde vai guardar
    [SerializeField]
    private string _path;

    //nome do arquivo onde guarda
    [SerializeField]
    private string _arquiveName;

    [SerializeField]
    private SaveData _saveData;

    //funcao para guardar
    public void Save()
    {
        string data = JsonUtility.ToJson(_saveData);
        File.WriteAllText(_path, data);
    }

    //funcao para carregar
    public void Load()
    {
        if (File.Exists(_path))
        {
            // Debug.Log("ok");
            string data = File.ReadAllText(_path);
            _saveData = JsonUtility.FromJson<SaveData>(data);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //dado que se encontra dentro da classe
        _path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + _arquiveName + ".json";
            }

    // Update is called once per frame
    void Update()
    {
        
    }
}
