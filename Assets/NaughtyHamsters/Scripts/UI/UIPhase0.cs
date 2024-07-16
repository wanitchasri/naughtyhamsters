using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

namespace NaughtyHamster
{

    public class UIPhase0 : MonoBehaviourPunCallbacks
    {
        [HideInInspector] Photon.Realtime.Player player;
        [HideInInspector] public string playerName;
        [HideInInspector] public int playerIndex;
        [HideInInspector] public int round;
        [HideInInspector] public int updateCount;

        [HideInInspector] public string step;

        public GameObject P0_Welcome;
        [HideInInspector] public int total;
        public TMP_Text total_text;
        // public Button ready_button;

        public GameObject P0_Load;
        [HideInInspector] public float timer;
        public TMP_Text timer_text;

        public GameObject P0_Round;

        public GameObject P0_InitCalorie;
        [HideInInspector] public int[] calorieRecord;
        [HideInInspector] public int calorie;
        public TMP_Text calorie_text;

        public GameObject P0_SwitchRole;
        [HideInInspector] public int[] roleRecord;
        [HideInInspector] public string role;
        public TMP_Text name_text;
        public TMP_Text role_text;
        public TMP_Text round_text;

        public UIPhase1 ui_phase1;

        void Start()
        {
            P0_Welcome.gameObject.SetActive(true);   
        }

        /* UPDATES */
        private void Update()
        {
            if (step == "BeginGame")
            {
                if (PhotonNetwork.CurrentRoom.GetStatus() == "assignedRole")
                {
                    Round();
                }
            }
            else if (step == "Round")
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    SetUpGame();
                }
            }
            else if (step == "SetUpGame")
            {
                timer_text.gameObject.SetActive(true);

                timer -= Time.deltaTime;
                timer_text.text = timer.ToString("f0");

                if (timer <= 0)
                {
                    InitializeCalorie();
                }
            }
            else if (step == "InitCalorie")
            {
                calorie = calorieRecord[playerIndex];
                calorie_text.text = calorie.ToString();

                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    SwitchRole();
                }
            }
            else if (step == "SwitchRole")
            {
                round = PhotonNetwork.CurrentRoom.GetRound();
                roleRecord = PhotonNetwork.CurrentRoom.GetRoleRecord();
                //Debug.Log("round" + round + " record"+ roleRecord[playerIndex] + " of playerIndex" + playerIndex);
                if (roleRecord[playerIndex] == round)
                {
                    role = "Guard";
                    player.SetRole(1);
                    PlayerPrefs.SetInt(PrefsKeys.playerRole, 1);
                }
                else 
                {
                    role = "Seeker";
                    player.SetRole(0);
                    PlayerPrefs.SetInt(PrefsKeys.playerRole, 0);
                }

                name_text.text = playerName.ToString();
                role_text.text = role.ToString();

                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    ui_phase1.EnterPhase1();
                    step = "Stop";
                }
            }
        }

        /* STEPS */
        public void BeginGame()
        {
            player = PhotonNetwork.LocalPlayer;
            playerName = player.NickName;
            // playerName = PlayerPrefs.GetString(PrefsKeys.playerName);
            playerIndex = player.GetTeam();

            calorieRecord = PhotonNetwork.CurrentRoom.GetCalorieRecord();
            roleRecord = PhotonNetwork.CurrentRoom.GetRoleRecord();

            WaitForPlayers();
            total = PhotonNetwork.CurrentRoom.GetTotalPlayer();
            total_text.text = total.ToString() + " : 4";
        }

        public void WaitForPlayers()
        {
            if (round == 0)
            {
                if (PhotonNetwork.CurrentRoom.GetTotalPlayer() == 4)
                {
                    if (player.IsMasterClient) { AssignRole(); }
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    step = "BeginGame";
                }
            }
            else if (round > 0)
            {
                //if (player.IsMasterClient) {
                //    Debug.Log("updateCounter"+PhotonNetwork.CurrentRoom.GetUpdateCounter());
                //    if (PhotonNetwork.CurrentRoom.GetUpdateCounter() == 0) { Debug.Log("Assigning Role"); AssignRole(); }
                //}
                if (player.IsMasterClient) { AssignRole(); }
                step = "BeginGame";
            }
        }

        public void AssignRole()
        { 
            Debug.Log("**Assigning Roles**");
            PhotonNetwork.CurrentRoom.AddRound(+1);
            round = PhotonNetwork.CurrentRoom.GetRound();
            Debug.Log("*ROUND " + round + "*");

            int[] roleRecord = PhotonNetwork.CurrentRoom.GetRoleRecord();
            for (int i = 0; i < roleRecord.Length; i++)
            {
                if (roleRecord[i] == 0)
                {
                    PhotonNetwork.CurrentRoom.UpdateRoleRecord(i, +round);
                    break;
                }
            }
            //PhotonNetwork.CurrentRoom.SetUpdateCounter(1);
            string roles = ""; foreach (var x in roleRecord) { roles += ", " + x; }
            Debug.Log("Roles" + roles);
            PhotonNetwork.CurrentRoom.SetStatus("assignedRole");

            if (round > 1) { PhotonNetwork.CurrentRoom.AddToUpdateCounter(-3); }
            Debug.Log("counter = " + PhotonNetwork.CurrentRoom.GetUpdateCounter());
        }


        public void Round()
        {
            step = "Round";
            Debug.Log("Step: " + step);

            P0_Welcome.gameObject.SetActive(false);
            P0_Round.SetActive(true);

            round = PhotonNetwork.CurrentRoom.GetRound();
            round_text.text = round.ToString();

            timer = 3f;
        }
        
        public void SetUpGame()
        {
            step = "SetUpGame";
            Debug.Log("Step: " + step);
            P0_Round.gameObject.SetActive(false);
            P0_Load.gameObject.SetActive(true);
            timer = 5f;
        }

        public void InitializeCalorie()
        {
            step = "InitCalorie";
            Debug.Log("Step: " + step);
            P0_Load.gameObject.SetActive(false);
            P0_InitCalorie.gameObject.SetActive(true);
            timer = 5f;
        }

        public void SwitchRole()
        {
            Debug.Log("Names: " + PhotonNetwork.CurrentRoom.GetNames());
            step = "SwitchRole";
            Debug.Log("Step: " + step);
            P0_InitCalorie.gameObject.SetActive(false);
            P0_SwitchRole.gameObject.SetActive(true);
            timer = 5f;
        }

    }
}
