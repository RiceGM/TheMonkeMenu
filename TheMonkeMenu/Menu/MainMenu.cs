using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TheMonkeMenu.Menu.Mods;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.XR;

namespace TheMonkeMenu.Menu
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        public static MainMenu instance;
        public MonkePatcher monkePatcher;

        public bool[] modsEnabled;
        public string[] monkeMods = new string[]
        {
            "Fly",
            "NoClip",
            "Platforms"
        };

        bool closingAnimation;
        GameObject menu;
        public GameObject platform;
        bool canUseMenu = false;
        bool initialized = false;

        Texture forestAtlas;

        void Start()
        {
            instance = this;
        }

        public void InitializeMenu()
        {
            Debug.Log("Initializing Menu...");

            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            menu.name = "MonkeMenu";

            gameObject.AddComponent<ModHelper>();
            gameObject.AddComponent<Platforms>().modEnabled = true;

            Debug.Log("Initializing AssetBundles...");

            AssetBundle platformsBundle = AssetBundle.LoadFromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("TheMonkeMenu.Resources.Mods.platformsab"));
            platform = Instantiate(platformsBundle.LoadAsset<GameObject>("PlatformsGO"));
            platform.name = "Platform";
            platformsBundle.Unload(false);

            Debug.Log("Changing Scales...");
            platform.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            Debug.Log("Loading Textures...");
            forestAtlas = GameObject.Find("Level/forest/ForestObjects/bridge").GetComponent<MeshRenderer>().sharedMaterials[0].mainTexture;

            Debug.Log("Setting Textures...");
            platform.GetComponent<MeshRenderer>().material = new Material(GorillaTagger.Instance.offlineVRRig.mainSkin.sharedMaterials[0].shader);
            platform.GetComponent<MeshRenderer>().material.mainTexture = forestAtlas;
            initialized = true;
        }

        public void Update()
        {
            MenuClosingChecks();
        }

        void MenuClosingChecks()
        {
            if(!initialized) return;
            if (ModHelper.instance.rightGrip && CanGrabMenu() && !menu.activeInHierarchy)
            {
                menu.SetActive(true);
                StartCoroutine(MenuPopInAnimation(true));
            }

            if (!ModHelper.instance.rightGrip && !closingAnimation && menu.activeInHierarchy)
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

        bool CanGrabMenu()
        {
            float dot = Vector3.Dot(GorillaLocomotion.Player.Instance.rightHandTransform.position - GorillaLocomotion.Player.Instance.transform.position, GorillaLocomotion.Player.Instance.transform.forward);
            return dot < -0.1f;
        }
    }
}
