using UnityEngine;

namespace Core.Scripts.ControlTimePower
{
    public class ControlTimeHandler : MonoBehaviour
    {
        #region Statements

        private void OnEnable()
        {
            ControlTime.SwitchEventAction += OnControlTimeSwitchEvent;
        }
        
        private void OnDisable()
        {
            ControlTime.SwitchEventAction -= OnControlTimeSwitchEvent;
        }

        #endregion

        private void OnControlTimeSwitchEvent()
        {
            Debug.Log("ControlTimeHandler: OnControlTimeSwitchEvent");
        }
    }
}