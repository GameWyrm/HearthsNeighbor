using OWML.Common;
using OWML.ModHelper;

namespace OuterWildsSummerJam
{
    public class OuterWildsSummerJam : ModBehaviour
    {
        public static bool debugMode = true;

        private INewHorizons nh;

        public static OuterWildsSummerJam Main
        {
            get
            {
                if (Main == null) main = FindObjectOfType<OuterWildsSummerJam>();
                return main;
            }
        }
        private static OuterWildsSummerJam main;

        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        private void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(OuterWildsSummerJam)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            nh = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            nh.LoadConfigs(this);

            // Example of accessing game code.
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                if (loadScene != OWScene.SolarSystem) return;
                ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
                if (nh.GetCurrentStarSystem() == "GameWyrm.JamSystem") InitSystem();
            };
        }

        /// <summary>
        /// Initialize our custom system
        /// </summary>
        private void InitSystem()
        {

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