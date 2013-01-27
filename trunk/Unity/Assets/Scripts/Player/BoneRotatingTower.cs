using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class BoneRotatingTower : GenericTower
    {
        void Start()
        {
            foreach (AnimationState state in animation)
            {
                state.speed = 5;
            }
        }

        public override void OnAttack(UnityEngine.Collider target)
        {
            Vector3 lookAt = target.transform.position;
            lookAt.y = transform.position.y;
            Transform joint = transform.FindChild("joint1");

            Vector3 diff = transform.position - target.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(diff);
            Vector3 eulerAngles = lookRotation.eulerAngles;
            eulerAngles.y += 90.0f;

            joint.rotation = Quaternion.Euler(eulerAngles);
        }
    }
}
