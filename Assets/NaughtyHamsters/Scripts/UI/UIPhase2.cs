using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;

namespace NaughtyHamster
{

    public class UIPhase2 : MonoBehaviourPunCallbacks
    {
        [HideInInspector] Photon.Realtime.Player player;
        [HideInInspector] public string playerName;
        [HideInInspector] public int playerIndex;
        [HideInInspector] public string playerRole;
        [HideInInspector] public int[] calorieRecord;
        [HideInInspector] public int[] roleRecord;

        [HideInInspector] public List<GameObject> spawned_food;
        [HideInInspector] public List<GameObject> collected_foodObjects;
        [HideInInspector] public List<string> collected_foodNames;

        [HideInInspector] public string step;
        [HideInInspector] public float timer;

        public GameObject P1_Title;
        public GameObject P1_Finalize;
        public GameObject P2_Title;
        public GameObject P2_Store;
        public GameObject P2_Return;

        public TMP_Text timer_store;

        public FoodManager CollectManager;

        public UIPhase1 ui_phase1;
        public UIPhase3 ui_phase3;

        [HideInInspector] List<string> foodList = new List<string>{"Apple(Clone)", "Banana(Clone)", "Watermelon(Clone)", "Cherry(Clone)"
                                            , "Cheese(Clone)", "Hamburger(Clone)", "Onigiri(Clone)", "Cake(Clone)"};

        private void Update()
        {
            if (step == "EnterPhase2")
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Store();
                }
            }
            else if (step == "Store")
            {
                if (playerRole == "Seeker")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            //Debug.Log(hit.transform.name);
                            if (foodList.Contains(hit.transform.name))
                            {
                                collected_foodNames.Add(hit.transform.name);
                                GameObject foodObject = hit.transform.gameObject;
                                collected_foodObjects.Add(foodObject);
                                foodObject.SetActive(false);
                            }
                        }
                    }
                }

                timer_store.text = timer.ToString("f0");
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Return();
                }
            }
            else if (step == "Return")
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    foreach (GameObject collected in collected_foodObjects)
                    {
                        CollectManager.GetInstance().HideFood(collected);
                    }

                    if (collected_foodObjects.Count == 0) 
                    {
                        Debug.Log("--Cheek is Empty--");
                        collected_foodObjects = spawned_food;
                        foreach (GameObject food in collected_foodObjects)
                        {
                            collected_foodNames.Add(food.name);
                        }
                    }
                    step = "EndPhase2";
                }
            }
            else if (step == "EndPhase2")
            {
                ui_phase3.EnterPhase3();
                step = "Stop";
            }

        }

        public void EnterPhase2()
        {
            Debug.Log("# PHASE 2 #");
            step = "EnterPhase2";

            player = PhotonNetwork.LocalPlayer;
            playerName = PlayerPrefs.GetString(PrefsKeys.playerName);
            playerIndex = player.GetTeam();
            //int roleIndex = PlayerPrefs.GetInt(PrefsKeys.playerRole);
            int roleIndex = player.GetRole();
            if (roleIndex == 0) { playerRole = "Seeker"; }
            else if (roleIndex == 1) { playerRole = "Guard"; }
            calorieRecord = PhotonNetwork.CurrentRoom.GetCalorieRecord();
            roleRecord = PhotonNetwork.CurrentRoom.GetRoleRecord();

            collected_foodNames = new List<string>();
            collected_foodObjects = new List<GameObject>();

            if (playerRole == "Seeker")
            {
                spawned_food = ui_phase1.GetInstance().GetSpawnedFood();
                P1_Finalize.gameObject.SetActive(false);
            } else
            {
                P1_Title.gameObject.SetActive(false);
            }
            P2_Title.gameObject.SetActive(true);

            timer = 3f;
        }

        public void Store()
        {
            step = "Store";
            Debug.Log("Step: " + step);

            if (playerRole == "Seeker")
            {
                foreach (var food in spawned_food) { food.gameObject.SetActive(true); }

                P2_Title.gameObject.SetActive(false);
                P2_Store.gameObject.SetActive(true);
            }
            timer = 15f;
        }

        public void DeactivateSpawnedFood()
        {
            foreach (var food in spawned_food)
            {
                if (food.activeSelf == true)
                {
                    food.SetActive(false);
                }
            }
        }

        public void Return()
        {
            step = "Return";
            Debug.Log("Step: " + step);

            if (playerRole == "Seeker")
            {
                DeactivateSpawnedFood();
                Debug.Log("Collected Foods..");
                foreach (var x in collected_foodNames)
                {
                    Debug.Log(x);
                }

                foreach (var food in collected_foodObjects)
                {
                    food.gameObject.SetActive(true);
                }

                string foodForReturn = ConvertFood(collected_foodNames);

                PhotonNetwork.LocalPlayer.SetCheek(foodForReturn);
                // PhotonNetwork.CurrentRoom.SetCheekRecords(playerIndex, foodForReturn);
                PhotonNetwork.CurrentRoom.AddToUpdateCounter(+1);

                Debug.Log("DEBUG-------CHEEK RECORD-------");
                for (int i = 0; i < PhotonNetwork.CurrentRoom.GetCheekRecords().Length; i++)
                {
                    Debug.Log("team " + i + " = " + PhotonNetwork.CurrentRoom.GetCheekRecords()[i]);
                }
                P2_Store.gameObject.SetActive(false);
                P2_Return.gameObject.SetActive(true);
            }
            timer = 5f;
        }

        public string ConvertFood(List<string> collected_foodNames)
        {
            string converted = "";
            foreach (var name in collected_foodNames)
            {
                converted = converted + name + ",";
            }
            Debug.Log("converted string=" + converted);
            return converted;
        }

    }
}
