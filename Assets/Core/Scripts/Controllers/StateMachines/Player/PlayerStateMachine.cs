using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            //SwitchState(new PlayerFreeLookState(this));
        }
    }
}
