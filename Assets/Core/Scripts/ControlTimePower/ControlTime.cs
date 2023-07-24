using System;
using UnityEngine;

namespace Core.Scripts.ControlTimePower
{
    public class ControlTime : MonoBehaviour
    {
        public static event Action SwitchEventAction;
        
        public static void OnSwitch()
        {
            SwitchEventAction?.Invoke();
        }
    }
}
