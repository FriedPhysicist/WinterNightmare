using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_ : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public AudioSource _as;
    [SerializeField]
    public AudioClip[] _ac;

    bool get_bigger=false;
    public GameObject charr_main;

    public static bool death=false;


    void Start()
    { 
    }


    public static bool roar_once=true;
    void Update()
    {
        anim.SetBool("death",death);

        if(!death)
        {
            if(Input.GetKeyUp(KeyCode.Y)) get_bigger=true;

            if(get_bigger && roar_once)
            {
                anim.SetTrigger("Roar"); 
                _as.clip=_ac[0]; 
                _as.Play();
                roar_once=false;
            }

            if(get_bigger) transform.localScale*=1.1f;

            if(transform.localScale.y>=3)
            {
                get_bigger=false;
                transform.LookAt(new Vector3(charr_main.transform.position.x,transform.position.y,charr_main.transform.position.z));
                rb.velocity=transform.forward*2f; 
            }
        }

        if(death)
        {
            rb.isKinematic=true;
            rb.velocity=Vector3.zero;
        }
    }


    public int damage=0;
    void OnTriggerStay(Collider other)
    { 
        if(other.CompareTag("Player") || other.CompareTag("ally") || other.CompareTag("enemy"))
        {
            damage++;

            if(damage>=2500)
            {
                death=true;
            }
        }
    }

    void play_sound(AudioClip sfx)
    {
        if(transform.localScale.y>=3)
        {
            camera_controller.vib_bool=true;
            _as.clip=sfx;
            _as.Play();
        }
    }
}
