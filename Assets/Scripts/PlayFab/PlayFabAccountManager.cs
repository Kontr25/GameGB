using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UI;
using UnityEngine;

namespace PlayFab
{
    public class PlayFabAccountManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleLabel;
        private void Start()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnFailure);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(),OnGetCatalogSuccess, OnFailure);
            PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest(), OnGetRandomResultTables, OnFailure);
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
    }
}