using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFab
{
    public class CreateAccountWindow : AccountDataWindowBase
    {
        [SerializeField] private InputField _mailField;
        [SerializeField] private Button _createAccountButton;

        protected string _mail;

        protected override void SubscriptionsElementsUi()
        {
            base.SubscriptionsElementsUi();
            _mailField.onValueChanged.AddListener(UpdateMail);
            _createAccountButton.onClick.AddListener(CreateAccount);
        }

        private void CreateAccount()
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
            {
                Username = _username,
                Password = _password,
                Email = _mail
            }, result =>
            {
                Debug.Log($"Success: {_username}");
                EnterInGameScene();
            }, error =>
            {
                Debug.LogError($"Fail: {error.ErrorMessage}");
            });
        }

        private void UpdateMail(string mail)
        {
            _mail = mail;
        }
    }
}