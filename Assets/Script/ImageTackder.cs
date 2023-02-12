using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTackder : MonoBehaviour
{
    // �̱���
    public static ImageTackder instance;
    //
    private ARTrackedImageManager trackedImageManager;
    // ���ҽ� �ε�� ������
    [SerializeField]
    private GameObject[] prefabs;
    // ������ ��ųʸ�
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

    // �̹��� ����
    void OnTrackedImage(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
            UpdatePrefab(trackedImage);

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
            UpdatePrefab(trackedImage);

        RemovePrefab();
        // ���� �̹����� ������ ���ŵ��� ����
        /*foreach (ARTrackedImage trackedImage in eventArgs.removed)
            monsterDic[trackedImage.name].SetActive(false);*/
    }

    // ���� ���� �� ��ġ
    void UpdatePrefab(ARTrackedImage trackedImage)
    {
        string ImageName = trackedImage.referenceImage.name;
        monsterDic[ImageName].transform.position = trackedImage.transform.position;
        //monsterDic[ImageName].transform.rotation = trackedImage.transform.rotation;

        monsterDic[ImageName].SetActive(true);
    }

    // ���� ����
    void RemovePrefab()
    {
        //trackables ���·� �̹��� Ȯ��
        foreach (var item in trackedImageManager.trackables)
        {
            if(item.trackingState != TrackingState.Tracking)
            {
                monsterDic[item.referenceImage.name].SetActive(false);
            }
        }
    }
}
