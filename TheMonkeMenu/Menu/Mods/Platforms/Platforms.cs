using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMonkeMenu.Menu.Mods.Platforms
{
    public class Platforms : MonkeMod
    {
        GameObject leftPlatform, rightPlatform;

        void Update()
        {
            if (!modEnabled || !MainMenu.instance.isInModded) return; // is from MonkeMod.cs

            // left platform
            if(ModHelper.instance.leftGrip && !leftPlatform)
            {
                leftPlatform = GameObject.Instantiate(MainMenu.instance.platform);
                leftPlatform.transform.GetChild(0).GetComponent<PlatformObject>().isMaster = false;
                leftPlatform.transform.position = GorillaLocomotion.Player.Instance.leftHandFollower.transform.position;
                leftPlatform.transform.eulerAngles = GorillaLocomotion.Player.Instance.leftHandTransform.transform.eulerAngles;
            } else if(!ModHelper.instance.leftGrip && leftPlatform)
            {
                leftPlatform.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                leftPlatform.transform.GetChild(0).GetComponent<PlatformObject>().isUnEquipped = true;
                leftPlatform = null;
            }

            // right platform (copy paste from left)
            if (ModHelper.instance.rightGrip && !rightPlatform)
            {
                rightPlatform = GameObject.Instantiate(MainMenu.instance.platform);
                rightPlatform.transform.GetChild(0).GetComponent<PlatformObject>().isMaster = false;
                rightPlatform.transform.GetChild(0).GetComponent<PlatformObject>().isLeftHand = false;
                rightPlatform.transform.position = GorillaLocomotion.Player.Instance.rightHandFollower.transform.position;
                rightPlatform.transform.eulerAngles = GorillaLocomotion.Player.Instance.rightHandTransform.transform.eulerAngles;
            }
            else if (!ModHelper.instance.rightGrip && rightPlatform)
            {
                rightPlatform.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                rightPlatform.transform.GetChild(0).GetComponent<PlatformObject>().isUnEquipped = true;
                rightPlatform = null;
            }
        }
    }
}
