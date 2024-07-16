using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NaughtyHamster
{

	public class NetworkManagerCustom : MonoBehaviourPunCallbacks
    {
        private static NetworkManagerCustom instance;

        public int offlineSceneIndex = 0;
        public int onlineSceneIndex = 1;

        public int maxPlayers = 4;
        public GameObject[] playerPrefabs;
        public static event Action connectionFailedEvent;


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            PhotonView view = gameObject.AddComponent<PhotonView>();
            view.ViewID = 999;
        }

        public static NetworkManagerCustom GetInstance()
        {
            return instance;
        }

        public static void StartMatch(NetworkMode mode)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = PlayerPrefs.GetString(PrefsKeys.playerName);

            switch (mode)
            {
                case NetworkMode.Online:
                    PhotonNetwork.ConnectUsingSettings();
                    break;

            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Player Disconnected");
            if (connectionFailedEvent != null)
                connectionFailedEvent();

            //do not switch scenes automatically when the game over screen is being shown already
            if (GameManager.GetInstance() != null && GameManager.GetInstance().ui_game.gameOverMenu.activeInHierarchy)
                return;

            ////switch from the online to the offline scene after connection is closed
            //if (SceneManager.GetActiveScene().buildIndex != offlineSceneIndex)
            //    SceneManager.LoadScene(offlineSceneIndex);
        }

        public override void OnConnectedToMaster()
        {
            // PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = PlayerPrefs.GetString(PrefsKeys.playerName);

            Hashtable expectedCustomRoomProperties = new Hashtable() { { "mode", (byte)PlayerPrefs.GetInt(PrefsKeys.gameMode) } };

            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, (byte)this.maxPlayers);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Photon did not find any matches on the Master Client we are connected to. Creating our own room...");

            RoomOptions roomOptions = new RoomOptions();

            roomOptions.CustomRoomPropertiesForLobby = new string[] { "mode" };
            roomOptions.CustomRoomProperties = new Hashtable() { { "mode", (byte)PlayerPrefs.GetInt(PrefsKeys.gameMode) } };

            roomOptions.MaxPlayers = (byte)this.maxPlayers;
            roomOptions.CleanupCacheOnLeave = false;
            roomOptions.BroadcastPropsChangeToAll = false;
            PhotonNetwork.CreateRoom(null, roomOptions, null);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            if (connectionFailedEvent != null)
                connectionFailedEvent();
        }

        public override void OnCreatedRoom()
        {
            //the initial team size of the game for the server creating a new room.
            //this cannot be set via GameManager because it does not exist at that point
            short totalTeams;
            //get the selected game mode out of PlayerPrefs
            GameMode activeGameMode = ((GameMode)PlayerPrefs.GetInt(PrefsKeys.gameMode));

            //set the initial room array size initialization based on game mode
            switch(activeGameMode)
            {
                //case GameMode.CTF:
                //    initialArrayLength = 2;
                //    break;
                default:
                    totalTeams = 4; // total of 4 teams
                    break;
            }

            //we created a room so we have to set the initial room properties for this room,
            //such as populating the team fill and score arrays
            Hashtable roomProps = new Hashtable();
            roomProps.Add(RoomExtensions.status, "");
            roomProps.Add(RoomExtensions.size, new int[totalTeams]);
            roomProps.Add(RoomExtensions.totalPlayer, 0);
            roomProps.Add(RoomExtensions.round, 0);
            roomProps.Add(RoomExtensions.calorieRecord, new int[totalTeams]);
            roomProps.Add(RoomExtensions.roleRecord, new int[totalTeams]);
            roomProps.Add(RoomExtensions.suspectRecord, new int[totalTeams]);
            roomProps.Add(RoomExtensions.names, "");
            roomProps.Add(RoomExtensions.updateCounter, 0);

            string[] cheekArr = new string[totalTeams];
            for (int i = 0; i < cheekArr.Length; i++) { cheekArr[i] = ""; }
            roomProps.Add(RoomExtensions.cheek, cheekArr);

            //roomProps.Add(RoomExtensions.isReady, new bool[totalTeams]);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            //PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { RoomExtensions.cheek, new string[totalTeams]  } });

            //load the online scene randomly out of all available scenes for the selected game mode
            //check for a naming convention here, if a scene starts with the game mode abbreviation
            List<int> matchingScenes = new List<int>();
            for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string[] scenePath = SceneUtility.GetScenePathByBuildIndex(i).Split('/');
                if (scenePath[scenePath.Length - 1].StartsWith(activeGameMode.ToString()))
                {
                    matchingScenes.Add(i);
                }
            }
			
			if(matchingScenes.Count == 0)
            {
                Debug.LogWarning("No Scene for selected Game Mode found in Build Settings!");
                return;
            }

            //get random scene out of available scenes and assign it as the online scene
            onlineSceneIndex = matchingScenes[UnityEngine.Random.Range(0, matchingScenes.Count)];
            PhotonNetwork.LoadLevel(onlineSceneIndex);
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            //if (GameManager.GetInstance() != null && GameManager.GetInstance().IsGameOver())
            //if (PhotonNetwork.CurrentRoom.GetStatus() == "started")
            //{
            //    Debug.Log("Room is finished. Disconnecting..");
            //    PhotonNetwork.Disconnect();
            //    return;
            //}

            if (!PhotonNetwork.IsMasterClient)
                return;

            //add ourselves to the game.
            //called on MasterClient because other clients will trigger the OnPhotonPlayerConnected callback directly
            StartCoroutine(WaitForSceneChange());
        }


        //this wait routine is needed on offline mode for waiting on completed scene change,
        //because in offline mode Photon does not pause network messages. But it doesn't hurt
        //leaving this in for all other network modes too
        IEnumerator WaitForSceneChange()
        {
            while (SceneManager.GetActiveScene().buildIndex != onlineSceneIndex)
            {
                yield return null;
            }

            OnPlayerEnteredRoom(PhotonNetwork.LocalPlayer);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            //get the next team index which the player should belong to
            //assign it to the player and update player properties
            int teamIndex = GameManager.GetInstance().GetTeamFill();

            if (teamIndex > -1)
            {
                PhotonNetwork.CurrentRoom.AddSize(teamIndex, +1);
                PhotonNetwork.CurrentRoom.AddTotalPlayer(1);
                player.SetTeam(teamIndex);

                PhotonNetwork.CurrentRoom.UpdateCalorieRecord(teamIndex, +50);
                PhotonNetwork.CurrentRoom.UpdateRoleRecord(teamIndex, 0);
                PhotonNetwork.CurrentRoom.UpdateSuspectRecord(teamIndex, 0);
                PhotonNetwork.CurrentRoom.SetCheekRecords(teamIndex, "");

                //Debug.Log("Adding Players to Room");

                //also player properties are not cleared when disconnecting and connecting
                //automatically, so we have to set all existing properties to null
                //these default values will get overriden by correct data soon
                player.Clear();

                this.photonView.RPC("AddPlayer", player);

                if (PhotonNetwork.CurrentRoom.GetTotalPlayer() >= 1)
                {
                    //Debug.Log("A player just got in.");
                    PhotonNetwork.CurrentRoom.SetNames(player.NickName + ",");
                    photonView.RPC("BeginGame", RpcTarget.AllViaServer);
                }
            } 
        }


		[PunRPC]
		void AddPlayer()
		{
			int prefabId = int.Parse(Encryptor.Decrypt(PlayerPrefs.GetString(PrefsKeys.activeModel)));
            
            //get the spawn position where our player prefab should be instantiated at, depending on the team assigned
            //if we cannot get a position, spawn it in the center of that team area - otherwise use the calculated position
			Transform startPos = GameManager.GetInstance().teams[PhotonNetwork.LocalPlayer.GetTeam()].spawn;
			if (startPos != null) PhotonNetwork.Instantiate(playerPrefabs[prefabId].name, startPos.position, startPos.rotation, 0);
			else PhotonNetwork.Instantiate(playerPrefabs[prefabId].name, Vector3.zero, Quaternion.identity, 0);
		}

        [PunRPC]
        void BeginGame()
        {
            GameManager.GetInstance().RunGameFlow();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
				return;

            GameObject targetPlayer = GetPlayerGameObject(player);
            PhotonNetwork.DestroyPlayerObjects(player);

            //decrease the team fill for the team of the leaving player and update room properties
            PhotonNetwork.CurrentRoom.AddSize(player.GetTeam(), -1);
            PhotonNetwork.CurrentRoom.AddSize(player.GetTeam(), -1);
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
        }

        /// <summary>
        /// Finds the remotely controlled Player game object of a specific player,
        /// by iterating over all Player components and searching for the matching creator.
        /// </summary>
        public GameObject GetPlayerGameObject(Photon.Realtime.Player player)
        {
            GameObject[] rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
            List<Player> playerList = new List<Player>();

            //get all Player components from root objects
            for (int i = 0; i < rootObjs.Length; i++)
            {
                Player p = rootObjs[i].GetComponentInChildren<Player>(true);
                if (p != null) playerList.Add(p);
            }

            //find the game object where the creator matches this specific player ID
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].photonView.CreatorActorNr == player.ActorNumber)
                {
                    return playerList[i].gameObject;
                }
            }

            return null;
        }

        public static string GetNameByIndex(int id)
        {
            string name = PhotonNetwork.CurrentRoom.GetNames();
            string[] names = name.Split(",");

            return names[id];
        }
    }

    public enum NetworkMode
    {
        Online = 0,
        LAN = 1,
        Offline = 2
    }

    public static class RoomExtensions
    {
        public const string status = "status";
        public const string size = "size";
        public const string names = "names";
        public const string totalPlayer = "totalPlayer";
        public const string round = "round";

        public const string calorieRecord = "calorieRecord";
        public const string roleRecord = "roleRecord";
        public const string suspectRecord = "suspectRecord";
        public const string cheek = "cheek";
        public const string updateCounter = "updateCounter";

        public static string GetStatus(this Room room)
        {
            return (string)room.CustomProperties[status];
        }

        public static string SetStatus(this Room room, string value)
        {
            // string currentStatus = room.GetStatus();
            string currentStatus = value;

            room.SetCustomProperties(new Hashtable() { { status, currentStatus } });
            return currentStatus;
        }

        public static int[] GetSize(this Room room)
        {
            return (int[])room.CustomProperties[size];
        }

        /// <summary>
        /// Increases the team fill when a new player joined the game.
        /// Decreases when a player disconnects
        /// </summary>
        public static int[] AddSize(this Room room, int teamIndex, int value)
        {
            int[] sizes = room.GetSize();
            sizes[teamIndex] += value;

            room.SetCustomProperties(new Hashtable() { { size, sizes } });
            return sizes;
        }

        public static string GetNames(this Room room)
        {
            return (string)room.CustomProperties[names];
        }

        public static string SetNames(this Room room, string value)
        {
            string oldNames = room.GetNames();

            room.SetCustomProperties(new Hashtable() { { names, oldNames += value } });
            return oldNames;
        }

        public static int GetTotalPlayer(this Room room)
        {
            return (int)room.CustomProperties[totalPlayer];
        }

        public static int AddTotalPlayer(this Room room, int value)
        {
            int total = room.GetTotalPlayer();
            total += value;

            room.SetCustomProperties(new Hashtable() { { totalPlayer, total } });
            return total;
        }

        public static int GetRound(this Room room)
        {
            return (int)room.CustomProperties[round];
        }

        public static int AddRound(this Room room, int value)
        {
            int currentRound = room.GetRound();
            currentRound += value;

            room.SetCustomProperties(new Hashtable() { { round, currentRound } });
            return currentRound;
        }

        public static int[] GetCalorieRecord(this Room room)
        {
            return (int[])room.CustomProperties[calorieRecord];
        }

        public static int[] UpdateCalorieRecord(this Room room, int teamIndex, int value)
        {
            int[] scores = room.GetCalorieRecord();
            scores[teamIndex] += value;
            
            room.SetCustomProperties(new Hashtable() {{calorieRecord, scores}});
            return scores;
        }

        public static int[] GetRoleRecord(this Room room)
        {
            return (int[])room.CustomProperties[roleRecord];
        }

        public static int[] UpdateRoleRecord(this Room room, int teamIndex, int value)
        {
            int[] roles = room.GetRoleRecord();
            roles[teamIndex] += value;

            room.SetCustomProperties(new Hashtable() { { roleRecord, roles } });
            return roles;
        }

        public static int[] GetSuspectRecord(this Room room)
        {
            return (int[])room.CustomProperties[suspectRecord];
        }

        public static int[] UpdateSuspectRecord(this Room room, int teamIndex, int value)
        {
            int[] suspects = room.GetSuspectRecord();
            suspects[teamIndex] = value;

            room.SetCustomProperties(new Hashtable() { { suspectRecord, suspects } });
            return suspects;
        }

        public static string[] GetCheekRecords(this Room room)
        {
            return (string[])room.CustomProperties[cheek];
        }

        public static string[] SetCheekRecords(this Room room, int teamIndex, string value)
        {
           string[] currentCheek = room.GetCheekRecords();
           currentCheek[teamIndex] = value;

           room.SetCustomProperties(new Hashtable() { { cheek, currentCheek } });
           return currentCheek;
        }

        public static string[] ClearCheek(this Room room)
        {
            string[] currentCheek = room.GetCheekRecords();
            for (int i=0; i<currentCheek.Length; i++)
            {
                currentCheek[i] = "";
            }

            room.SetCustomProperties(new Hashtable() { { cheek, currentCheek } });
            return currentCheek;
        }

        /// <summary>
        /// Update suspect record of players; 0=seeker 1=guard
        /// </summary>
        public static int GetUpdateCounter(this Room room)
        {
            return (int)room.CustomProperties[updateCounter];
        }

        public static int AddToUpdateCounter(this Room room, int value)
        {
            int newCounter = room.GetUpdateCounter();
            newCounter += value;

            room.SetCustomProperties(new Hashtable() { { updateCounter, newCounter } });
            return newCounter;
        }
    }
}