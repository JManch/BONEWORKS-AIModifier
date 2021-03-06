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
        public Vector3 offSet = Vector3.zero;

        private Transform playerHead;
        
        public void OnSpawn()
        {
            transform.position = playerHead.position + distance * Vector3.ProjectOnPlane(playerHead.forward, Vector3.up) + offSet;
        }

        void Awake()
        {
            playerHead = Player.GetRigManager().transform.FindChild("[SkeletonRig (Realtime SkeleBones)]/Head");
        }

        void OnEnable()
        {
            OnSpawn();
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, playerHead.position + distance * Vector3.ProjectOnPlane(playerHead.forward, Vector3.up) + offSet, Time.deltaTime * speed);
        }
    }
}
