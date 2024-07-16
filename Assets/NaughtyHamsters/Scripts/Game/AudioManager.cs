using UnityEngine;
using UnityEngine.SceneManagement;

namespace NaughtyHamster
{

	public class AudioManager : MonoBehaviour
	{
		private static AudioManager instance;

		public AudioSource musicSource;
		public AudioSource audioSource;
        public GameObject oneShotPrefab;
        public AudioClip[] musicClips;
        
		void Awake()
		{
            if (instance != null) { return; }

            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
		}

		public static AudioManager GetInstance()
		{
			return instance;
		}

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            musicSource.Stop();
        }

        public static void PlayMusic(int index)
        {
            instance.musicSource.clip = instance.musicClips[index];
            if (instance.musicSource.enabled) { instance.musicSource.Play(); }
        }
    }
}

