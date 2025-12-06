using BrilliantSkies.Core.Logger;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MTMTVFX.Core
{
    public static class Constants
    {
        public static int maxMuzzleFlash = 64;
        public static int maxLaser = 64;
    }

    public class Util
    {
        /// <summary>
        /// Calls AdvLogger.LogInfo with generated file path and member info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="option"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        public static void LogInfo<T>(
            string message,
            LogOptions option = LogOptions.None,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            string ns = typeof(T).Namespace ?? "";
            AdvLogger.LogInfo($"[{ns}.{System.IO.Path.GetFileName(file)}:{line} in {member}]\n\t{message}", option);
        }

        /// <summary>
        /// Calls AdvLogger.LogError with generated file path and member info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="option"></param>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        public static void LogError<T>(
            string message,
            LogOptions option = LogOptions.None,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            string ns = typeof(T).Namespace ?? "";
            AdvLogger.LogError($"[{ns}.{System.IO.Path.GetFileName(file)}:{line} in {member}]\n\t{message}", option);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetAssetbundleGUID(string assetName)
        {
            string dllDir = Assembly.GetExecutingAssembly().Location;
            string modFolder = Path.Combine(Path.GetDirectoryName(dllDir), "Asset Bundles");
            string[] files = Directory.GetFiles(modFolder, assetName);
            string json = File.ReadAllText(files[0]);
            var obj = JObject.Parse(json);
            return (string)obj["ComponentId"]["Guid"];
        }
    }
}
