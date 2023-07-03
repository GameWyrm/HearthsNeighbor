using OWML.Common;
using OWML.ModHelper;
using System.Collections.Generic;
using UnityEngine;

namespace OuterWildsSummerJam
{
    public class OuterWildsSummerJam : ModBehaviour
    {
        /// <summary>
        /// Determines whether we should print non-critical messages to the console and enables other dev features
        /// </summary>
        public static bool debugMode = true;
        /// <summary>
        /// Whether the player is in an elevator
        /// </summary>
        public bool isInElevator;

        private INewHorizons nh;
        private Dictionary<string, SeamlessPlayerWarp> warpList;

        public static OuterWildsSummerJam Main
        {
            get
            {
                if (main == null) main = FindObjectOfType<OuterWildsSummerJam>();
                return main;
            }
        }
        private static OuterWildsSummerJam main;

        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
            warpList = new Dictionary<string, SeamlessPlayerWarp>();
        }

        private void Start()
        {
            // Get the New Horizons API and load configs
            nh = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            nh.LoadConfigs(this);

            // Example of accessing game code.
            /*LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                if (loadScene != OWScene.SolarSystem) return;
                ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
                if (nh.GetCurrentStarSystem() == "GameWyrm.JamSystem") InitSystem();
            };*/

            nh.GetStarSystemLoadedEvent().AddListener(InitSystem);
        }

        /// <summary>
        /// Initialize our custom system
        /// </summary>
        private void InitSystem(string systemName)
        {
            isInElevator = false;
            foreach (string warp in warpList.Keys)
            {
                switch (warp)
                {
                    case ("MainLake"):
                        warpList[warp].linkedWarp = warpList["LakeEntrance"].gameObject;
                        break;
                    case ("LakeEntrance"):
                        warpList[warp].linkedWarp = warpList["MainLake"].gameObject;
                        break;
                }
            }
        }

        /// <summary>
        /// Registers an elevator start or end point so they can be connected
        /// </summary>
        public static void RegisterWarp(GameObject warpObject)
        {
            if (warpObject.GetComponentInChildren<SeamlessPlayerWarp>() != null)
            {
                Main.warpList.Add(warpObject.name, warpObject.GetComponentInChildren<SeamlessPlayerWarp>());
                LogSuccess($"Registered warp {warpObject.name}!");
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
    }
}