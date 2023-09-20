using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class RoomCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        
        private RoomInfo _roomInfo;

        public RoomInfo Room
        {
            get => _roomInfo;
            set
            {
                _nameText.text = value.Name;
                _roomInfo = value;
            }
        }
    }
}