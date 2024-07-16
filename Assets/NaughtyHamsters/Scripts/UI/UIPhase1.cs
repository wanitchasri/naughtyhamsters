using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;

namespace NaughtyHamster
{

    public class UIPhase1 : MonoBehaviourPunCallbacks
    {
        public UIPhase1 instance;
        [HideInInspector] Photon.Realtime.Player player;
        [HideInInspector] public string playerName;
        [HideInInspector] public int playerIndex;
        [HideInInspector] public string playerRole;
        [HideInInspector] public int[] calorieRecord;
        [HideInInspector] public int[] roleRecord;

        [HideInInspector] public string step;
        [HideInInspector] public float timer;

        public GameObject P0_SwitchRole;
        public GameObject P1_Title;
        public GameObject P1_FoundFood;
        public GameObject P1_ChooseNextMove;
        public GameObject P1_Finalize;

        public TMP_Text timer_foundFood;
        public TMP_Text timer_chooseNextMove;

        public Button keep_button;
        public Button change_button;

        public FoodManager SpawnManager;
        [HideInInspector] public GameObject[] spawnPoints;
        [HideInInspector] public GameObject[] spawnedFood = new GameObject[6];

        public UIPhase2 ui_phase2;

        public void Awake()
        {
            instance = this;    
        }

        public UIPhase1 GetInstance()
        {
            return instance;
        }

        private void Update()
        {
            if (step == "EnterPhase1") {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    FoundFood();
                }
            }
            else if (step == "FoundFood")
            {
                timer_foundFood.text = timer.ToString("f0");
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    ChooseNextMove();
                }
            }
            else if (step == "ChooseNextMove")
            {
                timer_chooseNextMove.text = timer.ToString("f0");
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    FinalizeFood();
                }
            }
            else if (step == "FinalizeFood")
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (playerRole == "Seeker")
                    {
                        for (int i = 0; i < spawnPoints.Length; i++)
                        {
                            SpawnManager.GetInstance().HideFood(spawnedFood[i]);
                        }
                    }
                    ui_phase2.EnterPhase2();
                    step = "Stop";
                }
            }

        }

        public void EnterPhase1()
        {
            Debug.Log("# PHASE 1 #");
            step = "EnterPhase1";

            P0_SwitchRole.gameObject.SetActive(false);
            P1_Title.gameObject.SetActive(true);

            player = PhotonNetwork.LocalPlayer;
            playerName = PlayerPrefs.GetString(PrefsKeys.playerName);
            playerIndex = player.GetTeam();
            int roleIndex = player.GetRole();
            if (roleIndex == 0) { playerRole = "Seeker"; }
            else if (roleIndex == 1) { playerRole = "Guard"; }
            calorieRecord = PhotonNetwork.CurrentRoom.GetCalorieRecord();
            roleRecord = PhotonNetwork.CurrentRoom.GetRoleRecord();

            timer = 3f;
        }

        public void FoundFood()
        {
            step = "FoundFood";
            Debug.Log("Step: " + step);

            if (playerRole == "Seeker")
            {
                P1_Title.gameObject.SetActive(false);
                P1_FoundFood.gameObject.SetActive(true);
                ShowItems();
            }
            
            timer = 5f;
        }

        public void ShowItems()
        {
            spawnPoints = GameManager.GetInstance().teams[playerIndex].spawnPoints;
            for (int i=0; i<spawnPoints.Length; i++)
            {
                spawnedFood[i] = SpawnManager.GetInstance().SpawnFood(spawnPoints[i]);
            }
        }

        public void ChooseNextMove()
        {
            step = "ChooseNextMove";
            Debug.Log("Step: " + step);

            if (playerRole == "Seeker")
            {
                P1_FoundFood.gameObject.SetActive(false);
                P1_ChooseNextMove.gameObject.SetActive(true);
            }
            timer = 10f;
        }
        public void KeepItems()
        {
            P1_ChooseNextMove.gameObject.SetActive(false);
        }
        public void ChangeItems()
        {
            for (int i= 0; i < spawnPoints.Length; i++)
            {
                SpawnManager.GetInstance().HideFood(spawnedFood[i]);
                spawnedFood[i] = SpawnManager.GetInstance().SpawnFood(spawnPoints[i]);
            }
            P1_ChooseNextMove.gameObject.SetActive(false);
        }


        public void FinalizeFood()
        {
            step = "FinalizeFood";
            Debug.Log("Step: " + step);

            if (playerRole == "Seeker")
            {
                P1_ChooseNextMove.gameObject.SetActive(false);
                P1_Finalize.gameObject.SetActive(true);
            } 
            timer = 5f;
        }
        public List<GameObject> GetSpawnedFood()
        {
            List<GameObject> spawnedFoodList = new List<GameObject>();

            foreach (GameObject food in spawnedFood)
            {
                spawnedFoodList.Add(food);
            }
            return spawnedFoodList;
        }

    }
}
