using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    float speed;
    [SerializeField]
    private Transform following;
    [SerializeField]
    private bool isFollowing = false;

    public static CameraControl Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (following == null)
            isFollowing = false;

        if(isFollowing == false)
        {
            PlayerControl();
        } else
        {
            FollowCreature();
        }
        Zoom();
    }

    private void PlayerControl()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        if (hInput != 0)
        {
            cam.transform.position = new Vector3(transform.position.x + (speed * hInput * (Time.deltaTime/Time.timeScale)), transform.position.y, transform.position.z);
        }

        if (vInput != 0)
        {
            cam.transform.position = new Vector3(transform.position.x, transform.position.y + (speed * vInput * (Time.deltaTime / Time.timeScale)), transform.position.z);
        }
    }

    public void SetFollow(Transform creature)
    {
        isFollowing = true;
        following = creature; 
    }

    public void StopFollow()
    {
        isFollowing = false;
    }
    private void FollowCreature()
    {

        cam.transform.position = new Vector3(following.position.x, following.position.y, transform.position.z);
    }

    private void Zoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0 && cam.orthographicSize > .3)
        {
            cam.orthographicSize -= .2f;
        }
        else if (scroll < 0 && cam.orthographicSize < 3)
        {
            cam.orthographicSize += .2f;
        }
    }

}
