using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace NaughtyHamster
{

    public class GameManager : MonoBehaviourPun
    {
        private static GameManager instance;

        [HideInInspector]
        public Player localPlayer;

        public GameMode gameMode = GameMode.Casual;
        public UIGame ui_game;
        public UIPhase0 ui_phase0;

        public Team[] teams;
        public int maxScore = 30;

        void Awake()
        {
            instance = this;
        }

        public static GameManager GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// Returns the next team index a player should be assigned to.
        /// </summary>
        public int GetTeamFill()
        {
            //init variables
            int[] size = PhotonNetwork.CurrentRoom.GetSize();
            int teamNo = -1;

            //int min = size[0];
            int max = 1;

            //loop over teams to find the lowest fill
            for (int i = 0; i < teams.Length; i++)
            {
                //if fill is lower than the previous value
                //store new fill and team for next iteration
                if (size[i] < max)
                {
                    //min = size[i];
                    teamNo = i;
                    PlayerPrefs.SetInt(PrefsKeys.teamIndex, teamNo);
                    return teamNo;
                }
            }

            //return index of lowest team
            return teamNo;
        }


        /// <summary>
        /// Returns a random spawn position within the team's spawn area.
        /// </summary>
        public Vector3 GetSpawnPosition(int teamIndex)
        {
            //init variables
            Vector3 pos = teams[teamIndex].spawn.position;
            BoxCollider col = teams[teamIndex].spawn.GetComponent<BoxCollider>();

            if(col != null)
            {
                //find a position within the box collider range, first set fixed y position
                //the counter determines how often we are calculating a new position if out of range
                pos.y = col.transform.position.y;
                int counter = 10;
                
                //try to get random position within collider bounds
                //if it's not within bounds, do another iteration
                do
                {
                    pos.x = UnityEngine.Random.Range(col.bounds.min.x, col.bounds.max.x);
                    pos.z = UnityEngine.Random.Range(col.bounds.min.z, col.bounds.max.z);
                    counter--;
                }
                while(!col.bounds.Contains(pos) && counter > 0);
            }
            
            return pos;
        }

        public void RunGameFlow()
        {
            ui_phase0.BeginGame();

        }

        /// <summary>
        /// Adds points to the target team depending on matching game mode and score type.
        /// This allows us for granting different amount of points on different score actions.
        /// </summary>
        public void AddScore(ScoreType scoreType, int teamIndex)
        {
            //distinguish between game mode
            switch(gameMode)
            {
                //in TDM, we only grant points for killing
                case GameMode.Casual:
                    switch(scoreType)
                    {
                        case ScoreType.Kill:
                            PhotonNetwork.CurrentRoom.UpdateCalorieRecord(teamIndex, 1);
                            break;
                    }
                break;

            }
        }
        

        /// <summary>
        /// Returns whether a team reached the maximum game score.
        /// </summary>
        public bool IsGameOver()
        {
            //init variables
            bool isOver = false;
            int[] score = PhotonNetwork.CurrentRoom.GetCalorieRecord();

            //loop over teams to find the highest score
            for(int i = 0; i < teams.Length; i++)
            {
                //score is greater or equal to max score,
                //which means the game is finished
                if(score[i] >= maxScore)
                {
                    isOver = true;
                    break;
                }
            }
            
            //return the result
            return isOver;
        }

        /// <summary>
        /// Only for this player: sets game over text stating the winning team.
        /// Disables player movement so no updates are sent through the network.
        /// </summary>
        public void DisplayGameOver(int teamIndex, string winnerName)
        {
            //PhotonNetwork.isMessageQueueRunning = false;
            localPlayer.enabled = false;
            localPlayer.camFollow.HideMask(true);
            //ui_game.SetGameOverText(teams[teamIndex]);

            //starts coroutine for displaying the game over window
            //StopCoroutine(SpawnRoutine());
            StartCoroutine(ShowGameOverWindow(winnerName));
        }


        //displays game over window after short delay
        IEnumerator ShowGameOverWindow(string winnerName)
        {
            //give the user a chance to read which team won the game
            //before enabling the game over screen
            yield return new WaitForSeconds(3);

            //show game over window (still connected at that point)
            ui_game.ShowGameOver(winnerName);
        }


        //clean up callbacks on scene switches
        void OnDestroy()
        {

        }
    }


    /// <summary>
    /// Defines properties of a team.
    /// </summary>
    [System.Serializable]
    public class Team
    {
        /// <summary>
        /// The name of the team shown on game over.
        /// </summary>
        public string name;

        /// <summary>
        /// The color of a team for UI and player prefabs.
        /// </summary>   
        public Material material;

        /// <summary>
        /// The spawn point of a team in the scene. In case it has a BoxCollider
        /// component attached, a point within the collider bounds will be used.
        /// </summary>
        public Transform spawn;

        /// <summary>
        /// The spawn point of food of this team
        /// </summary>
        public GameObject[] spawnPoints;

        /// <summary>
        /// The spawn point for choosing suspects of this team
        /// </summary>
        public GameObject[] suspectPoints;

        /// <summary>
        /// The spawn point for revealing suspects
        /// </summary>
        public GameObject[] revealPoints;

        /// <summary>
        /// The spawn point for other teams during investigation
        /// </summary>
        public GameObject otherTeamPoints;
        /// <summary>
        /// The spawn point for guard during investigation
        /// </summary>
        public GameObject[] guardPoint;

    }

    /// <summary>
    /// Defines the types that could grant points to players or teams.
    /// Used in the AddScore() method for filtering.
    /// </summary>
    public enum ScoreType
    {
        Kill,
        Capture
    }


    /// <summary>
    /// Available game modes selected per scene.
    /// Used in the AddScore() method for filtering.
    /// </summary>
    public enum GameMode
    {
        Casual

    }
}