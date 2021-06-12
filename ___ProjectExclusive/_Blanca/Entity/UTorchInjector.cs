using IKEssentials;
using SharedLibrary;
using UnityEngine;

namespace Blanca
{
    public class UTorchInjector : MonoInjector
    {
        [SerializeField] private UHandTargetBase leftTarget;
        [SerializeField] private UHandTargetBase rightTarget;

        public override void DoInjection()
        {
            BlancaPropsEntity entity = BlancaEntitySingleton.Instance.Props;
            entity.TorchLeftTarget = leftTarget;
            entity.TorchRightTarget = rightTarget;
        }
    }
}
