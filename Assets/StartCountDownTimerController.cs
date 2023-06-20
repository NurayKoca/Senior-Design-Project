using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCountDownTimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownTxt;

    private int count = 3;

    private void OnEnable()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        StartCoroutine(CountDownTimer());
    }
    
    private IEnumerator CountDownTimer()
    {
        var second = new WaitForSeconds(1);
        
        countDownTxt.SetText(count.ToString());

        count--;

        yield return second;
        
        countDownTxt.SetText(count.ToString());

        count--;

        yield return second;
        
        countDownTxt.SetText(count.ToString());

        count--;

        yield return second;
        
        countDownTxt.SetText(count.ToString());

        yield return new WaitForSeconds(.15f);
        
        countDownTxt.SetText("GO!");
        
        yield return new WaitForSeconds(.15f);
        
        gameObject.SetActive(false);
    }
}
