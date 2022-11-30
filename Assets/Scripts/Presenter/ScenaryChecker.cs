using System;
using Extensions;
using UnityEngine;

namespace Presenter
{
    public class ScenaryChecker : MonoBehaviour
    {
        [SerializeField] private GameObject _loading;
        
        [SerializeField] private GameObject _noInternet;

        [SerializeField] private FirebaseConnector _firebaseConnector;

        [SerializeField] private GameObject _serverError;

        [SerializeField] private UniWebView _uniWebView;

        [SerializeField] private GameObject _block;
        
        private const string SavedLinkKey = "SavedLinkKey";

        public string SavedLink
        {
            get => PlayerPrefs.GetString(SavedLinkKey);
            set => PlayerPrefs.SetString(SavedLinkKey, value);
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(SavedLink))
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    _block.SetActive(false);
                    
                    _noInternet.gameObject.SetActive(true);
                }
                else
                {
                    OpenWebView(SavedLink);
                }
            }
            else
            {
                _loading.SetActive(true);

                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    _noInternet.SetActive(true);
                    
                    _loading.SetActive(false);
                    
                    _block.SetActive(false);
                    
                    return;
                }
                
                // Подключенгие firebase
                _firebaseConnector.GetLink(delegate(string link)
                {
                    _loading.SetActive(false);

                    bool canOpen = Application.internetReachability == NetworkReachability.NotReachable ||
                                   DeviceTester.ChecksEmulator() || string.IsNullOrEmpty(link);
                    
                    Debug.Log($"Result link: {link}");
                    
                    Debug.Log($"Check can open: {canOpen}");
                    
                    Debug.Log(Application.internetReachability == NetworkReachability.NotReachable);
                    
                    Debug.Log(DeviceTester.ChecksEmulator());

                    if (DeviceTester.ChecksEmulator() || string.IsNullOrEmpty(link))
                    {
                        _block.SetActive(false);
                    }
                    else
                    {
                        SavedLink = link;
                        
                        OpenWebView(link);
                    }
                }, delegate
                {
                    _loading.SetActive(false);
                    
                    _block.SetActive(false);
                    
                    _serverError.SetActive(true);
                });
            }
        }

        private void OpenWebView(string link)
        {
            Screen.autorotateToLandscapeLeft = true;

            Screen.autorotateToLandscapeRight = true;

            Debug.Log($"Opened link: {link}");
            
            _uniWebView.Show();
            
            _uniWebView.Load(link);
        }
    }
}