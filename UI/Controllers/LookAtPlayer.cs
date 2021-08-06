using UnityEngine;
using ModThatIsNotMod;
using System;

namespace AIModifier.UI
{
    class LookAtPlayer : MonoBehaviour
    {
        public LookAtPlayer(IntPtr ptr) : base(ptr) { }

        public bool fixedXAxis = true;
        private Transform playerHead;


        private void Awake()
        {
            playerHead = Player.GetPlayerHead().transform;
        }
        
        private void Update()
        {
            if(fixedXAxis)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(playerHead.transform.position.x, transform.position.y, playerHead.transform.position.z));
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(transform.position - playerHead.transform.position);
            }

            // Code for smooth rotation
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - playerHead.position), 5 * Time.deltaTime);
        }
    }
}
