using BrilliantSkies.Core.Logger;
using BrilliantSkies.Effects.Regulation;
using BrilliantSkies.FromTheDepths.Game;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MTMTVFX.Core
{
    public static class Util
    {
        public static bool DEBUG_MODE { get; private set; } = false;

        public static bool E_MUZZLE { get; private set; } = false;
        public static int COUNT_MUZZLE { get; private set; } = 0;
        public static bool E_RAILGUN { get; private set; } = false;
        public static int COUNT_RAILGUN { get; private set; } = 0;
        public static bool E_EXPL { get; private set; } = false;
        public static int COUNT_EXPL { get; private set; } = 0;
        public static bool E_PULSE { get; private set; } = false;
        public static int COUNT_PULSE { get; private set; } = 0;
        public static bool E_PAC { get; private set; } = false;
        public static int COUNT_PAC { get; private set; } = 0;
        public static bool E_PLASMA { get; private set; } = false;
        public static int COUNT_PLASMA { get; private set; } = 0;
        public static bool E_FLAMER { get; private set; } = false;
        public static int COUNT_FLAMER { get; private set; } = 0;

        public static bool E_CONTINUOUS { get; private set; } = false;
        public static bool IS_DEGRADED
        {
            get
            {
                if (!_init) InitConfig();
                return !e_in_degraded && !ProcessorLoading.Instance.FullMode;
            }
        }
        private static bool e_in_degraded = false;
        private static bool _init = false;



        /// <summary>
        /// Calls AdvLogger.LogInfo with generated file path and member info
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="option">The logging option</param>
        /// <param name="file">Leave this empty</param>
        /// <param name="member">Leave this empty</param>
        /// <param name="line">Leave this empty</param>
        public static void LogInfo<T>(
            string message,
            LogOptions option = LogOptions.None,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            if (!DEBUG_MODE) return;
            string ns = typeof(T).Namespace ?? "";
            AdvLogger.LogInfo($"[{ns}.{Path.GetFileName(file)}:{line} in {member}]\n\t{message}", option);
        }

        /// <summary>
        /// Calls AdvLogger.LogError with generated file path and member info
        /// </summary>
        /// <param name="message">The error to log</param>
        /// <param name="option">The logging option</param>
        /// <param name="file">Leave this empty</param>
        /// <param name="member">Leave this empty</param>
        /// <param name="line">Leave this empty</param>
        public static void LogError<T>(
            string message,
            LogOptions option = LogOptions.None,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            string ns = typeof(T).Namespace ?? "";
            AdvLogger.LogError($"[{ns}.{Path.GetFileName(file)}:{line} in {member}]\n\t{message}", option);
        }

        /// <summary>
        /// Find the mod's assetbundle GUID by file name
        /// </summary>
        /// <param name="filename">Name of the assetbundle json file, usually "name_*.assetbundle"</param>
        /// <returns></returns>
        public static string GetAssetbundleGUID(string filename)
        {
            string dllDir = Assembly.GetExecutingAssembly().Location;
            string modFolder = Path.Combine(Path.GetDirectoryName(dllDir), "Asset Bundles");
            string[] files = Directory.GetFiles(modFolder, filename);
            string json = File.ReadAllText(files[0]);
            var obj = JObject.Parse(json);
            return (string)obj["ComponentId"]["Guid"];
        }

        /// <summary>
        /// Get all the configs
        /// </summary>
        public static void InitConfig()
        {
            try
            {
                if (_init) return;

                string dllPath = Assembly.GetExecutingAssembly().Location;
                string dllDir = Path.GetDirectoryName(dllPath);
                string configPath = Path.Combine(dllDir, "config.json");
                string json = File.ReadAllText(configPath);
                var obj = JObject.Parse(json);

                DEBUG_MODE = (bool)obj["config"]["debug_mode"];

                E_MUZZLE = (bool)obj["config"]["muzzle"]["enabled"];
                COUNT_MUZZLE = (int)obj["config"]["muzzle"]["maxCount"];

                E_RAILGUN = (bool)obj["config"]["railgun"]["enabled"];
                COUNT_RAILGUN = (int)obj["config"]["railgun"]["maxCount"];

                E_EXPL = (bool)obj["config"]["explosion"]["enabled"];
                COUNT_EXPL = (int)obj["config"]["explosion"]["maxCount"];

                E_PULSE = (bool)obj["config"]["laser_pulse"]["enabled"];
                COUNT_PULSE = (int)obj["config"]["laser_pulse"]["maxCount"];

                E_PAC = (bool)obj["config"]["pac"]["enabled"];
                COUNT_PAC = (int)obj["config"]["pac"]["maxCount"];

                E_PLASMA = (bool)obj["config"]["plasma"]["enabled"];
                COUNT_PLASMA = (int)obj["config"]["plasma"]["maxCount"];

                E_FLAMER = (bool)obj["config"]["flamer"]["enabled"];
                COUNT_FLAMER = (int)obj["config"]["flamer"]["maxCount"];

                E_CONTINUOUS = (bool)obj["config"]["laser_continuous"]["enabled"];
                e_in_degraded = (bool)obj["config"]["enable_during_degraded"];

                _init = true;
            }
            catch (Exception e)
            {
                LogError<CorePlugin>(e.Message);
            }
        }

        public static GameObject[] GetChildren(this GameObject self, bool includeInactive = false)
        {
            return (from c in self.GetComponentsInChildren<Transform>(includeInactive)
                    where c != self.transform
                    select c.gameObject).ToArray<GameObject>();
        }

        /// <summary>
        /// Dummy method, patch this to add or run custom scripts on the objects
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="modName"></param>
        public static void AddScript(GameObject obj, Enum type, string modName) { }
    }

    [HarmonyPatch(typeof(AutoBattle), "Start")]
    public class Loader
    {
        private static void Postfix()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                VFXManager.Instance.Init();
            });
        }
    }
}
