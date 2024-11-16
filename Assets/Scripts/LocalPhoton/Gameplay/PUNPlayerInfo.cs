using UnityEngine;

namespace LocalPhoton.Gameplay
{
    public class PUNPlayerInfo
    {
        public string name; // Name of the player

        public int actor; // Actor number that comes from Photon
        public int kills; // Number of kills the player has
        //public int deaths; // Number of deaths the player has
        // TODO: More stats

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actor"></param>
        /// <param name="kills"></param>
        ///// <param name="deaths"></param>
        public PUNPlayerInfo(string name, int actor, int kills/*, int deaths*/)
        {
            this.name = name;
            this.actor = actor;
            this.kills = kills;
            //this.deaths = deaths;
        }
    }
}