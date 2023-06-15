using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Controllers.Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        #region Statements

        public List<Target> Targets = new();

        #endregion

        #region Events

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) return;
            
            Targets.Add(target);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) return;
            
            Targets.Remove(target);
        }

        #endregion
    }
}
