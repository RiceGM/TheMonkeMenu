using Photon.Voice.PUN.UtilityScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMonkeMenu.Menu.Mods.Platforms
{
    public class PlatformObject : MonoBehaviour
    {
        public bool isUnEquipped = false;
        bool hasInvokedSelfDeath = false;
        public bool isLeftHand = true;
        public bool isMaster = false;

        void OnEnable()
        {
            transform.localScale = Vector3.zero;
        }

        void FixedUpdate()
        {
            if (isMaster) return;
            if(isUnEquipped)
            {
                if (!hasInvokedSelfDeath)
                {
                    hasInvokedSelfDeath = true;
                    Invoke("SelfDeath", 10f);
                }
                transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f) * Time.fixedDeltaTime * 5f;
                if (transform.localScale.x < 0) SelfDeath();
            } else
            {
                if (transform.localScale.x < 0.1f)
                {
                    transform.localScale += new Vector3(0.15f, 0.15f, 0.15f) * Time.fixedDeltaTime * 5f;
                    if(isLeftHand)
                    {
                        transform.parent.transform.position = GorillaLocomotion.Player.Instance.leftHandFollower.transform.position;
                        transform.parent.transform.eulerAngles = GorillaLocomotion.Player.Instance.leftHandTransform.transform.eulerAngles;
                    } else
                    {
                        transform.parent.transform.position = GorillaLocomotion.Player.Instance.rightHandFollower.transform.position;
                        transform.parent.transform.eulerAngles = GorillaLocomotion.Player.Instance.rightHandTransform.transform.eulerAngles;
                    }
                }
            }
        }

        void SelfDeath()
        {
            GameObject.Destroy(transform.parent.gameObject);
            GameObject.Destroy(gameObject);
        }
    }
}
