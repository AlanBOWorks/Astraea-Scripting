using System;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(SphereCollider))]
    public class BiToggleTriggerEvents : MonoBehaviour
    {
        public bool IsStateA = false;
        [Title("Settings")]
        [SerializeField, HideInPlayMode]
        private Collider[] _ignoreColliders = new Collider[0];

        [SerializeField] private TriggerSetting _pointASetting;
        [SerializeField] private TriggerSetting _pointBSetting;

        [Title("Toggle objects")]
        public UltEvent PointAEvent = new UltEvent();
        public UltEvent PointBEvent = new UltEvent();

        [SerializeField]
        private SphereCollider _myCollider = null;

        private void Awake()
        {
            if (_ignoreColliders.Length < 1) return;

            if(_myCollider is null)
                _myCollider = GetComponent<SphereCollider>();

            for (int i = 0; i < _ignoreColliders.Length; i++)
            {
                Physics.IgnoreCollision(_myCollider, _ignoreColliders[i]);
            }

            _ignoreColliders = null;
        }

        private void Start()
        {
            PrepareState();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (IsStateA)
            {
                PointAEvent.Invoke();

                SwitchState(_pointBSetting);
            }
            else
            {
                PointBEvent.Invoke();

                SwitchState(_pointASetting);
            }
            IsStateA = !IsStateA;
        }
        private void SwitchState(TriggerSetting setting)
        {
            _myCollider.radius = setting.TriggerRadius;
            _myCollider.center = transform.InverseTransformPoint(setting.PointReference.position);
        }

        [Button]
        private void PrepareState()
        {
            SwitchState(IsStateA ? _pointASetting : _pointBSetting);
        }

        [Serializable]
        private struct TriggerSetting
        {
            public Transform PointReference;
            public float TriggerRadius;
        }
    }
}
