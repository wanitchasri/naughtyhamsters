using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NaughtyHamster
{
    public static class PlayerExtensions
    {
        //keys for saving and accessing values in custom properties Hashtable
        public const string team = "team";
        public const string calorie = "calorie";
        public const string role = "role"; //0=seeker, 1=guard
        public const string cheek = "cheek";

        /* ----------------------------------- */
        /* ONLINE MODE */
        /* ----------------------------------- */

        /// <summary>
        /// Returns the networked player nick name.
        /// Offline: bot name. Online: PhotonPlayer name.
        /// </summary>
        public static string GetName(this PhotonView player)
        {
            return player.Owner.NickName;
        }

        /// <summary>
        /// Online: returns the networked team number of the player out of properties.
        /// </summary>
        public static int GetTeam(this Photon.Realtime.Player player)
        {
            return System.Convert.ToInt32(player.CustomProperties[team]);
        }

        /// <summary>
        /// Online: synchronizes the team number of the player for all players via properties.
        /// </summary>
        public static void SetTeam(this Photon.Realtime.Player player, int teamIndex)
        {
            player.SetCustomProperties(new Hashtable() { { team, (byte)teamIndex } });
        }

        /// <summary>
        /// Online: returns the networked health value of the player out of properties.
        /// </summary>
        public static int GetCalorie(this Photon.Realtime.Player player)
        {
            return System.Convert.ToInt32(player.CustomProperties[calorie]);
        }

        /// <summary>
        /// Online: synchronizes the health value of the player for all players via properties.
        /// </summary>
        public static void SetCalorie(this Photon.Realtime.Player player, int value)
        {
            player.SetCustomProperties(new Hashtable() { { calorie, (byte)value } });
        }

        /// <summary>
        /// Online: returns the networked role value of the player out of properties.
        /// </summary>
        public static int GetRole(this Photon.Realtime.Player player)
        {
            return System.Convert.ToInt32(player.CustomProperties[role]);
        }

        /// <summary>
        /// Online: synchronizes the role value of the player for all players via properties.
        /// </summary>
        public static void SetRole(this Photon.Realtime.Player player, int value)
        {
            player.SetCustomProperties(new Hashtable() { { role, (byte)value } });
        }

        /// <summary>
        /// Online: Clears all networked variables of the player via properties in one instruction.
        /// </summary>
        public static void Clear(this Photon.Realtime.Player player)
        {
            player.SetCustomProperties(new Hashtable() { { PlayerExtensions.calorie, (byte)0 } });
        }

        public static string GetCheek(this Photon.Realtime.Player player)
        {
            return (string)player.CustomProperties[cheek];
        }

        public static void SetCheek(this Photon.Realtime.Player player, string value)
        {
            player.SetCustomProperties(new Hashtable() { { cheek, value } });
        }

        /* ----------------------------------- */
        /* OFFLINE MODE */
        /* ----------------------------------- */

        /// <summary>
        /// Offline: returns the team number of a bot stored in PlayerBot.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static int GetTeam(this PhotonView player)
        {
            return player.Owner.GetTeam();
        }

        /// <summary>
        /// Offline: synchronizes the team number of a PlayerBot locally.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static void SetTeam(this PhotonView player, int teamIndex)
        {
            player.Owner.SetTeam(teamIndex);
        }

        /// <summary>
        /// Offline: returns the health value of a bot stored in PlayerBot.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static int GetCalorie(this PhotonView player)
        {
            return player.Owner.GetCalorie();
        }

        /// <summary>
        /// Offline: synchronizes the health value of a PlayerBot locally.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static void SetCalorie(this PhotonView player, int value)
        {
            player.Owner.SetCalorie(value);
        }

        /// <summary>
        /// Offline: synchronizes the health value of a PlayerBot locally.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static void SetRole(this PhotonView player, int value)
        {
            player.Owner.SetRole(value);
        }

        /// <summary>
        /// Offline: clears all properties of a PlayerBot locally.
        /// Fallback to online mode for the master or in case offline mode was turned off.
        /// </summary>
        public static void Clear(this PhotonView player)
        {
            player.Owner.Clear();
        }

        public static string GetCheek(this PhotonView player)
        {
            return player.Owner.GetCheek();
        }

        public static void SetCheek(this PhotonView player, string value)
        {
            player.Owner.SetCheek(value);
        }

    }
}
