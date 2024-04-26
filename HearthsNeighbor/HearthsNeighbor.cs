using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HearthsNeighbor
{
    public class HearthsNeighbor : ModBehaviour
    {
        /// <summary>
        /// Determines whether we should print non-critical messages to the console and enables other dev features
        /// </summary>
        public static bool debugMode = false;
        /// <summary>
        /// Whether the player is in an elevator
        /// </summary>
        public bool isInElevator;
        public GameObject MainPlanet;
        public GameObject AlpinePlanet;
        public GameObject LakePlanet;
        public GameObject LavaPlanet;
        public GameObject DerelictShip;
        public INewHorizons nh;

        private Dictionary<string, SeamlessPlayerWarp> warpList;

        public static HearthsNeighbor Main
        {
            get
            {
                if (main == null) main = FindObjectOfType<HearthsNeighbor>();
                return main;
            }
        }
        private static HearthsNeighbor main;

        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
            warpList = new Dictionary<string, SeamlessPlayerWarp>();

            // Prepare for patching
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        private void Start()
        {
            // Get the New Horizons API and load configs
            nh = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            nh.LoadConfigs(this);

            // Runs every time our system loads
            nh.GetStarSystemLoadedEvent().AddListener(InitSystem);
        }

        /// <summary>
        /// Initialize our custom system
        /// </summary>
        private void InitSystem(string systemName)
        {
            if (systemName == "GameWyrm.HearthsNeighbor")
            {

                LogMessage("Loaded into jam system");

                MainPlanet = GameObject.Find("LonelyHermit_Body");
                AlpinePlanet = GameObject.Find("AlpineCore_Body");
                LakePlanet = GameObject.Find("LakeCore_Body");
                LavaPlanet = GameObject.Find("LavaCore_Body");
                DerelictShip = GameObject.Find("DerelictShip_Body");

                isInElevator = false;
                foreach (string warp in warpList.Keys)
                {
                    LogMessage($"Determining destination for {warp}");
                    LogMessage($"Position: {warpList[warp].transform.position}, Body: {warpList[warp].GetAttachedOWRigidbody().name}");
                    switch (warp)
                    {
                        case ("MainLake"):
                            warpList[warp].linkedWarp = warpList["LakeEntrance"].gameObject;
                            LogSuccess($"Assigned {warp}({warpList[warp].transform.parent.parent.name}) to LakeEntrance! ({warpList[warp].linkedWarp.transform.parent.parent.name})");
                            break;
                        case ("MainAlpine"):
                            warpList[warp].linkedWarp = warpList["AlpineEntrance"].gameObject;
                            LogSuccess($"Assigned {warp}({warpList[warp].transform.parent.parent.name}) to AlpineEntrance! ({warpList[warp].linkedWarp.transform.parent.parent.name})");
                            break;
                        case ("MainLava"):
                            warpList[warp].linkedWarp = warpList["LavaEntrance"].gameObject;
                            LogSuccess($"Assigned {warp}({warpList[warp].transform.parent.parent.name}) to LavaEntrance! ({warpList[warp].linkedWarp.transform.parent.parent.name})");
                            break;
                        case ("LakeEntrance"):
                            warpList[warp].linkedWarp = warpList["MainLake"].gameObject;
                            LogSuccess($"Assigned {warp}({warpList[warp].transform.parent.parent.name}) to MainLake! ({warpList[warp].linkedWarp.transform.parent.parent.name})");
                            break;
                        case ("AlpineEntrance"):
                            warpList[warp].linkedWarp = warpList["MainAlpine"].gameObject;
                            LogSuccess($"Assigned {warp}({warpList[warp].transform.parent.parent.name}) to MainAlpine! ({warpList[warp].linkedWarp.transform.parent.parent.name})");
                            break;
                        case ("LavaEntrance"):
                            warpList[warp].linkedWarp = warpList["MainLava"].gameObject;
                            LogSuccess($"Assigned {warp}({warpList[warp].transform.parent.parent.name}) to MainLava! ({warpList[warp].linkedWarp.transform.parent.parent.name})");
                            break;
                    }
                }
                warpList.Clear();


                StartCoroutine(EndOfFrameInit());
            }
            else if (systemName == "SolarSystem")
            {
                GameObject endingDialogue = GameObject.Find("TimberHearth_Body").transform.Find("Sector_TH/HNEndingDialogue").gameObject;
                endingDialogue.AddComponent(typeof(MagistariumConnection));
            }
        }

        /// <summary>
        /// Registers an elevator start or end point so they can be connected
        /// </summary>
        public static void RegisterWarp(GameObject warpObject)
        {
            if (warpObject.GetComponentInChildren<SeamlessPlayerWarp>() != null)
            {
                string warpName = warpObject.name;
                Main.warpList.Add(warpName, warpObject.GetComponentInChildren<SeamlessPlayerWarp>());
                LogSuccess($"Registered warp {warpName}! Warplist now contains {Main.warpList.Keys.Count} entries.");
            }
        }


        /// <summary>
        /// Logs an info (blue text) to the console. Only applies in debug versions
        /// </summary>
        /// <param name="infoText">Text to print</param>
        public static void LogInfo(string infoText)
        {
            if (debugMode) Main.ModHelper.Console.WriteLine(infoText, MessageType.Info);
        }

        /// <summary>
        /// Logs a message (white text) to the console. Only applies in debug versions.
        /// </summary>
        /// <param name="infoText">Text to print</param>
        public static void LogMessage(string infoText)
        {
            if (debugMode) Main.ModHelper.Console.WriteLine(infoText, MessageType.Message);
        }

        /// <summary>
        /// Logs a success (green text) to the console. Only applies in debug versions
        /// </summary>
        /// <param name="infoText">Text to print</param>
        public static void LogSuccess(string infoText)
        {
            if (debugMode) Main.ModHelper.Console.WriteLine(infoText, MessageType.Success);
        }

        /// <summary>
        /// Logs a warning (yellow text) to the console
        /// </summary>
        /// <param name="infoText">Text to print</param>
        public static void LogWarning(string infoText)
        {
            Main.ModHelper.Console.WriteLine(infoText, MessageType.Warning);
        }

        /// <summary>
        /// Logs an error (red text) to the console
        /// </summary>
        /// <param name="infoText">Text to print</param>
        public static void LogError(string infoText)
        {
            Main.ModHelper.Console.WriteLine(infoText, MessageType.Error);
        }

        private IEnumerator EndOfFrameInit()
        {
            yield return new WaitForEndOfFrame();

            foreach (Transform obj in AlpinePlanet.transform)
            {
                obj.gameObject.SetActive(false);
            }
            foreach (Transform obj in LakePlanet.transform)
            {
                obj.gameObject.SetActive(false);
            }
            foreach (Transform obj in LavaPlanet.transform)
            {
                obj.gameObject.SetActive(false);
            }


            if (PlayerData.GetShipLogFactSave("HN_POD_RESOLUTION") != null && PlayerData.GetShipLogFactSave("HN_POD_RESOLUTION").revealOrder > -1)
            {
                Locator.GetShipLogManager().RevealFact("DS_ENDING");
            }

            GameObject.Find("Alpine Core_Proxy").SetActive(false);
            GameObject.Find("Lake Core_Proxy").SetActive(false);
            GameObject.Find("Lava Core_Proxy").SetActive(false);
        }
    }
}