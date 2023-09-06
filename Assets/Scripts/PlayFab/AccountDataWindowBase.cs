using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlayFab
{
    public class AccountDataWindowBase : MonoBehaviour
    {
        [SerializeField] private InputField _usernameField;
        [SerializeField] private InputField _passwordField;

        protected string _username;
        protected string _password;

        private void Start()
        {
            SubscriptionsElementsUi();
        }

        protected virtual void SubscriptionsElementsUi()
        {
            _usernameField.onValueChanged.AddListener(UpdateUsername);
            _passwordField.onValueChanged.AddListener(UpdatePassword);
        }

        private void UpdatePassword(string password)
        {
            _password = password;
        }

        private void UpdateUsername(string username)
        {
            _username = username;
        }

        protected void EnterInGameScene()
        {
            Loading.Instance.LoadingState(true);
            SceneManager.LoadScene(1);
        }
    }
}