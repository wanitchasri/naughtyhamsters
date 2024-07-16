using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

namespace NaughtyHamster
{
    /// <summary>
    /// UI script for all elements, team events and user interactions in the game scene.
    /// </summary>
    public class UIGame : MonoBehaviourPunCallbacks
    {
        public Slider[] teamSize;
        public Text[] teamScore;
        public TMP_Text[] teamName;

        public Text spawnDelayText;
        public Text gameOverText;

        public GameObject gameOverMenu;
        public TMP_Text winnerNameText;

        void Start()
        {
            //play background music
            AudioManager.PlayMusic(1);
        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
		{
            if (propertiesThatChanged.ContainsKey("names"))
            {
                OnTeamNameChanged(PhotonNetwork.CurrentRoom.GetNames());
            }

            OnTeamSizeChanged(PhotonNetwork.CurrentRoom.GetSize());
			OnTeamScoreChanged(PhotonNetwork.CurrentRoom.GetCalorieRecord());
		}

        public void OnTeamSizeChanged(int[] size)
        {
            //loop over sliders values and assign it
			for(int i = 0; i < size.Length; i++)
            	teamSize[i].value = size[i];
        }

        public void OnTeamScoreChanged(int[] score)
        {
            //loop over texts
			for(int i = 0; i < score.Length; i++)
            {
                //detect if the score has been increased, then add fancy animation
                if(score[i] > int.Parse(teamScore[i].text))
                    teamScore[i].GetComponent<Animator>().Play("Animation");

                //assign score value to text
                teamScore[i].text = score[i].ToString();
            }
        }

        public void OnTeamNameChanged(string names)
        {
            string[] n = names.Split(",");
            for (int i = 0; i < n.Length; i++)
            {
                if (i < teamName.Length)
                {
                    teamName[i].text = n[i];
                }
            }
        }

        /// <summary>
        /// Set respawn delay value displayed to the absolute time value received.
        /// The remaining time value is calculated in a coroutine by GameManager.
        /// </summary>
        public void SetSpawnDelay(float time)
        {                
            spawnDelayText.text = Mathf.Ceil(time) + "";
        }

        public void ShowGameOver(string winnerName)
        {
            winnerNameText.text = winnerName; 
            gameOverText.gameObject.SetActive(false);
            gameOverMenu.SetActive(true);

        }

        /// <summary>
        /// Returns to the starting scene and immediately requests another game session.
        /// In the starting scene we have the loading screen and disconnect handling set up already,
        /// so this saves us additional work of doing the same logic twice in the game scene. The
        /// restart request is implemented in another gameobject that lives throughout scene changes.
        /// </summary>
        public void Restart()
        {
            GameObject gObj = new GameObject("RestartNow");
            gObj.AddComponent<UIRestartButton>();
            DontDestroyOnLoad(gObj);
            
            Disconnect();
        }

        public void Disconnect()
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Disconnect();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(NetworkManagerCustom.GetInstance().offlineSceneIndex);
        }
    }
}
