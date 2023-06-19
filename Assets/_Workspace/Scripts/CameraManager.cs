using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera carSelectCamera;
    public CinemachineVirtualCamera carFollowCamera;


    public CinemachineVirtualCamera activeCamera;

    private void Awake()
    {
        activeCamera = carSelectCamera;

        carFollowCamera.enabled = false;
    }

    public void ChangeCamera(CinemachineVirtualCamera newCamera)
    {
        activeCamera.enabled = false;
        newCamera.enabled = true;
        activeCamera = newCamera;
    }
    
}
