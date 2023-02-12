using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTackder : MonoBehaviour
{
    // 싱글톤
    public static ImageTackder instance;
    //
    private ARTrackedImageManager trackedImageManager;
    // 리소스 로드용 프리팹
    [SerializeField]
    private GameObject[] prefabs;
    // 관리용 딕셔너리
    public Dictionary<string, GameObject> monsterDic;

    void Awake()
    {
        if(instance == null)
            instance = this;
        
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        monsterDic = new Dictionary<string, GameObject>();

        foreach(GameObject one in prefabs)
        {
            GameObject go = Instantiate(one);
            go.name = one.name;
            go.SetActive(false);

            monsterDic.Add(go.name, go);
        }
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImage;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImage;
    }

    // 이미지 추적
    void OnTrackedImage(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
            UpdatePrefab(trackedImage);

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
            UpdatePrefab(trackedImage);

        RemovePrefab();
        // 참조 이미지는 로직상 제거되지 않음
        /*foreach (ARTrackedImage trackedImage in eventArgs.removed)
            monsterDic[trackedImage.name].SetActive(false);*/
    }

    // 몬스터 생성 및 위치
    void UpdatePrefab(ARTrackedImage trackedImage)
    {
        string ImageName = trackedImage.referenceImage.name;
        monsterDic[ImageName].transform.position = trackedImage.transform.position;
        //monsterDic[ImageName].transform.rotation = trackedImage.transform.rotation;

        monsterDic[ImageName].SetActive(true);
    }

    // 몬스터 제거
    void RemovePrefab()
    {
        //trackables 상태로 이미지 확인
        foreach (var item in trackedImageManager.trackables)
        {
            if(item.trackingState != TrackingState.Tracking)
            {
                monsterDic[item.referenceImage.name].SetActive(false);
            }
        }
    }
}
