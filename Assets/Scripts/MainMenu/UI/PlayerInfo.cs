using UnityEngine;

namespace MainMenu.UI
{
    /// <summary>
    /// Class in charge of the player info in the Joined Room
    /// </summary>
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _playerName;
        public string Name { get; set; }

        /// <summary>
        /// Sets the player name in the UI  
        /// </summary>
        /// <param name="playerName"></param>
        public void SetPlayerInfo(string playerName)
        {
            _playerName.text = playerName;
            Name = playerName;
        }
    }
}