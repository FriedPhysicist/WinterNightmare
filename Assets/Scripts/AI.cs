using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    [SerializeField]
    public Transform[] points;

    [SerializeField]
    public Transform[] enemies;
    Transform selected_enemy;
    
    public GameObject monster_enemy;

    public Rigidbody rb;
    public Animator anim;
    public AudioSource _as;
    [SerializeField]
    public AudioClip[] _ac;

    float timer_current;
    public float timer_limit=60;
    bool shoot_bool=false;

    public GameObject snow_ball;
    public Transform snow_ball_point;
    static public Vector3 where_to_go_AI;


    Vector3 next_point;
    float enemy_ally;


    void Start()
    { 
        if(transform.tag=="ally") enemy_ally=Random.Range(-15,-9);
        if(transform.tag=="enemy") enemy_ally=Random.Range(-31,-24);
        next_point=new Vector3(Random.Range(-5,50),0,enemy_ally);
        timer_limit+=Random.Range(-10,35); 
    }

    void Update()
    {
        if(!monster_.death)
        {
            timer_shoot();
            running(shoot_bool);
            shooting(shoot_bool);        
        }

        anim.SetBool("dance",monster_.death);
    }


    void running(bool shoot_bool)
    {
        if(!shoot_bool)
        { 
            transform.LookAt(new Vector3(next_point.x,transform.position.y,next_point.z));
            rb.velocity=transform.forward*2;
            if(monster_.roar_once) selected_enemy=enemies[Random.Range(0,5)];
            if(!monster_.roar_once) selected_enemy=monster_enemy.transform;

            if(transform.tag=="ally") enemy_ally=Random.Range(-25,-9);
            if(transform.tag=="enemy") enemy_ally=Random.Range(-31,-15);

            if(Vector3.Distance(transform.position,next_point)<=5f) next_point=new Vector3(Random.Range(0,41),0,enemy_ally);
        }
    }

    void shooting(bool shoot_bool)
    {
        if(shoot_bool)
        { 
            rb.velocity=Vector3.zero;
            transform.LookAt(new Vector3(selected_enemy.position.x,transform.position.y,selected_enemy.position.z));
            anim.SetTrigger("throw");
        }
    }


    void timer_shoot()
    { 
        if(timer_current<=timer_limit)
        {
            timer_current++;
            shoot_bool=false;
        }

        if(timer_current>=timer_limit)
        {
            shoot_bool=true;
        }
    }


    bool trigger_prot=false;
    void OnTriggerStay(Collider other)
    { 
        if(!trigger_prot)
        {
            trigger_prot=true;
            if(enemy_ally==-8.5f)
            {
                if(other.CompareTag("enemy"))
                {
                    anim.SetTrigger("impact");
                }
            }

            if(enemy_ally==-31f)
            {
                if(other.CompareTag("ally"))
                {
                    anim.SetTrigger("impact");
                }
            }

            if(Random.Range(0,101)>90) play_sound(_ac[1]); 
            StartCoroutine(trigprot());
        }
    }

    IEnumerator trigprot()
    {
        yield return new WaitForSeconds(0.45f);
        trigger_prot=false; 
    }


    public void shoot_event()
    {
        StartCoroutine(throw_sound());
        GameObject copy_AI=Instantiate(snow_ball,snow_ball_point.position,Quaternion.identity,snow_ball_point);
        copy_AI.GetComponent<snowball>().where_to_go=new Vector3(selected_enemy.position.x,selected_enemy.position.y+1,selected_enemy.position.z);
        copy_AI.tag=transform.tag; 
    }
    IEnumerator throw_sound()
    {
        yield return new WaitForSeconds(0.7f); 
        play_sound(_ac[0]);
    }


    public void shoot_event_end()
    {
        shoot_bool=false;
        timer_current=0;
    }
    
    void play_sound(AudioClip sfx)
    {
        _as.clip=sfx;
        _as.Play();
    }

}
