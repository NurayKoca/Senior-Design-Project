using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void BackToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(0);
    }

    public void OpenAndUpdateUI(int rank, float currentTime)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .3f);
        
        timeTxt.SetText(secondToTimeConvertor(currentTime));
        rankTxt.SetText($"Your Rank Is : {rank}");
    }

    private string secondToTimeConvertor(float second)
    {
        return (second / 60f).ToString("0.00") + "Minute";
    }
}
