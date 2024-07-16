using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace NaughtyHamster
{
    public class InvestigationManager : MonoBehaviour
    {
        private static InvestigationManager instance;
        public GameObject[] hamsterPrefabs;
        public GameObject[] food_objects;

        List<string> hamster_foods = new List<string> { "Apple(Clone)", "Watermelon(Clone)", "Cherry(Clone)", "Banana(Clone)" };
        List<string> human_foods = new List<string> { "Onigiri(Clone)", "Hamburger(Clone)", "Cheese(Clone)", "Cake(Clone)" };

        public void Awake()
        {
            instance = this;
        }
        public InvestigationManager GetInstance()
        {
            return instance;
        }

        public List<GameObject> SpawnSuspectClones(int playerIndex, GameObject[] suspectPoints)
        {
            int totalPlayer = PhotonNetwork.CurrentRoom.GetTotalPlayer();
            List<GameObject> spawnedHamsters = new List<GameObject>();
            GameObject suspect;

            int spawningIndex = 0;
            for (int i = 0; i < totalPlayer; i++)
            {
                if (i == playerIndex) {
                    //Debug.Log("this is local player team "+ i);
                    // do nothing
                } else
                {
                    //Debug.Log("ANOTHER player team " + i);
                    suspect = Instantiate(hamsterPrefabs[i], suspectPoints[spawningIndex].transform.position, Quaternion.Inverse(Quaternion.identity));

                    //Debug.Log("(1) ? " + suspect.transform.Find("Canvas/TopText") == null);
                    //Debug.Log("(2) ? " + suspect.transform.Find("Canvas/TopText").transform.GetComponent<Text>().text);
                    //Debug.Log("(3) ? " + NetworkManagerCustom.GetNameByIndex(i));
                    suspect.transform.Find("Canvas/TopText").GetComponent<TMP_Text>().text = NetworkManagerCustom.GetNameByIndex(i);
                    suspect.GetComponent<Animator>().Play("Base Layer.Idle_B", 0);

                    spawnedHamsters.Add(suspect);
                    spawningIndex++;
                }
            }
            return spawnedHamsters;
        }

        public void HideHamsters(List<GameObject> spawnedHamsters)
        {
            foreach (GameObject hamster in spawnedHamsters)
            {
                if (hamster.activeSelf == true)
                {
                    //Debug.Log("set active = false");
                    hamster.SetActive(false);
                }
            }
        }

        public void HandleSuspectRecord(List<string> suspect_names)
        {
            int round = PhotonNetwork.CurrentRoom.GetRound();
            int teamIndex = 0;
            for (int i = 0; i < suspect_names.Count; i++)
            {
                if (suspect_names[i] == "Red(Clone)")
                {
                    teamIndex = 0;
                }
                else if (suspect_names[i] == "Blue(Clone)")
                {
                    teamIndex = 1;
                }
                else if (suspect_names[i] == "Green(Clone)")
                {
                    teamIndex = 2;
                }
                else if (suspect_names[i] == "Yellow(Clone)")
                {
                    teamIndex = 3;
                }
                PhotonNetwork.CurrentRoom.UpdateSuspectRecord(teamIndex, round);
            }

        }

        public List<GameObject> RevealFood(string[] toRevealFood, GameObject[] revealPoints)
        {
            List<GameObject> revealedList = new List<GameObject>();
            for (int i = 0; i < toRevealFood.Length; i++)
            {
                //Debug.Log("Spawning..");
                string toReveal = toRevealFood[i];
                foreach (GameObject food in food_objects)
                {
                    //Debug.Log("toReveal" + toReveal + "??food.name" + food.name);
                    string nameToCompare = food.name + "(Clone)";
                    if (toReveal == nameToCompare)
                    {
                        //Debug.Log("spawn to reveal");
                        GameObject revealed = Instantiate(food, revealPoints[i].transform.position, Quaternion.identity);
                        revealed.GetComponent<Rigidbody>().useGravity = true;
                        revealedList.Add(revealed);
                    }
                }
            }
            return revealedList;
        }
        
        public GameObject RevealSuspect(int suspectIndex, GameObject suspectPoint)
        {
            GameObject suspect = Instantiate(hamsterPrefabs[suspectIndex], suspectPoint.transform.position, Quaternion.identity);
            Animator suspectAnimator = suspect.gameObject.GetComponent<Animator>();
            suspectAnimator.Play("Base Layer.Die", 0);
            return suspect;
        } 

        public bool guardIsCorrect(string[] toRevealFood)
        {
            bool result = false;
            for (int i = 0; i < toRevealFood.Length; i++)
            {
                //Debug.Log("food "+toRevealFood[i]);
                if (human_foods.Contains(toRevealFood[i]))
                {
                    //Debug.Log("This is human food!");
                    result = true;
                    break;
                }
            }
            return result;
        }

        public (int, int) CalculateCalories(string[] toRevealFood, string result)
        {
            int guard_cal = 0;
            int seeker_cal = 0;

            switch (result)
            {
                case "Correct":
                    Debug.Log("Calculate for Correct Inv");
                    List<string> ofHuman = findHumanFood(toRevealFood);
                    List<string> ofHamster = findHamsterFood(toRevealFood);

                    int human_cal = 0;
                    for (int i = 0; i < ofHuman.Count; i++)
                    {
                        human_cal += checkCalories(ofHuman[i]);
                    }
                    int hamster_cal = 0;
                    for (int i = 0; i < ofHamster.Count; i++)
                    {
                        hamster_cal += checkCalories(ofHamster[i]);
                    }
                    guard_cal += human_cal;
                    seeker_cal += hamster_cal;
                    seeker_cal -= human_cal;
                    break;

                case "False":
                    for (int i = 0; i < toRevealFood.Length; i++)
                    {
                        string food = toRevealFood[i];
                        seeker_cal += checkCalories(food);
                    }
                    guard_cal -= (seeker_cal / 2);
                    seeker_cal += (seeker_cal / 2);
                    break;
            }

            Debug.Log("seeker_cal = " + seeker_cal);
            Debug.Log("guard_cal = " + guard_cal);
            return (guard_cal, seeker_cal);
        }

        public int checkCalories(string food_name)
        {
            int calorie = 0;
            if (hamster_foods.Contains(food_name))
            {
                if (food_name == "Apple(Clone)")
                {
                    calorie += 2;
                }
                else if (food_name == "Watermelon(Clone)")
                {
                    calorie += 3;
                }
                else if (food_name == "Cherry(Clone)")
                {
                    calorie += 4;
                }
                else if (food_name == "Banana(Clone)")
                {
                    calorie += 5;
                }

            } else if (human_foods.Contains(food_name))
            {
                if (food_name == "Onigiri(Clone)")
                {
                    calorie += 7;
                }
                else if (food_name == "Hamburger(Clone)")
                {
                    calorie += 8;
                }
                else if (food_name == "Cheese(Clone)")
                {
                    calorie += 9;
                }
                else if (food_name == "Cake(Clone)")
                {
                    calorie += 10;
                }
            }
            return calorie;
        }

        public List<string> findHumanFood(string[] toRevealFood)
        {
            Debug.Log("findHumanFood..");
            List<string> foodList = new List<string>();
            for (int i = 0; i < toRevealFood.Length; i++)
            {
                string food_name = toRevealFood[i];
                if (human_foods.Contains(food_name))
                {
                    foodList.Add(food_name);
                    Debug.Log(food_name);
                }
            }
            return foodList;
        }

        public List<string> findHamsterFood(string[] toRevealFood)
        {
            Debug.Log("findHamsterFood..");
            List<string> foodList = new List<string>();
            for (int i = 0; i < toRevealFood.Length; i++)
            {
                string food_name = toRevealFood[i];
                if (hamster_foods.Contains(food_name))
                {
                    foodList.Add(food_name);
                    Debug.Log(food_name);
                }
            }
            return foodList;
        }


    }
}

        