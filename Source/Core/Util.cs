using BrilliantSkies.Core.Logger;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MTMTVFX.Core
{
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
            AdvLogger.LogInfo($"[{ns}.{System.IO.Path.GetFileName(file)}:\n\t{line} in {member}] {message}", option);
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
            AdvLogger.LogError($"[{ns}.{System.IO.Path.GetFileName(file)}:\n\t{line} in {member}] {message}", option);
        }
    }
}
