using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 연결이 안되고 있음...
public class MonsterController : MonoBehaviour
{
    public enum State
    {
        idle,
        rotate,
        attack,
    }

    public State state;

    Animator anim;
    ParticleSystem pt;


    void Start()
    {
        anim = GetComponent<Animator>();
        pt = Util.FindChild("Particle", gameObject.transform).GetComponent<ParticleSystem>();
        pt.gameObject.SetActive(false);
        state = State.idle;
    }
        
    void Update()
    {
        switch(state)
        {
            case State.idle:
                Idle();
                break;
            case State.attack:
                Attack();
                break;
            case State.rotate:
                Rotate();
                break;
        }
    }

    void Idle()
    {
        // 애니메이션 실행
        anim.SetInteger("stats", 0);
        pt.gameObject.SetActive(false);
        pt.Stop();
    }
    void Attack()
    {
        // 애니메이션 실행
        anim.SetInteger("stats", 1);
        // 파티클 실행
        pt.gameObject.SetActive(true);
        pt.Play();
    }
    void Rotate()
    {
        // 애니메이션 실행
        anim.SetInteger("stats", 2);
        pt.gameObject.SetActive(false);
        pt.Stop();
    }
}

