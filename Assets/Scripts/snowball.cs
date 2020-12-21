using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowball : MonoBehaviour
{

    public float start_time;
    bool go=false;
    public Rigidbody rb;
    public Vector3 where_to_go;
    Vector3 where_to_go_enemy;
    public AudioSource _as;

    public ParticleSystem boom;

    bool every_thing_lie=false;


    void Start()
    { 
        StartCoroutine(can_go(start_time));
    }


    void Update()
    { 
        if(!every_thing_lie)
        {
            if(Vector3.Distance(transform.position,where_to_go)<=0.01f)
            { 
                go=false;
                rb.AddRelativeForce(new Vector3(0,0,15f),ForceMode.Impulse);
                where_to_go=transform.forward*2;
            }

            if(go)
            {
                transform.LookAt(where_to_go);
                //transform.position=Vector3.MoveTowards(transform.position,where_to_go,0.7f);
                transform.position=Vector3.MoveTowards(transform.position,where_to_go,0.7f); 
            }
        }
    }


    bool audio_protect=false;
    void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag(transform.tag) && !other.CompareTag("plane") && !other.CompareTag("Player") && !audio_protect)
        { 
            rb.isKinematic=true;
            every_thing_lie=true;
            _as.Play();
            transform.GetComponent<MeshRenderer>().enabled=false;
            boom.Play();
            Destroy(gameObject,0.7f);
            audio_protect=true; 
        }
    }

    IEnumerator can_go(float time)
    {
        yield return new WaitForSeconds(time); 
        go=true;
        transform.parent=null;
    }
}
