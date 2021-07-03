using System;
using UnityEngine;
using ModThatIsNotMod;

namespace AIModifier.UI
{
    class KeyboardController : MonoBehaviour
    {
        public KeyboardController(IntPtr ptr) : base(ptr) { }

        private Transform playerHead;
        private Transform playerPelvis;
        

        void Awake()
        {
            playerPelvis = Player.GetRigManager().transform.FindChild("[SkeletonRig (Realtime SkeleBones)]").FindChild("Pelvis");
            playerHead = Player.GetPlayerHead().transform;
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, playerPelvis.position + 0.8f * playerPelvis.forward, Time.deltaTime * 6);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - playerHead.position), 5 * Time.deltaTime);
        }
    }
}
