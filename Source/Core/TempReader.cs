using BrilliantSkies.Core.Constants;
using BrilliantSkies.PlayerProfiles;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTMTVFX.Core
{
    public static class TempReader
    {
        public static readonly object LOCK = new object();
        private static JObject jObject = new JObject();

        private static DirectoryInfo GetDir2()
        {
            return new DirectoryInfo(Get.ProfilePaths.ProfileDetailsDir().ToString());
        }
        private static string GetDir()
        {
            DirectoryInfo temp = new DirectoryInfo(Get.ProfilePaths.ProfileRootDir().ToString());
            DirectoryInfo parent = temp.Parent;
            string path = Path.Combine(parent.ToString(), ProfileManager.Instance.GetProfileName(), "Profile");
            return path;
        }

        public static void LoadJson()
        {
            lock (LOCK)
            {
                string path = Path.Combine(GetDir().ToString(), "profile.MTMTConfig");
                if (File.Exists(path))
                {
                    jObject = JObject.Parse(File.ReadAllText(path));
                }
            }
        }

        public static string Read(string name)
        {
            JToken token = jObject[name];
            return token != null ? token.ToString() : "";
        }

        public static string LoadAndRead(string name)
        {
            LoadJson();
            return Read(name);
        }
    }
}
