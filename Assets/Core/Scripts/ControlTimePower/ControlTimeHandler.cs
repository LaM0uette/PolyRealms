using UnityEngine;

namespace Core.Scripts.ControlTimePower
{
    public class ControlTimeHandler : MonoBehaviour
    {
        #region Statements
        
        private int _currentAge;

        private GameObject _childMiddleAge;
        private GameObject _childModernAge;
        private GameObject _childFuturAge;

        private void Awake()
        {
            _childMiddleAge = gameObject.transform.GetChild(0).gameObject;
            _childModernAge = gameObject.transform.GetChild(1).gameObject;
            _childFuturAge = gameObject.transform.GetChild(2).gameObject;
        }

        private void OnEnable()
        {
            ControlTime.SwitchEventAction += OnControlTimeSwitchEvent;
        }
        
        private void OnDisable()
        {
            ControlTime.SwitchEventAction -= OnControlTimeSwitchEvent;
        }

        #endregion

        #region Functions

        private void OnControlTimeSwitchEvent()
        {
            ResetAges();
            
            switch (_currentAge)
            {
                case 0:
                    _childModernAge.SetActive(true);
                    _currentAge++;
                    break;
                case 1:
                    _childFuturAge.SetActive(true);
                    _currentAge++;
                    break;
                case 2:
                    _childMiddleAge.SetActive(true);
                    _currentAge = 0;
                    break;
            }
        }

        private void ResetAges()
        {
            _childMiddleAge.SetActive(false);
            _childModernAge.SetActive(false);
            _childFuturAge.SetActive(false);
        }

        #endregion
    }
}