using System;
using UnityEngine;
using UnityEngine.Events;
using Workspace.Scripts;

public class UIController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject netcodeUIParent;
    [SerializeField] private GameObject carSelectUIParent;
    [SerializeField] private GameObject miniMapParent;
    [SerializeField] private GameObject waitingUIParent;
    [SerializeField] private GameObject countDownTimerUIParent;

    #endregion


    #region Unity Actions

    public static  UnityAction<int> OnNetcodeUiSelected;

    #endregion


    #region Unity Funcs

    private void Start()
    {
        OnNetcodeUiSelected += CloseNetcodeUI;
        CarSelectAreaController.OnCarSelected += CloseCarSelectUI;
        CarSelectAreaController.OnCarSelected += OpenWaitingUI;
        GameManager.instance.OnAllPlayersReady += CloseWaitingUI;
        GameManager.instance.OnAllPlayersReady += OpenCountDownTimerUI;
    }

    private void OnDestroy()
    { 
           OnNetcodeUiSelected -= CloseNetcodeUI;
           CarSelectAreaController.OnCarSelected -= CloseCarSelectUI;
           CarSelectAreaController.OnCarSelected -= OpenWaitingUI;
           GameManager.instance.OnAllPlayersReady -= CloseWaitingUI;
           GameManager.instance.OnAllPlayersReady -= OpenCountDownTimerUI;
    }

   
    #endregion
    
    
    private void CloseCarSelectUI(int index)
    {
        carSelectUIParent.SetActive(false);
        OpenMiniMap();
    }

    private void OpenCarSelectUI(int index)
    {
        carSelectUIParent.SetActive(true);
    }

    private void CloseNetcodeUI(int index)
    {
        netcodeUIParent.SetActive(false);
        OpenCarSelectUI(index);
    }

    private void OpenMiniMap()
    {
        miniMapParent.SetActive(true);
    }

    private void CloseMiniMap()
    {
        miniMapParent.SetActive(false);
    }

    private void OpenWaitingUI(int index)
    {
        waitingUIParent.SetActive(true);
    }

    private void CloseWaitingUI()
    {
        waitingUIParent.SetActive(false);
    }

    private void OpenCountDownTimerUI()
    {
        countDownTimerUIParent.SetActive(true);
    }

    private void CloseCountDownTimerUI()
    {
        countDownTimerUIParent.SetActive(false);
    }
    
    
}
