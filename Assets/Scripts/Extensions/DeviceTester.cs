using UnityEngine;

namespace Extensions
{
    public static class DeviceTester
    {
        public static bool ChecksEmulator()
        {
            AndroidJavaClass unityClass;
            AndroidJavaObject unityActivity;
            AndroidJavaObject pluginInstance;
            
            unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            
            pluginInstance = new AndroidJavaObject("com.example.libraryforunity.PluginInstance");

            if (pluginInstance == null)
            {
                Debug.LogError("Plugin error");
                
                return false;
            }

            var result = pluginInstance.Call<int>("Add", 5, 6);
            
            Debug.Log(result);

            bool isEmulator = pluginInstance.Call<bool>("CheckIsEmulator");
            
            Debug.Log(isEmulator);

            string model = pluginInstance.Call<string>("GetPhoneModel");
            
            Debug.Log(model);
            
            return isEmulator;
        }
        
        private static readonly string[] s_BlueStacksDirSigs =
        {
            "/sdcard/windows/BstSharedFolder",
            "/mnt/windows/BstSharedFolder",
        };
 
        public static bool IsRunningBlueStacks()
        {
            foreach (string dir in s_BlueStacksDirSigs)
                if (System.IO.Directory.Exists(dir))
                    return true;
 
            return false;
        }
    }
}