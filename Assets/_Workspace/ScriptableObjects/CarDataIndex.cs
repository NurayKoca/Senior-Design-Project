using System.Collections.Generic;
using UnityEngine;

namespace _Workspace.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CarDataIndex", menuName = "CarDataIndex", order = 0)]
    public class CarDataIndex : ScriptableObject
    {
        public List<CarDataHolder> carDataSoList = new List<CarDataHolder>();

        public int FindCarDataSoIndex(CarDataHolder carDataSo)
        {
            return carDataSoList.IndexOf(carDataSo);
        }

        public CarDataHolder GetCarDataSoByIndex(int index)
        {
            return carDataSoList[index];
        }
    }
}