using System;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject netcodeUIParent;
    [SerializeField] private GameObject carSelectUIParent;
    [SerializeField] private GameObject miniMapParent;

    #endregion


    #region Unity Actions

    public static  UnityAction<int> OnNetcodeUiSelected;

    #endregion


    #region Unity Funcs

    private void OnEnable()
    {
        OnNetcodeUiSelected += CloseNetcodeUI;
        CarSelectAreaController.OnCarSelected += CloseCarSelectUI;
    }

    private void OnDisable()
    { 
           OnNetcodeUiSelected -= CloseNetcodeUI;
           CarSelectAreaController.OnCarSelected -= CloseCarSelectUI;
    }

    private void CloseCarSelectUI(int index)
    {
        carSelectUIParent.SetActive(false);
        OpenMiniMap();
    }

    public void OpenCarSelectUI(int index)
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
    #endregion
    
    
}
