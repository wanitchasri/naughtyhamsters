using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace NaughtyHamster
{

    public class UIPhase3 : MonoBehaviourPunCallbacks
    {
        [HideInInspector] Photon.Realtime.Player player;
        [HideInInspector] public string playerName;
        [HideInInspector] public int playerIndex;
        [HideInInspector] public string playerRole;
        [HideInInspector] public int[] calorieRecord;
        [HideInInspector] public int[] roleRecord;
        [HideInInspector] public int round;

        [HideInInspector] public string step;
        [HideInInspector] public float timer;

        public GameObject P2_Title;
        public GameObject P2_Return;
        public GameObject P3_Title;
        public GameObject P3_Talk;
        public GameObject P3_ChooseSuspects;
        public GameObject P3_GuardIsChoosing;
        public GameObject P3_TruthReveals;
        public GameObject P3_InvestigationResult;

        public GameObject P4_Winner;
        public GameObject P4_Swap;
        public GameObject ScoreBoard;

        public TMP_Text timer_talk;
        public TMP_Text timer_chooseSuspects;
        public TMP_Text timer_guardIsChoosing;
        public TMP_Text suspect_text;
        public TMP_Text result_text;
        public TMP_Text winner_text;

        [HideInInspector] public int totalPlayer;

        public InvestigationManager InvestigationManager;
        [HideInInspector] public List<GameObject> spawnedSuspects;
        [HideInInspector] public GameObject[] suspectPoints;
        [HideInInspector] public List<string> hamsterTeam = new List<string> { "Red(Clone)", "Blue(Clone)", "Green(Clone)", "Yellow(Clone)" };
        [HideInInspector] public List<GameObject> suspect_objects = new List<GameObject>();
        [HideInInspector] public List<string> suspect_names = new List<string>();

        //[HideInInspector] public int currentSuspectIndex;

        [HideInInspector] public int counter;

        [HideInInspector] public string[] cheek;
        [HideInInspector] public int[] suspectRecord;
        [HideInInspector] public GameObject[] toRevealPoints;
        [HideInInspector] public GameObject toRevealSuspect;
        [HideInInspector] public List<GameObject> revealedList;
        [HideInInspector] public GameObject revealedSuspect;

        [HideInInspector] public string[] toRevealFood;
        [HideInInspector] public string investigation_result;

        [HideInInspector] public bool revealTime;
        [HideInInspector] int toRemove;
        [HideInInspector] public string[] nameList;

        public Camera main_camera;
        public Camera camera_2;

        /* UPDATES */
        private void Update()
        {
            if (step == "EnterPhase3")
            {
                //counter = PhotonNetwork.CurrentRoom.GetUpdateCounter();

                timer -= Time.deltaTime;
                if ((timer <= 0))
                {
                    Talk();
                }
            }
            else if (step == "Talk")
            {
                timer_talk.text = timer.ToString("f0");
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    ChooseSuspects();
                }
            }
            else if (step == "ChooseSuspects")
            {
                timer -= Time.deltaTime;
                if (playerRole == "Guard")
                {
                    timer_chooseSuspects.text = timer.ToString("f0");
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            string clickedName = hit.transform.name;
                            if (hamsterTeam.Contains(clickedName))
                            {
                                suspect_names.Add(clickedName);

                                GameObject suspectObject = hit.transform.gameObject;
                                suspect_objects.Add(suspectObject);
                                suspectObject.SetActive(false);
                            }
                        }
                    }
                }
                else
                {
                    timer_guardIsChoosing.text = timer.ToString("f0");
                }

                if (timer <= 0)
                {
                    if (playerRole == "Guard")
                    {
                        Debug.Log("----SUSPECT LIST---");
                        foreach (var name in suspect_names)
                        {
                            Debug.Log("name = " + name);
                        }
                        InvestigationManager.GetInstance().HideHamsters(spawnedSuspects);
                        UpdateSuspectRecord(suspect_names);
                        PhotonNetwork.CurrentRoom.SetStatus("recordedSuspect");

                        //roleRecord = PhotonNetwork.CurrentRoom.GetRoleRecord();
                        //Debug.Log("-------ROLE RECORD-------");
                        //for (int i = 0; i < roleRecord.Length; i++)
                        //{
                        //    Debug.Log("team" + i + " = " + roleRecord[i]);
                        //}
                    }

                    if (PhotonNetwork.CurrentRoom.GetStatus() == "recordedSuspect")
                    {
                        TruthReveals();
                        revealTime = true;
                    }
                }
            }
            else if (step == "TruthReveals")
            {
                //Debug.Log("timer " + timer.ToString("f0"));
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    P4_Swap.SetActive(false);
                    step = "EndRound";
                    Debug.Log("Step: " + step);
                }
                else if (timer <= 5f)
                {
                    P4_Winner.SetActive(false);
                    P4_Swap.SetActive(PhotonNetwork.CurrentRoom.GetRound() != PhotonNetwork.CurrentRoom.GetTotalPlayer());
                }
                else if (timer <= 8f)
                {
                    int naughtiestIndex = findTheNaughtiest();
                    winner_text.text = nameList[naughtiestIndex];
                    P3_InvestigationResult.gameObject.SetActive(false);
                    P4_Winner.SetActive(true);
                }
                else if (timer <= 12f)
                {
                    //Debug.Log("Time to hide revealed items " + revealTime);
                    InvestigationManager.GetInstance().HideHamsters(revealedList);
                    revealedSuspect.SetActive(false);
                    if (!revealTime)
                    {
                        Debug.Log("toRemove" + toRemove);
                        PhotonNetwork.CurrentRoom.UpdateSuspectRecord(toRemove, 0);
                        P3_TruthReveals.gameObject.SetActive(false);
                        P3_InvestigationResult.gameObject.SetActive(true);
                        result_text.text = investigation_result;

                        timer = 22f;
                        revealTime = true;
                    } 
                }
                else if (timer <= 20f) 
                {
                    if (revealTime)
                    {
                        P3_InvestigationResult.SetActive(false);
                        suspectRecord = PhotonNetwork.CurrentRoom.GetSuspectRecord();
                        round = PhotonNetwork.CurrentRoom.GetRound();
                        totalPlayer = PhotonNetwork.CurrentRoom.GetTotalPlayer();
                        //Debug.Log("Check for Suspects.. of round" + round);
                        for (int i = 0; i < totalPlayer; i++)
                        {
                            //Debug.Log("team" + i + "record" + suspectRecord[i]);
                            if (suspectRecord[i] == round)
                            {
                                Debug.Log("team " + i + " is Suspect");
                                //revealedList = RevealCheek(i);
                                RevealCheek(i);
                                toRemove = i;
                                revealTime = false;
                                break;
                            }
                        }
                    }
                    
                }

            }
            else if (step == "EndRound")
            {
                if (totalPlayer != round)
                {
                    if (PhotonNetwork.IsMasterClient) {
                        PhotonNetwork.CurrentRoom.ClearCheek();
                    }
                    GameManager.GetInstance().RunGameFlow();
                }
                else if (totalPlayer == round)
                {
                    GameManager.GetInstance().DisplayGameOver(playerIndex, nameList[findTheNaughtiest()]);
                }
                step = "Stop";
            }
        }

        /* STEPS */
        public void EnterPhase3()
        {
            string cals = ""; foreach (var x in PhotonNetwork.CurrentRoom.GetCalorieRecord()) { cals += ", " + x; }
            Debug.Log("Calories" + cals);

            string cheek = ""; foreach (var x in PhotonNetwork.CurrentRoom.GetCheekRecords()) { cheek += ", " + x; }
            Debug.Log("Cheek" + cheek);

            Debug.Log("# PHASE 3 #");
            step = "EnterPhase3";

            player = PhotonNetwork.LocalPlayer;
            playerName = player.NickName;
            // playerName = PlayerPrefs.GetString(PrefsKeys.playerName);
            playerIndex = player.GetTeam();

            int roleIndex = player.GetRole();
            if (roleIndex == 0) { playerRole = "Seeker"; }
            else if (roleIndex == 1) { playerRole = "Guard"; }

            if (playerRole == "Seeker")
            {
                P2_Return.gameObject.SetActive(false);
            } else
            {
                P2_Title.gameObject.SetActive(false);
            }
            P3_Title.gameObject.SetActive(true);

            timer = 3f;
        }

        public void Talk()
        {
            step = "Talk";
            Debug.Log("Step: " + step);

            P3_Title.gameObject.SetActive(false);
            P3_Talk.gameObject.SetActive(true);

            timer = 30f;
        }

        public void ChooseSuspects()
        {
            step = "ChooseSuspects";
            Debug.Log("Step: " + step);

            if (playerRole == "Seeker")
            {
                P3_Talk.gameObject.SetActive(false);
                P3_GuardIsChoosing.gameObject.SetActive(true);
            } else
            {
                P3_Talk.gameObject.SetActive(false);
                P3_ChooseSuspects.gameObject.SetActive(true);
                suspectPoints = GameManager.GetInstance().teams[playerIndex].suspectPoints;
                spawnedSuspects = InvestigationManager.GetInstance().SpawnSuspectClones(playerIndex, suspectPoints);
            }
            timer = 15f;
        }

        public void UpdateSuspectRecord(List<string> names)
        {

            int teamIndex = 0;
            round = PhotonNetwork.CurrentRoom.GetRound();
            foreach (var n in names)
            {
                if (n == "Red(Clone)")
                {
                    teamIndex = 0;
                } 
                else if (n == "Blue(Clone)")
                {
                    teamIndex = 1;
                }
                else if (n == "Green(Clone)")
                {
                    teamIndex = 2;
                }
                else if (n == "Yellow(Clone)")
                {
                    teamIndex = 3;
                }
                //Debug.Log("Suspect is on team " + teamIndex);
                PhotonNetwork.CurrentRoom.UpdateSuspectRecord(teamIndex, round);
            }
        }

        public void TruthReveals()
        {
            timer = 22f;
            step = "TruthReveals";
            Debug.Log("Step: " + step);

            if (playerRole == "Guard") { P3_ChooseSuspects.gameObject.SetActive(false); }
            if (playerRole == "Seeker") { P3_GuardIsChoosing.gameObject.SetActive(false); }

            string suspects = ""; foreach (var x in PhotonNetwork.CurrentRoom.GetSuspectRecord()) { suspects += ", " + x; }
            Debug.Log("Suspects" + suspects);

            //main_camera.enabled = false;
            //camera_2.enabled = true;
        }

        public void RevealCheek(int suspectIndex)
        {
            Debug.Log("Revealing Cheek of team "+ suspectIndex);

            cheek = PhotonNetwork.CurrentRoom.GetCheekRecords();
            toRevealPoints = GameManager.GetInstance().teams[playerIndex].revealPoints;
            toRevealSuspect = GameManager.GetInstance().teams[playerIndex].otherTeamPoints;

            int suspect_cal = 0;
            int guard_cal = 0;

            P3_TruthReveals.gameObject.SetActive(true);

            nameList = PhotonNetwork.CurrentRoom.GetNames().Split(',');

            suspect_text.text = nameList[suspectIndex];
            string cheekStr = cheek[suspectIndex].ToString();
            toRevealFood = cheekStr.Split(',');

            string ch = ""; foreach (var x in toRevealFood) { ch += ", " + x; }
            Debug.Log("Cheek" + ch);

            revealedList = InvestigationManager.GetInstance().RevealFood(toRevealFood, toRevealPoints);
            revealedSuspect = InvestigationManager.GetInstance().RevealSuspect(suspectIndex, toRevealSuspect);

            investigation_result = (InvestigationManager.GetInstance().guardIsCorrect(toRevealFood)) ? "Correct" : "False";

            if (player.IsMasterClient)
            {
                (guard_cal, suspect_cal) = InvestigationManager.CalculateCalories(toRevealFood, investigation_result);
                PhotonNetwork.CurrentRoom.UpdateCalorieRecord(findGuardIndex(), guard_cal);
                PhotonNetwork.CurrentRoom.UpdateCalorieRecord(suspectIndex, suspect_cal);
            }

            string cals = ""; foreach (var x in PhotonNetwork.CurrentRoom.GetCalorieRecord()) { cals += ", " + x; }
            Debug.Log("Calories" + cals);

        }



        public int findGuardIndex()
        {
            roleRecord = PhotonNetwork.CurrentRoom.GetRoleRecord();
            int index = 0;
            for (int i=0; i<roleRecord.Length; i++)
            {
                if (roleRecord[i] == round)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public int findTheNaughtiest()
        {
            calorieRecord = PhotonNetwork.CurrentRoom.GetCalorieRecord();
            int index = 0;

            int highest = 0;
            for (int i=0; i<calorieRecord.Length; i++)
            {
                if (calorieRecord[i] > highest)
                {
                    highest = calorieRecord[i];
                    index = i;
                }
            }

            return index;
        }
    }
}
