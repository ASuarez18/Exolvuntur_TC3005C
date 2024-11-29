namespace LocalPhoton.Gameplay
{
    /// <summary>
    /// Class that holds event codes for PUN
    /// </summary>
    public class PUNEventCodes
    {
        public enum EventCodes : byte
        {
            NewPlayer,
            ListPlayers,
            UpdateStat,
            UpdateGameState
        }

        public enum GameStates : byte
        {
            Waiting,
            Starting,
            Playing,
            Ending
        }

        public enum PlayerStats : byte
        {
            Kills,
            TotalObjects
            // More stats
        }
    }
}