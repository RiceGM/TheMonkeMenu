using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheMonkeMenu.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilla;

namespace TheMonkeMenu
{
    [BepInPlugin("org.ricegm.thatmonkemenu", "ThatMonkeMenu", "1.0.0")]
    [BepInDependency("")]
    [ModdedGamemode]
    public class MonkePatcher : BaseUnityPlugin
    {
        GameObject mainMenu;
        MainMenu mainMenuLocal;
        Harmony harmony;

        [ModdedGamemodeJoin]
        void JoinModded()
        {
            mainMenuLocal.isInModded = true;
        }

        [ModdedGamemodeLeave]
        void LeaveModded()
        {
            mainMenuLocal.isInModded = false;
        }

        void OnEnable()
        {
            if (harmony == null) harmony = new Harmony("org.ricegm.thatmonkemenu");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            SceneManager.sceneLoaded += InitializeMonkeMenu;
        }

        void InitializeMonkeMenu(Scene arg0, LoadSceneMode arg1)
        {
            if (!mainMenu)
            {
                mainMenu = new GameObject("MonkeMenu");
                GameObject.DontDestroyOnLoad(mainMenu);
                mainMenuLocal = mainMenu.AddComponent<MainMenu>();
                mainMenuLocal.modsEnabled = new bool[mainMenuLocal.monkeMods.Length];
                mainMenuLocal.monkePatcher = this;

                mainMenuLocal.modsEnabled[0] = Config.Bind("EnabledMods", "Pen", false, "Is the Pen mod enabled?").Value;
                mainMenuLocal.modsEnabled[1] = Config.Bind("EnabledMods", "Platforms", false, "Is the Platforms mod enabled?").Value;

                mainMenuLocal.InitializeMenu();
            }
        }

        void OnDisable()
        {
            harmony.UnpatchSelf();
        }

        public void ChangeConfig(int modID, string modName, string modSection, bool enabled)
        {
            Config.TryGetEntry(modSection, modName, out ConfigEntry<bool> entry);
            entry.Value = enabled;
            Config.Save();
            mainMenuLocal.modsEnabled[modID] = enabled;
        }
    }
}