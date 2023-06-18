using UnityEngine;

namespace _Workspace.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Car Data", menuName = "Car Data", order = 0)]
    public class CarDataHolder : ScriptableObject
    {
        public Material carMaterial;
    }
}