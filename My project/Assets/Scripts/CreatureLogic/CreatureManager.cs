using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    private float LogTimer = 0;
    [SerializeField]
    private float LogRate = 60f;

    public static CreatureManager instance;

    private string FileName = "/Log.txt";

    [SerializeField]
    public TextAsset log;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        
    }

    void FixedUpdate(){
        if(LogTimer < LogRate){
            LogTimer += Time.deltaTime;
        }else{
            Debug.Log("Logging State");
            LogTimer = 0;
            LogCreatureStates();
        }
    }

    public void LogCreatureStates(){
        string path = Application.persistentDataPath + FileName;
        StreamWriter writer = new(path, true);
        List<BaseCreature> listOfCreatures = new List<BaseCreature>(gameObject.GetComponentsInChildren<BaseCreature>());
        float AvSpeed = 0, AvRange = 0;
        foreach(BaseCreature creature in listOfCreatures){
            AvRange += creature.data.SightRange;
            AvSpeed += creature.data.Speed;
        }
        writer.WriteLine("Log");
        writer.WriteLine("Average Speed" + AvSpeed / listOfCreatures.Count);
        writer.WriteLine("Average Range" + AvRange / listOfCreatures.Count);
        writer.Close();
    }
}
