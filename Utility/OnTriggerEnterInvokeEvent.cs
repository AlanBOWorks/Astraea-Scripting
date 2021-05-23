using System.Collections.Generic;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace Utility
{
    public class OnTriggerEnterInvokeEvent : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField, HideInPlayMode]
        private Collider[] _ignoreColliders = new Collider[0];

        [InfoBox("This component will be destroyed after it triggers")]
        [SerializeField] 
        private int _enterCounterForActivation = 1;
        private int _enteredCounter = 0;
        private bool _invoked = false;

        [SerializeField] private bool _destroyInInvoke = true;

        [Title("Toggle objects")]
        public UltEvent OnEnterMeetEvent = new UltEvent();
        public UltEvent OnExitEvents = new UltEvent();


        private void Awake()
        {
            if(_ignoreColliders.Length < 1) return;

            Collider myCollider = GetComponent<Collider>() ?? gameObject.AddComponent<SphereCollider>();

            for (int i = 0; i < _ignoreColliders.Length; i++)
            {
                Physics.IgnoreCollision(myCollider,_ignoreColliders[i]);
            }

            _ignoreColliders = null;
        }

        private void OnTriggerEnter(Collider col)
        {
            _enteredCounter++;
            if (_enteredCounter < _enterCounterForActivation) return;

            OnEnterMeetEvent.Invoke();

            if(_destroyInInvoke)
                Destroy(this);
            else
            {
                _invoked = true;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            _enteredCounter--;

            if (!_invoked || _enteredCounter > 0) return;

            OnExitEvents.Invoke();
            _invoked = false;
        }

        private void PrepareObject(Collider myCollider)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            myCollider.isTrigger = true;
        }
    }
}
