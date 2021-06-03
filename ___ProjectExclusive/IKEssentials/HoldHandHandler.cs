using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IKEssentials
{
    public class HoldHandHandler
    {
        public Transform MasterReference;
        public Transform SlaveReference;
        private readonly Transform _targetIk;
        private readonly Transform _masterTarget;
        private readonly Transform _slaveTarget;

        private const float SeparationDistance = .02f;
        public HoldHandHandler(Transform master, Transform slave, bool debugWithPrimitives = true)
        {
            MasterReference = master;
            SlaveReference = slave;

            _targetIk = new GameObject("Hold Handler (Helper)").transform;
            _masterTarget = new GameObject("Master Target").transform;
            _slaveTarget = new GameObject("Slave Target").transform;

            _masterTarget.parent = _targetIk;
            _slaveTarget.parent = _targetIk;

            Vector3 targetLocalPosition = new Vector3(0, SeparationDistance, SeparationDistance);
            _masterTarget.localPosition = targetLocalPosition;
            targetLocalPosition.z *= -1;
            _slaveTarget.localPosition = targetLocalPosition;

            Timing.RunCoroutine(Update());

            if (debugWithPrimitives)
            {
                GeneratePrimitive(_slaveTarget);
                GeneratePrimitive(_masterTarget);
                void GeneratePrimitive(Transform onTransform)
                {
                    Transform primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform;
                    primitive.parent = onTransform;
                    primitive.localPosition = Vector3.zero;
                    primitive.localScale = Vector3.one * .05f; 

                }
            }
        }


        private IEnumerator<float> Update()
        {
            while (MasterReference != null && SlaveReference != null)
            {
                Vector3 masterPosition = MasterReference.position;
                Vector3 slavePosition = SlaveReference.position;

                Vector3 directionTowardsSlave = slavePosition - masterPosition;
                Vector3 positionOfHands = masterPosition + directionTowardsSlave * .5f;

                _targetIk.position = positionOfHands;
                _targetIk.rotation = Quaternion.LookRotation(directionTowardsSlave);
                yield return Timing.WaitForOneFrame; 
            }
        }

    }
}
