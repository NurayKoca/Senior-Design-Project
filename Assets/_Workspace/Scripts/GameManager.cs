using System;
using UnityEngine;

namespace Workspace.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}
