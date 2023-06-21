using DG.Tweening;
using TMPro;
using UnityEngine;

public class SpeedoMeterUIController : MonoBehaviour
{
    [SerializeField] private Transform speedHand;
    [SerializeField] private TextMeshProUGUI speedText;


    public void SetSpeed(float from, float to)
    {
        DOTween.Kill(speedText, true);
         DOVirtual.Float(from, to, .1f, (x)=>
         {
            speedText.SetText(((int)x).ToString());
         }).SetEase(Ease.Linear);
    }

    public void SetSpeedHand(float velocity)
    {
        // DOTween.Kill(speedHand, false);
        // speedHand.transform.DORotate(new Vector3(0, 0, velocity), 1f, RotateMode.FastBeyond360)
        //     .SetEase(Ease.Linear);

        speedHand.transform.localEulerAngles = new Vector3(0, 0, velocity);
    }
}
