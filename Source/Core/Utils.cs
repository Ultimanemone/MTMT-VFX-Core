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
using MTMTVFX.UI;
using BrilliantSkies.PlayerProfiles;

namespace MTMTVFX.Core
{
    public static class Utils
    {
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
            if (!ProfileManager.Instance.GetModule<SettingsConfig>().DEBUG_MODE) return;
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
        /// Dummy method, patch this to add or run custom scripts on the object
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
