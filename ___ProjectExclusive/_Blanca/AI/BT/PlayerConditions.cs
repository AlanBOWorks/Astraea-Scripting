using Companion;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Player;
using UnityEngine;

namespace Blanca
{
    [Name("Check if moved\n [Player]")]
    [Category("___ Player ___")]
    [Description("Just checks if the player has move")]
    public class HasPlayerMoved : ConditionTask
    {
        protected override bool OnCheck()
        {
            return PlayerUtilsKinematic.IsPlayerMoving;
        }
    }

    [Name("Close to Blanca\n [Player]")]
    [Category("___ Player ___")]
    public class PlayerInBlancaRange : ConditionTask
    {
        public float DistanceThreshold = .2f;

        protected override bool OnCheck()
        {
            return CompanionUtils.DirectDistanceOfSeparation < DistanceThreshold;
        }
    }


    [Name("Direction of movement\n  is similar to Blanca \n [Player]")]
    [Category("___ Player ___")]
    public class PlayerMoveInBlancaRange : ConditionTask
    {
        public float DistanceThreshold = .2f;

        protected override bool OnCheck()
        {
            return CompanionUtils.DirectDistanceOfSeparation < DistanceThreshold;
        }
    }
}
