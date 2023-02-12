using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class InputSystem : MonoBehaviour
{
    public Camera arCamera;

    // ��� ���·� ���ư��� �ð�
    public float attackAnimTime;
    public float rotateAnimTime;

    // ��ġ ��ǥ (����, ��, ����)
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDis;
    // �ΰ���
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
        // ��ġ ����
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
                // ��������
                if(Mathf.Abs(touchDis.y) > swipeSensivity || Mathf.Abs(touchDis.x) > swipeSensivity)
                {
                    foreach (var item in ImageTackder.instance.monsterDic)
                    {
                        //���� ������ ���� ������ Ŀ�� ��� ���� ����
                        MonsterController mob = item.Value.GetComponent<MonsterController>();
                        mob.state = MonsterController.State.rotate;
                        StartCoroutine(Timer(mob, MonsterController.State.idle, rotateAnimTime));
                    }
                }
                // ��ġ
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
                                //���� ������ ���� ������ Ŀ�� ��� ���� ����
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

    // �ð� ��� �� ���ϴ� ���·� ��ȯ
    IEnumerator Timer(MonsterController mob, MonsterController.State state, float time)
    {
        yield return new WaitForSeconds(time);
        mob.state = state;
    }
}
