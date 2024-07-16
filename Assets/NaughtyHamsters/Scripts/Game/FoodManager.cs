using System.Collections.Generic;
using UnityEngine;

namespace NaughtyHamster
{
    public class FoodManager : MonoBehaviour
    {
        private static FoodManager instance;
        public GameObject[] objects;
        List<string> foodList = new List<string>{"Apple(Clone)", "Banana(Clone)", "Watermelon(Clone)", "Cherry(Clone)"
                                            , "Cheese(Clone)", "Hamburger(Clone)", "Onigiri(Clone)", "Cake(Clone)"};

        public void Awake()
        {
            instance = this;
        }
        public FoodManager GetInstance()
        {
            return instance;
        }

        public GameObject SpawnFood(GameObject point)
        {
            //Debug.Log("Random Spawn");
            int random = Random.Range(0, objects.Length);
            GameObject spawned = Instantiate(objects[random], point.transform.position, Quaternion.Euler(0,0,0));
            return spawned;
        }

        public void HideFood(GameObject food)
        {
            food.gameObject.SetActive(false);
        }

        public int CalculateCalories(List<string> cheek)
        {
            int sum = 0;
            for (int i=0; i<cheek.Count; i++)
            {
                string food = cheek[i];
                if (food == "Apple(Clone)")
                {
                    sum += 2;
                }
                else if (food == "Watermelon(Clone)")
                {
                    sum += 3;
                }
                else if (food  == "Cherry(Clone)")
                {
                    sum += 4;
                }
                else if (food  == "Banana(Clone)")
                {
                    sum += 5;
                }
                else if (food == "Onigiri(Clone)")
                {
                    sum += 7;
                }
                else if (food == "Hamburger(Clone)")
                {
                    sum += 8;
                }
                else if (food == "Cheese(Clone)")
                {
                    sum += 9;
                }
                else if (food == "Cake(Clone)")
                {
                    sum += 10;
                }
            }
            return sum;
        }

        public void DestroyFood(List<GameObject> remaining)
        {
            foreach (var food in remaining)
            {
                Destroy(food);
            }
        }
    }
}
