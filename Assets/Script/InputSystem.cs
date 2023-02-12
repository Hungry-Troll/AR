using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class InputSystem : MonoBehaviour
{
    public Camera arCamera;

    // 대기 상태로 돌아가는 시간
    public float attackAnimTime;
    public float rotateAnimTime;

    // 터치 좌표 (시작, 끝, 방향)
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDis;
    // 민감도
    private float swipeSensivity;

    void Start()
    {
        attackAnimTime = 0.3f;
        rotateAnimTime = 0.6f;
        swipeSensivity = 4.0f;
    }

    void Update()
    {
        InputCheck();
    }

    void InputCheck()
    {
        // 터치 시작
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;
            }

            if(touch.phase == TouchPhase.Ended)
            {
                touchEndedPos = touch.position;
                touchDis = (touchEndedPos - touchBeganPos);
                // 스와이프
                if(Mathf.Abs(touchDis.y) > swipeSensivity || Mathf.Abs(touchDis.x) > swipeSensivity)
                {
                    foreach (var item in ImageTackder.instance.monsterDic)
                    {
                        //몬스터 관리는 추후 볼륨이 커질 경우 따로 관리
                        MonsterController mob = item.Value.GetComponent<MonsterController>();
                        mob.state = MonsterController.State.rotate;
                        StartCoroutine(Timer(mob, MonsterController.State.idle, rotateAnimTime));
                    }
                }
                // 터치
                else
                {
                    Ray ray;
                    RaycastHit hitInfo;

                    ray = arCamera.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        foreach (var item in ImageTackder.instance.monsterDic)
                        {
                            if (hitInfo.collider.name.Equals(item.Key))
                            {
                                //몬스터 관리는 추후 볼륨이 커질 경우 따로 관리
                                MonsterController mob = hitInfo.collider.gameObject.GetComponent<MonsterController>();
                                mob.state = MonsterController.State.attack;
                                StartCoroutine(Timer(mob, MonsterController.State.idle, attackAnimTime));
                            }
                        }
                    }
                }
            }
        }
    }

    // 시간 계산 후 원하는 상태로 변환
    IEnumerator Timer(MonsterController mob, MonsterController.State state, float time)
    {
        yield return new WaitForSeconds(time);
        mob.state = state;
    }
}
