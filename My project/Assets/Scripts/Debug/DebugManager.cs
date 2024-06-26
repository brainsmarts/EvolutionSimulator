using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;
    private BaseCreature creature;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    CameraControl camcon;
    private bool infoOpened = false;
    // Start is called before the first frame update
    //Hello Testing Testing
    void Start()
    {
        Instance = this;
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.enabled = false;
            camcon.StopFollow();
        }
    }

    private void FixedUpdate()
    {
        if(creature == null)
        {
            canvas.enabled = false;
            return;
        }
        if(infoOpened) {
            SetTextInfo();
        }
    }

    public void Display(BaseCreature _creature)
    {
        infoOpened = true;
        this.creature = _creature;
        canvas.enabled = true;
        camcon.SetFollow(_creature.GetTransform());
    }

    private void SetTextInfo()
    {
        //energy / max energy
        //age
        //sight
        //speed

        string stats = "";
        stats += "Path: " + _creature.data.path.Count + "\n";
        stats += "ID:\t" +creature.data.ID + "\n";
        stats += "Energy:" +creature.data.CurrentEnergy + " / " + creature.data.Energy + "\n";
        stats += "Age:\t" +creature.GetAge() + "\n";
        stats += "Sight Range:" +creature.data.SightRange + "\n";
        stats += "Speed:"+creature.data.Speed + "\n";
        stats += "Target Location: " + creature.data.TargetLocation + "\n";
        stats += "Current Location:" + creature.transform.position + "\n";
        stats += "Action: " + creature.currentActionNode.action.ToString();

        
        text.text = stats;
    }
}
