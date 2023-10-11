using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFab
{
    public class SlotCharacterWidget : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _emptySlot;
        [SerializeField] private GameObject _infoCharacterSlot;
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private TMP_Text _goldLabel;
        [SerializeField] private TMP_Text _damageLabel;
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private TMP_Text _xpLabel;

        public Button SlotButton => _button;

        public void ShowInfoCharacterSlot(string name, string level, string gold,string damage, string health, string xp)
        {
            _nameLabel.text = name;
            _levelLabel.text = $"Level: {level}";
            _goldLabel.text = $"Gold: {gold}";
            _damageLabel.text = $"Damage: {damage}";
            _healthLabel.text = $"Health: {health}";
            _xpLabel.text = $"XP: {xp}";
            
            _infoCharacterSlot.SetActive(true);
            _emptySlot.SetActive(false);
        }

        public void ShowEmptySlot()
        {
            _infoCharacterSlot.SetActive(false);
            _emptySlot.SetActive(true);
        }
    }
}