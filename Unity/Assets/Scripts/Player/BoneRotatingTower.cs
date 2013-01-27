using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class BoneRotatingTower : GenericTower
    {
        public override void OnAttack(UnityEngine.Collider target)
        {
            return;
            Vector3 lookAt = target.transform.position;
            lookAt.y = transform.position.y;
            Transform joint = transform.FindChild("joint1");
            joint.LookAt(lookAt);
        }
    }
}
