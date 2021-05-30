using IKEssentials;
using SharedLibrary;
using UnityEngine;

namespace Blanca
{
    public class BlancaIKControl : ITicker
    {
        private readonly ICharacterTransformData _characterTransform;
        private readonly IHeadIKSolver _headIkSolver;
        private readonly BlancaLookAtControlHolder _lookAtControls;

        public BlancaIKControl(ICharacterTransformData characterTransform,
            IHeadIKSolver headIkSolver,
            BlancaLookAtControlHolder lookAtControls)
        {
            _characterTransform = characterTransform;
            _headIkSolver = headIkSolver;
            _lookAtControls = lookAtControls;
            _headIkSolver.SetWeight(1);
        }

        public bool Disabled { get; set; }
        public void Tick()
        {
            Vector3 headPosition = _characterTransform.HeadPosition;
            Vector3 pointLookAt = _lookAtControls.CalculatePointLookAt(headPosition);
            Vector3 smallForward = _characterTransform.MeshForward * .2f;

            _headIkSolver.SetTarget(pointLookAt + smallForward);
        }
    }
}
