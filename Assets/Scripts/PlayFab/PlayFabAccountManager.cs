using System.Collections.Generic;
using System.Linq;
using PlayFab.ClientModels;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFab
{
    public class PlayFabAccountManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleLabel;
        [SerializeField] private GameObject _newCharacterCreatePanel;
        [SerializeField] private Button _createCharacterButton;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private List<SlotCharacterWidget> _slots;

        private string _characterName;
        private void Start()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnFailure);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(),OnGetCatalogSuccess, OnFailure);
            PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest(), OnGetRandomResultTables, OnFailure);

            GetCharacters();

            foreach (var slot in _slots)
            {
                slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);
            }
            
            _inputField.onValueChanged.AddListener(OnNameChange);
            _createCharacterButton.onClick.AddListener(CreateCharacter);
        }

        private void CreateCharacter()
        {
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
            {
                CharacterName =  _characterName,
                ItemId = "character_token",
            }, result =>
            {
                UpdateCharacterStatistics(result.CharacterId);
            }, OnError);
        }

        private void UpdateCharacterStatistics(string resultCharacterId)
        {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = resultCharacterId,
                CharacterStatistics = new Dictionary<string, int>
                {
                    {"Level", 1},
                    {"Gold", 10},
                    {"Damage", 2},
                    {"Health", 100},
                    {"XP", 0},
                }
            }, result =>
            {
                Debug.Log("Complete");
                CloseCreateNewCharacter();
                GetCharacters();
            }, OnError);
        }

        private void OnNameChange(string changedName)
        {
            _characterName = changedName;
        }

        private void OpenCreateNewCharacter()
        {
            _newCharacterCreatePanel.SetActive(true);
        }
        
        private void CloseCreateNewCharacter()
        {
            _newCharacterCreatePanel.SetActive(false);
        }

        private void GetCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(), result =>
            {
                Debug.Log($"Character count: {result.Characters.Count}");
                ShowCharactersInSlor(result.Characters);
            }, OnError);
        }

        private void ShowCharactersInSlor(List<CharacterResult> resultCharacters)
        {
            if(resultCharacters.Count == 0)
            {
                foreach (var slot in _slots)
                {
                    slot.ShowEmptySlot();
                }
            }
            else if (resultCharacters.Count > 0 && resultCharacters.Count <= _slots.Count)
            {
                PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = resultCharacters.First().CharacterId,
                }, result =>
                {
                    var level = result.CharacterStatistics["Level"].ToString();
                    var gold = result.CharacterStatistics["Gold"].ToString();
                    var damage = result.CharacterStatistics["Damage"].ToString();
                    var health = result.CharacterStatistics["Health"].ToString();
                    var xp = result.CharacterStatistics["XP"].ToString();
                    
                    _slots.First().ShowInfoCharacterSlot(resultCharacters.First().CharacterName, level, gold, damage, health, xp);
                }, OnError);
            }
            else
            {
                Debug.LogError("Add slots for characters");
            }
        }

        private void OnGetRandomResultTables(PlayFab.ServerModels.GetRandomResultTablesResult result)
        {
            
        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            Debug.Log("OnGetCatalogSuccess");

            ShowItems(result.Catalog);
        }

        private void ShowItems(List<CatalogItem> resultCatalog)
        {
            foreach (var item in resultCatalog)
            {
                Debug.Log($"{item.ItemId}");
            }
        }

        private void OnGetAccountSuccess(GetAccountInfoResult result)
        {
            _titleLabel.text = $" Welcome back, {result.AccountInfo.Username} \n Your ID: {result.AccountInfo.PlayFabId}";
            Loading.Instance.LoadingState(false);
        }
        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
            Loading.Instance.LoadingState(false);
        }

        private void OnError(PlayFabError error)
        {
            var ErroeMessage = error.GenerateErrorReport();
            Debug.LogError(ErroeMessage);
        }
    }
}