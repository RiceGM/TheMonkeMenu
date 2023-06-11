using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TheMonkeMenu.Menu.Mods;
using TheMonkeMenu.Menu.Mods.Platforms;
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
        public GameObject platformModel;
        bool canUseMenu = false;
        bool initialized = false;
        float menuAnimationSpeed = 5f;
        Texture forestAtlas;

        void Start()
        {
            instance = this;
        }

        public void InitializeMenu()
        {
            Debug.Log("Initializing Menu...");

            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(menu.GetComponent<Rigidbody>());
            GameObject.Destroy(menu.GetComponent<BoxCollider>());
            platform = new GameObject("ActualPlatform");
            menu.name = "MonkeMenu";

            gameObject.AddComponent<ModHelper>();
            gameObject.AddComponent<Platforms>().modEnabled = modsEnabled[2];

            Debug.Log("Initializing AssetBundles...");

            AssetBundle platformsBundle = AssetBundle.LoadFromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("TheMonkeMenu.Resources.Mods.platformsab"));
            platformModel = Instantiate(platformsBundle.LoadAsset<GameObject>("PlatformsGO"));
            platformModel.name = "Platform";
            platformModel.AddComponent<PlatformObject>();
            platformModel.transform.parent = platform.transform;
            platformModel.transform.eulerAngles = new Vector3(90, 0, 0);
            platformsBundle.Unload(false);
            
            Debug.Log("Changing Scales...");
            platformModel.transform.localScale = new Vector3(0.1f, 0.1f, 0.2f);
            Debug.Log("Loading Textures...");
            forestAtlas = GameObject.Find("Level/forest/ForestObjects/bridge").GetComponent<MeshRenderer>().sharedMaterials[0].mainTexture;

            Debug.Log("Setting Textures...");
            platformModel.GetComponent<MeshRenderer>().material = new Material(GorillaTagger.Instance.offlineVRRig.mainSkin.sharedMaterials[0].shader);
            platformModel.GetComponent<MeshRenderer>().material.mainTexture = forestAtlas;

            initialized = true;
        }

        public void Update()
        {
            if (ModHelper.instance.rightGrip)
            {
                menu.SetActive(true);
                if(menu.transform.localScale.x < 1)
                {
                    menu.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f) * Time.fixedDeltaTime * menuAnimationSpeed;
                }
                menu.transform.position = GorillaLocomotion.Player.Instance.rightHandTransform.position;
            } else
            {
                if (menu.transform.localScale.x > 0)
                {
                    menu.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f) * Time.fixedDeltaTime * menuAnimationSpeed;
                } else
                {
                    menu.SetActive(false);
                }
            }
        }
    }
}
