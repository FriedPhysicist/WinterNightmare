using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class charr_ : MonoBehaviour
{
    
    public Rigidbody rb; 
    public Animator anim;
    public float speed;

    public Camera cam; 
    Ray ray;
    RaycastHit hit;
    public static Vector3 camera_to_world;

    float x_axis;
    float z_axis;
    float jump;
    Vector3 move_dir;

    public Transform snow_ball_spawn_point;
    public GameObject snow_ball; 
    public AudioSource _as;
    public AudioClip[] _ac;
    public RawImage image;
    public GameObject bed;
    public static bool go_=false;






    void Start()
    { 
    }


    void Update()
    { 
        if(image.GetComponent<RectTransform>().sizeDelta.x <=5000 && monster_.death && Vector3.Distance(transform.position,bed.transform.position)>=5f) image.GetComponent<RectTransform>().sizeDelta*=1.01f;
        if(image.GetComponent<RectTransform>().sizeDelta.x>=5000 && monster_.death)
        {
            transform.position=bed.transform.position;
            //transform.rotation=Quaternion.Euler(90,0,0);
            go_=true;
        }

        if(Vector3.Distance(transform.position,bed.transform.position)<=5f && image.GetComponent<RectTransform>().sizeDelta.x>=15) 
        {
            image.GetComponent<RectTransform>().sizeDelta/=1.01f;
        }

        x_axis=Input.GetAxis("Horizontal");
        z_axis=Input.GetAxis("Vertical");
        jump=Input.GetAxis("Jump");

        //return camera point to world point
        camera_to_world=throw_point();

        Input_s();
        anim.SetBool("running", move_dir!=Vector3.zero && !anim.GetCurrentAnimatorStateInfo(0).IsName("throw")); 
    }

    void FixedUpdate()
    {        
        move_dir= transform.right*x_axis+transform.forward*z_axis;

        //u can run only when charr_ running anim is activated
        if(!rb.isKinematic && anim.GetCurrentAnimatorStateInfo(0).IsName("running"))
        { 
            rb.MovePosition(rb.position+move_dir*speed*Time.fixedDeltaTime); 
        }
    }


    void Input_s()
    {
        if(Input.GetMouseButtonUp(0) && move_dir==Vector3.zero)
        { 
            anim.SetTrigger("throw"); 
        }
    }



    bool trigger_port=false;    
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("enemy") && !trigger_port)
        {
            anim.SetTrigger("impact");
            if(Random.Range(0,101)>90) play_sound(_ac[1]);
            trigger_port=true;
            StartCoroutine(trigprot());
        }
    }

    IEnumerator trigprot()
    {
        yield return new WaitForSeconds(0.45f);
        trigger_port=false; 
    }


    void play_sound(AudioClip sfx)
    {
        _as.clip=sfx;
        _as.Play();
    }

    public void throw_()
    { 
        StartCoroutine(throw_sound());
        GameObject copy=Instantiate(snow_ball,snow_ball_spawn_point.position,Quaternion.identity,snow_ball_spawn_point);
        copy.GetComponent<snowball>().where_to_go=camera_to_world;
        copy.tag=transform.tag;
    }

    IEnumerator throw_sound()
    {
        yield return new WaitForSeconds(0.7f); 
        play_sound(_ac[0]);
    }

    Vector3 throw_point()
    { 
        Vector3 point_= new Vector3();
        ray= cam.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray,out hit))
        { 
            point_=hit.point;
        }
        
        if(!Physics.Raycast(ray,out hit))
        { 
            point_=Camera.main.transform.forward*40f;
        }

        return point_;
    }
}
