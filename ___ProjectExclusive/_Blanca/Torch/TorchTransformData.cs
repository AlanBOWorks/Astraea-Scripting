using System;
using UnityEngine;

namespace Blanca
{
    public class TorchLocalPositionData : TorchTargetsBase<Vector3>
    {

        public TorchLocalPositionData(ITorchTargets<Transform> transforms)
        {
            Holder = transforms.Holder.localPosition;
            LeftHandRoot = transforms.LeftHandRoot.localPosition;
            RightHandRoot = transforms.RightHandRoot.localPosition;
            LeftBendTarget = transforms.LeftBendTarget.localPosition;
            RightBendTarget = transforms.RightBendTarget.localPosition;
        }

    }

    public class TorchLocalRotationData : TorchTargetsBase<Quaternion>
    {

        public TorchLocalRotationData(ITorchTargets<Transform> transforms)
        {
            Holder = transforms.Holder.localRotation;
            LeftHandRoot = transforms.LeftHandRoot.localRotation;
            RightHandRoot = transforms.RightHandRoot.localRotation;
            LeftBendTarget = transforms.LeftBendTarget.localRotation;
            RightBendTarget = transforms.RightBendTarget.localRotation;
        }
    }
}
