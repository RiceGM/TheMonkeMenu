using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TheMonkeMenu.Menu.Mods;
using TheMonkeMenu.Menu.Mods.Pen;
using TheMonkeMenu.Menu.Mods.Platforms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;
using UnityEngine.XR;

namespace TheMonkeMenu.Menu
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        public static MainMenu instance;
        public MonkePatcher monkePatcher;
        public bool isInModded;

        public bool[] modsEnabled;
        public string[] monkeMods = new string[]
        {
            "Pen",
            "Platforms"
        };

        bool closingAnimation;
        GameObject menu;
        public GameObject platform, pen;
        public GameObject platformModel, penModel;

        bool canUseMenu = false;
        bool initialized = false;
        float menuAnimationSpeed = 5f;
        Texture forestAtlas;

        GameObject monkeBundle;

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
            pen = new GameObject("ActualPen");
            menu.name = "MonkeMenu";

            Debug.Log("Initializing AssetBundles...");

            InitializeBundles();
            
            Debug.Log("Changing Scales...");
            platformModel.transform.localScale = new Vector3(0.1f, 0.1f, 0.2f);
            penModel.transform.localScale = new Vector3(25f, 25f, 25f);
            Debug.Log("Loading Textures...");
            forestAtlas = GameObject.Find("Level/forest/ForestObjects/bridge").GetComponent<MeshRenderer>().sharedMaterials[0].mainTexture;

            Debug.Log("Setting Textures...");
            platformModel.GetComponent<MeshRenderer>().material = new Material(GorillaTagger.Instance.offlineVRRig.mainSkin.sharedMaterials[0].shader);
            platformModel.GetComponent<MeshRenderer>().material.mainTexture = forestAtlas;

            penModel.GetComponent<MeshRenderer>().material = new Material(GorillaTagger.Instance.offlineVRRig.mainSkin.sharedMaterials[0].shader);
            penModel.GetComponent<MeshRenderer>().material.mainTexture = forestAtlas;

            Debug.Log("Setting Transforms...");
            platformModel.transform.parent = platform.transform;
            penModel.transform.parent = pen.transform;

            platformModel.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

            Debug.Log("Initializing Mods...");
            gameObject.AddComponent<ModHelper>();
            penModel.transform.GetChild(0).gameObject.AddComponent<Mods.Pen.Pen>().modEnabled = modsEnabled[0];
            gameObject.AddComponent<Platforms>().modEnabled = modsEnabled[1];

            Debug.Log("Adding Scripts to Objects...");

            platformModel.AddComponent<PlatformObject>().isMaster = true;

            initialized = true;
        }

        void InitializeBundles()
        {
            AssetBundle monkeBundleAB = AssetBundle.LoadFromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("TheMonkeMenu.Resources.Mods.monkebundle"));
            monkeBundle = Instantiate(monkeBundleAB.LoadAsset<GameObject>("MonkeBundle"));

            penModel = monkeBundle.transform.GetChild(0).gameObject;
            platformModel = monkeBundle.transform.GetChild(1).gameObject;

            monkeBundleAB.Unload(false);
        }

        public void Update()
        {
            return;
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
