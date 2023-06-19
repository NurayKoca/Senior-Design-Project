using _Workspace.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectingScreenController : MonoBehaviour
{
    public Button nextCarButton;
    public Button prevCarButton;
    public Button selectCarButton;

    private int _carId = 0;

    public CarDataIndex carDataIndex;
    private void Awake()
    {
        nextCarButton.onClick.AddListener(OnNextCarSelected);
        prevCarButton.onClick.AddListener(OnPrevCarSelected);
        selectCarButton.onClick.AddListener(OnCarChoosed);
    }

    private void OnPrevCarSelected()
    {
        CarSelectAreaController.OnPrevCarClicked?.Invoke(DecreaseCarId());
    }

    private void OnNextCarSelected()
    {
        CarSelectAreaController.OnNextCarClicked?.Invoke(IncreaseCarId());
    }

    private void OnCarChoosed()
    {
        CarSelectAreaController.OnCarSelected?.Invoke(_carId);
    }


    private int IncreaseCarId()
    {
        _carId++;
        if (_carId >= carDataIndex.carDataSoList.Count)
        {
            _carId = 0;
        }

        return _carId;
    }

    private int DecreaseCarId()
    {
        _carId--;

        if (_carId < 0)
        {
            _carId = carDataIndex.carDataSoList.Count - 1;
        }

        return _carId;
    }
}
