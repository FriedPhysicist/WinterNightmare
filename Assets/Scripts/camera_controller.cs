using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour
{

    public float mouseSensivity=1f;
    public Transform playerBody;

    float xRotation;

    Vector3 start_pos;
    int current_vib;
    public int vib_public;
    public static bool vib_bool=false;
    bool can_go=true;

    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
        start_pos=transform.localPosition;
    }


    void Update()
    {
        float mouseX=Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;        
        float mouseY=Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;        

        xRotation-=mouseY;
        xRotation= Mathf.Clamp(xRotation, -90f,90f);

        transform.localRotation= Quaternion.Euler(xRotation,0f,0f);
        playerBody.Rotate(Vector3.up*mouseX); 

        if(vib_bool)
        {
            transform.localPosition+=new Vector3(Random.Range(-1,2),Random.Range(-1,2),Random.Range(-1,2));

            current_vib++;
        }

        if(current_vib>=vib_public)
        { 
            transform.localPosition=start_pos;
            vib_bool=false;
            current_vib=0;
        }

        if(charr_.go_ && can_go)
        {
            transform.localPosition=new Vector3(0,0,3); 
            can_go=false;
        }
    }
}
