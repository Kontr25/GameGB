using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFab
{
    public class EnterInGameWindow: MonoBehaviour
    {
        [SerializeField] private Button _signInButton;
        [SerializeField] private Button _createAccountButton;
        [SerializeField] private Canvas _enterInGameCanvas;
        [SerializeField] private Canvas _createAccounCanvas;
        [SerializeField] private Canvas _signInCanvas;

        private void Start()
        {
            _signInButton.onClick.AddListener(OpenSognInWindow);
            _createAccountButton.onClick.AddListener(OpenCreateAccounWindow);
        }

        private void OpenSognInWindow()
        {
            _signInCanvas.enabled = true;
            _enterInGameCanvas.enabled = false;
        }

        private void OpenCreateAccounWindow()
        {
            _createAccounCanvas.enabled = true;
            _enterInGameCanvas.enabled = false;
        }
    }
}