using UnityEngine;

namespace NaughtyHamster
{
    public class DontDestroyManager : MonoBehaviour
    {
        private static DontDestroyManager instance;
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}