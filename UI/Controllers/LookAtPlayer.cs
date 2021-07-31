using UnityEngine;
using ModThatIsNotMod;
using System;

namespace AIModifier.UI
{
    class LookAtPlayer : MonoBehaviour
    {
        public LookAtPlayer(IntPtr ptr) : base(ptr) { }

        Transform playerHead;

        private void Awake()
        {
            playerHead = Player.GetPlayerHead().transform;
        }
        
        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(playerHead.transform.position.x, transform.position.y, playerHead.transform.position.z));
        }
    }
}
