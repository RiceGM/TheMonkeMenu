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
        GameObject platform;
        bool canUseMenu = false;
        bool initialized = false;

        Texture forestAtlas;

        public void InitializeMenu()
        {
            Debug.Log("Initializing Menu...");

            menu = new GameObject("MonkeModMenu");

            Debug.Log("Initializing AssetBundles...");

            AssetBundle platformsBundle = AssetBundle.LoadFromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("TheMonkeMenu.Resources.Mods.platformsab"));
            platform = Instantiate(platformsBundle.LoadAsset<GameObject>("PlatformsGO"));
            platform.name = "Platform";
            platformsBundle.Unload(false);

            Debug.Log("Loading Textures...");
            forestAtlas = GameObject.Find("Level/forest/ForestObjects/bridge").GetComponent<MeshRenderer>().sharedMaterials[0].mainTexture;

            Debug.Log("Setting Textures...");
            platform.GetComponent<MeshRenderer>().material = new Material(GorillaTagger.Instance.offlineVRRig.mainSkin.sharedMaterials[0].shader);
            platform.GetComponent<MeshRenderer>().material.mainTexture = forestAtlas;
            initialized = true;
        }

        public void Update()
        {
            GetMenuInputs();
            MenuClosingChecks();
        }

        void MenuClosingChecks()
        {
            if(!initialized) return;
            if (rightGrip && CanGrabMenu() && !menu.activeInHierarchy)
            {
                menu.SetActive(true);
                StartCoroutine(MenuPopInAnimation(true));
            }

            if (!rightGrip && !closingAnimation && menu.activeInHierarchy)
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
            // fuck off error >:(
            // whgere is error :(((
            // https://github.com/RiceGM/TheMonkeMenu/assets/122515661/0c1f0328-5818-47e3-a99f-eca45f91e46c
            List<InputDevice> leftList = new List<InputDevice>();
            List<InputDevice> rightList = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, leftList);
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightList);
            if(leftList[0]!= null) leftList[0].TryGetFeatureValue(CommonUsages.gripButton, out leftGrip);
            if(leftList[0]!= null) leftList[0].TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger);
            if(leftList[0]!= null) leftList[0].TryGetFeatureValue(CommonUsages.primaryButton, out yBtn);
            if(leftList[0]!= null) leftList[0].TryGetFeatureValue(CommonUsages.secondaryButton, out xBtn);

            if(rightList[0] != null) rightList[0].TryGetFeatureValue(CommonUsages.triggerButton, out rightTrigger);
            if(rightList[0] != null) rightList[0].TryGetFeatureValue(CommonUsages.gripButton, out rightGrip);
            if(rightList[0] != null) rightList[0].TryGetFeatureValue(CommonUsages.primaryButton, out bBtn);
            if(rightList[0] != null) rightList[0].TryGetFeatureValue(CommonUsages.secondaryButton, out aBtn);
        }

        bool CanGrabMenu()
        {
            float dot = Vector3.Dot(GorillaLocomotion.Player.Instance.rightHandTransform.position - GorillaLocomotion.Player.Instance.transform.position, GorillaLocomotion.Player.Instance.transform.forward);
            return dot < -0.1f;
        }
    }
}
