using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.XR;

namespace TheMonkeMenu.Menu
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        public MonkePatcher monkePatcher;

        public bool[] modsEnabled;
        public string[] monkeMods = new string[]
        {
            "Fly",
            "NoClip",
            "Platforms"
        };

        bool aBtn, bBtn, xBtn, yBtn;
        bool leftTrigger, rightTrigger, leftGrip, rightGrip;

        bool closingAnimation;
        GameObject menu;

        public void InitializeMenu()
        {
            Debug.Log("Initializing Menu...");
            menu = new GameObject("MonkeModMenu");
        }

        public void Update()
        {
            GetMenuInputs();

            if(rightGrip && CanGrabMenu() && !menu.activeInHierarchy)
            {
                menu.SetActive(true);
                StartCoroutine(MenuPopInAnimation(true));
            }

            if(!rightGrip && !closingAnimation && menu.activeInHierarchy)
            {
                StartCoroutine(MenuPopInAnimation(false));
            }
        }

        IEnumerator MenuPopInAnimation(bool open)
        {
            menu.SetActive(true);

            if (!open) // menu is closing
            {
                closingAnimation = true;
                for (int i = 100; i > 0; i--)
                {
                    menu.transform.localScale = new Vector3(i / 100, i / 100, i / 100);
                    yield return new WaitForEndOfFrame();
                }

                menu.SetActive(false);
                closingAnimation = false;
            } else
            {
                menu.transform.localScale = new Vector3(0, 0, 0);
                for (int i = 0; i < 100; i++)
                {
                    menu.transform.localScale = new Vector3(i / 100, i / 100, i / 100);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        void GetMenuInputs()
        {
            List<InputDevice> leftList = new List<InputDevice>();
            List<InputDevice> rightList = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, leftList);
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightList);
            leftList[0].TryGetFeatureValue(CommonUsages.gripButton, out leftGrip);
            leftList[0].TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger);
            leftList[0].TryGetFeatureValue(CommonUsages.primaryButton, out yBtn);
            leftList[0].TryGetFeatureValue(CommonUsages.secondaryButton, out xBtn);

            rightList[0].TryGetFeatureValue(CommonUsages.triggerButton, out rightTrigger);
            rightList[0].TryGetFeatureValue(CommonUsages.gripButton, out rightGrip);
            rightList[0].TryGetFeatureValue(CommonUsages.primaryButton, out bBtn);
            rightList[0].TryGetFeatureValue(CommonUsages.secondaryButton, out aBtn);
        }

        bool CanGrabMenu()
        {
            float dot = Vector3.Dot(GorillaLocomotion.Player.Instance.rightHandTransform.position - GorillaLocomotion.Player.Instance.transform.position, GorillaLocomotion.Player.Instance.transform.forward);
            return dot < -0.1f;
        }
    }
}
