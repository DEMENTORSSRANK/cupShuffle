using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine;

namespace Presenter
{
    public class FirebaseConnector : MonoBehaviour
    {
        private void Awake()
        {
            // System.Collections.Generic.Dictionary<string, object> defaults =
            //     new System.Collections.Generic.Dictionary<string, object>();
            //
            // defaults.Add("link", "");
            //
            // Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            //     .ContinueWithOnMainThread(delegate(Task task) { });
        }

        public Task GetLink(Action<string> resulted, Action error)
        {
            Debug.Log("Fetching data...");

            try
            {
                Task fetchTask =
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();

                return fetchTask.ContinueWithOnMainThread(delegate(Task task)
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Faulted");
                        
                        return;
                    }

                    resulted?.Invoke(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("url").StringValue);
                });
            }
            catch (Exception e)
            {
                error?.Invoke();
                
                throw;
            }
        }
    }
}