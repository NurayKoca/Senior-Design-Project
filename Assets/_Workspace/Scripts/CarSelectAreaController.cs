using System;
using System.Collections.Generic;
using _Workspace.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CarSelectAreaController : MonoBehaviour
{
    public static UnityAction<int> OnNextCarClicked;
    public static UnityAction<int> OnPrevCarClicked;
    public static UnityAction<int> OnCarSelected;
    
    
    [SerializeField] Transform  carSelectPlatformTransform;

    [SerializeField] private CarDataIndex carDataIndex;

    [SerializeField] private MeshRenderer carMeshRenderer;

    private CameraManager _cameraManager;
    
    
    private void Start()
    {
        RotatePlatformTween();
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnEnable()
    {
        OnNextCarClicked += NextCar;
        OnPrevCarClicked += PrevCar;
        OnCarSelected += CarSelected;
    }

    private void OnDisable()
    { 
        OnNextCarClicked -= NextCar;
        OnPrevCarClicked -= PrevCar;
        OnCarSelected -= CarSelected;
    }

    private Tween RotatePlatformTween()
    {
            return carSelectPlatformTransform.DOBlendableRotateBy(new Vector3(0,360,0),7f,RotateMode.FastBeyond360)
                .SetLoops(-1,LoopType.Incremental)
                .SetEase(Ease.Linear);
    }
    
    public void NextCar(int index)
    {
        carMeshRenderer.material = carDataIndex.GetCarDataSoByIndex(Mathf.Abs(index) % carDataIndex.carDataSoList.Count).carMaterial;
    }
    
    public void PrevCar(int index)
    {
        carMeshRenderer.material = carDataIndex.GetCarDataSoByIndex(Mathf.Abs(index) % carDataIndex.carDataSoList.Count).carMaterial;
    }

    public void CarSelected(int index)
    {
        _cameraManager.ChangeCamera(_cameraManager.carFollowCamera);
        Destroy(gameObject);
        
        
    }
}
