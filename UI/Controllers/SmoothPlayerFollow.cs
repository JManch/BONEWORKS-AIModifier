using System;
using UnityEngine;
using ModThatIsNotMod;

namespace AIModifier.UI
{
    class SmoothPlayerFollow : MonoBehaviour
    {
        public SmoothPlayerFollow(IntPtr ptr) : base(ptr) { }

        public float distance = 0.8f;
        public float speed = 6f;

        private Transform playerHead;
        
        void Awake()
        {
            playerHead = Player.GetRigManager().transform.FindChild("[SkeletonRig (Realtime SkeleBones)]/Head");
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, playerHead.position + distance * Vector3.ProjectOnPlane(playerHead.forward, Vector3.up), Time.deltaTime * speed);
        }
    }
}
