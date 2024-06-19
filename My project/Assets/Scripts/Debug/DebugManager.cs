using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;
    private BaseCreature _creature;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    CameraControl camcon;
    private bool info_opened = false;
    // Start is called before the first frame update
    //Hello Testing Testing
    void Start()
    {
        Instance = this;
        _canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _canvas.enabled = false;
            camcon.StopFollow();
        }
    }

    private void FixedUpdate()
    {
        if(_creature == null)
        {
            _canvas.enabled = false;
            return;
        }
        if(info_opened) {
            SetTextInfo();
        }
    }

    public void Display(BaseCreature _creature)
    {
        info_opened = true;
        this._creature = _creature;
        _canvas.enabled = true;
        camcon.SetFollow(_creature.GetTransform());
    }

    private void SetTextInfo()
    {
        //energy / max energy
        //age
        //sight
        //speed

        string stats = "";
        stats += "ID:\t" +_creature.data.ID + "\n";
        stats += "Energy:" +_creature.data.Current_energy + " / " + _creature.data.Energy + "\n";
        stats += "Age:\t" +_creature.GetAge() + "\n";
        stats += "Sight Range:" +_creature.data.Sight_range + "\n";
        stats += "Speed:"+_creature.data.Speed + "\n";
        stats += "Target Location: " + _creature.data.Target_Location + "\n";
        stats += "Current Location:" + _creature.transform.position + "\n";
        stats += "Path: " + _creature.data.path.Count + "\n";
        stats += "Action: " + _creature.current_action_node.action.ToString();

        
        _text.text = stats;
    }
}
