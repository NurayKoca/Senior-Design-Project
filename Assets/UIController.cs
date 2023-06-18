using System;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject netcodeUIParent;

    #endregion


    #region Unity Actions

    public static  UnityAction OnNetcodeUiSelected;

    #endregion


    #region Unity Funcs

    private void OnEnable()
    {
        OnNetcodeUiSelected += CloseNetcodeUI;
    }

    private void OnDisable()
    { 
        OnNetcodeUiSelected -= CloseNetcodeUI;
    }

    #endregion
    
    private void CloseNetcodeUI()
    {
        netcodeUIParent.SetActive(false);
    }
}
