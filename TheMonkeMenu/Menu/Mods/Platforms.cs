﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMonkeMenu.Menu.Mods
{
    public class Platforms : MonkeMod
    {
        GameObject leftPlatform, rightPlatform;

        void Update()
        {
            if (!modEnabled) return; // is from MonkeMod.cs

            // left platform
            if(ModHelper.instance.leftGrip && !leftPlatform)
            {
                leftPlatform = GameObject.Instantiate(MainMenu.instance.platform);
                leftPlatform.transform.position = GorillaLocomotion.Player.Instance.leftHandFollower.transform.position;
                leftPlatform.transform.rotation = GorillaLocomotion.Player.Instance.leftHandTransform.transform.rotation;
            } else if(!ModHelper.instance.leftGrip && leftPlatform)
            {
                GameObject.Destroy(leftPlatform);
            }

            // right platform (copy paste from left)
            if (ModHelper.instance.rightGrip && !rightPlatform)
            {
                rightPlatform = GameObject.Instantiate(MainMenu.instance.platform);
                rightPlatform.transform.position = GorillaLocomotion.Player.Instance.rightHandFollower.transform.position;
                rightPlatform.transform.rotation = GorillaLocomotion.Player.Instance.rightHandTransform.transform.rotation;
            }
            else if (!ModHelper.instance.rightGrip && rightPlatform)
            {
                GameObject.Destroy(rightPlatform);
            }
        }
    }
}
