using _Workspace.Scripts;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<CarController>(out var carController))
        {
            carController.Finish();
            carController.UpdateFinishUI();
        }
    }
}
