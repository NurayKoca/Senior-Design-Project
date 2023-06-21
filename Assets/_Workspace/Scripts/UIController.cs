using System;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Workspace.Scripts;

public class UIController : MonoBehaviour
{
    #region Variables

    public static UIController instance;

    [SerializeField] private GameObject netcodeUIParent;
    [SerializeField] private GameObject carSelectUIParent;
    [SerializeField] private GameObject miniMapParent;
    [SerializeField] private GameObject waitingUIParent;
    [SerializeField] private GameObject countDownTimerUIParent;
    [SerializeField] private CanvasGroup speedoMeterCanvasGroup;
    [SerializeField] private LevelEndUIController _levelEndUIController;

    #endregion


    #region Unity Actions

    public static  UnityAction<int> OnNetcodeUiSelected;

    #endregion
    
    #region Unity Funcs

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnNetcodeUiSelected += CloseNetcodeUI;
        CarSelectAreaController.OnCarSelected += OpenWaitingUI_ServerRpc;
        CarSelectAreaController.OnCarSelected += CloseCarSelectUI;
        GameManager.instance.OnAllPlayersReady += CloseWaitingUI;
        GameManager.instance.OnAllPlayersReady += OpenCountDownTimerUI;
    }

    private void OnDestroy()
    { 
           OnNetcodeUiSelected -= CloseNetcodeUI;
           CarSelectAreaController.OnCarSelected -= CloseCarSelectUI;
           CarSelectAreaController.OnCarSelected -= OpenWaitingUI_ServerRpc;
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

    [ServerRpc]
    private void OpenWaitingUI_ServerRpc(int index)
    {
        if(GameManager.instance.NetworkManager.ConnectedClientsList.Count ==1)
            return;
        
        Debug.Log($"Open Waiting UI -> {Time.time}");
        waitingUIParent.SetActive(true);
    }

    private void CloseWaitingUI()
    {
        Debug.Log($"Close Waiting UI -> {Time.time}");
        waitingUIParent.SetActive(false);
    }

    private void OpenCountDownTimerUI()
    {
        Debug.Log($"Open CountDown UI -> {Time.time}");
        countDownTimerUIParent.SetActive(true);
        CloseWaitingUI();
    }

    private void CloseCountDownTimerUI()
    {
        countDownTimerUIParent.SetActive(false);
    }

    public void OpenSpeedoMeterUI()
    {
        speedoMeterCanvasGroup.DOFade(1, .3f);
    }


    public void OpenAndUpdateFinishUI(int rank, float currentTime)
    {
        _levelEndUIController.OpenAndUpdateUI(rank, currentTime);
    }
}
