using System;
using DG.Tweening;
using TMPro;
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
    [SerializeField] private GameObject timeElapsedUIGo;
    [SerializeField] private TextMeshProUGUI timeElapsedTxt;

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
        if(NetworkManager.Singleton.IsHost)
            if(GameManager.instance.NetworkManager.ConnectedClientsList.Count ==1)
                return;
        
        waitingUIParent.SetActive(true);
    }

    private void CloseWaitingUI()
    {
        waitingUIParent.SetActive(false);
    }

    private void OpenCountDownTimerUI()
    {
        
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
        OpenTimePanel();
    }


    private bool isFinishUIUsed = false;
    public void OpenAndUpdateFinishUI(int rank, float currentTime)
    {
        if(isFinishUIUsed)
            return;

        isFinishUIUsed = true;
        CloseTimePanel();
        _levelEndUIController.OpenAndUpdateUI(rank, currentTime);
    }

    public void OpenTimePanel()
    {
        timeElapsedUIGo.SetActive(true);
    }

    public void CloseTimePanel()
    {
        timeElapsedUIGo.SetActive(false);
    }

    public void UpdateTimeElapsed(string time)
    {
        timeElapsedTxt.SetText($"Time : {time}");
    }
}
