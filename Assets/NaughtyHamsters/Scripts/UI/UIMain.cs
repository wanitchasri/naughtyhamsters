using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NaughtyHamster
{
    public class UIMain : MonoBehaviour
    {
        /* ----------------------------------- */
        /* CANVAS */
        /* ----------------------------------- */

        public GameObject loadingWindow;
        public GameObject connectionErrorWindow;

        /* ----------------------------------- */
        /* FIELDS */
        /* ----------------------------------- */

		public InputField nameField;
        public Dropdown networkDrop;
		public InputField serverField;
        public Toggle musicToggle;
        public Slider volumeSlider;

        void Start()
        {
            if (!PlayerPrefs.HasKey(PrefsKeys.playerName)) PlayerPrefs.SetString(PrefsKeys.playerName, "Player" + System.String.Format("{0:0000}", Random.Range(1, 9999)));
            if (!PlayerPrefs.HasKey(PrefsKeys.networkMode)) PlayerPrefs.SetInt(PrefsKeys.networkMode, 0);
            if (!PlayerPrefs.HasKey(PrefsKeys.gameMode)) PlayerPrefs.SetInt(PrefsKeys.gameMode, 0);
            if (!PlayerPrefs.HasKey(PrefsKeys.serverAddress)) PlayerPrefs.SetString(PrefsKeys.serverAddress, "127.0.0.1");
            if (!PlayerPrefs.HasKey(PrefsKeys.playMusic)) PlayerPrefs.SetString(PrefsKeys.playMusic, "true");
            if (!PlayerPrefs.HasKey(PrefsKeys.appVolume)) PlayerPrefs.SetFloat(PrefsKeys.appVolume, 1f);
            if (!PlayerPrefs.HasKey(PrefsKeys.activeModel)) PlayerPrefs.SetString(PrefsKeys.activeModel, Encryptor.Encrypt("0"));

            PlayerPrefs.Save();

            //read the selections and set them in the corresponding UI elements
            nameField.text = PlayerPrefs.GetString(PrefsKeys.playerName);
            networkDrop.value = PlayerPrefs.GetInt(PrefsKeys.networkMode);
            serverField.text = PlayerPrefs.GetString(PrefsKeys.serverAddress);
            musicToggle.isOn = bool.Parse(PlayerPrefs.GetString(PrefsKeys.playMusic));
            volumeSlider.value = PlayerPrefs.GetFloat(PrefsKeys.appVolume);

            //call the onValueChanged callbacks once with their saved values
            OnMusicChanged(musicToggle.isOn);
            OnVolumeChanged(volumeSlider.value);

            //listen to network connection and IAP billing errors
            NetworkManagerCustom.connectionFailedEvent += OnConnectionError;

        }


        /// <summary>
        /// Tries to enter the game scene. Sets the loading screen active while connecting to the
        /// Matchmaker and starts the timeout coroutine at the same time.
        /// </summary>
        public void Play()
        {
            //UnityAnalyticsManager.MainSceneClosed(shopOpened, settingsOpened, musicToggle.isOn,
            //                      Encryptor.Decrypt(PlayerPrefs.GetString(PrefsKeys.activeTank)));

            loadingWindow.SetActive(true);
            NetworkManagerCustom.StartMatch((NetworkMode)PlayerPrefs.GetInt(PrefsKeys.networkMode));
            StartCoroutine(HandleTimeout());
        }


        //coroutine that waits 10 seconds before cancelling joining a match
        IEnumerator HandleTimeout()
        {
            yield return new WaitForSeconds(10);

            Photon.Pun.PhotonNetwork.Disconnect();
            OnConnectionError();
        }

        void OnConnectionError()
        {
            //game shut down completely
            if (this == null)
                return;

            StopAllCoroutines();
            loadingWindow.SetActive(false);
            connectionErrorWindow.SetActive(true);
        }

        /* ----------------------------------- */
        /* ON VALUE CHANGES */
        /* ----------------------------------- */

        /// <summary>
        /// Save newly selected GameMode value to PlayerPrefs in order to check it later.
        /// Called by DropDown onValueChanged event.
        /// </summary>
        public void OnGameModeChanged(int value)
        {
            PlayerPrefs.SetInt(PrefsKeys.gameMode, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Modify music AudioSource based on player selection.
        /// Called by Toggle onValueChanged event.
        /// </summary>
        public void OnMusicChanged(bool value)
        {
            AudioManager.GetInstance().musicSource.enabled = musicToggle.isOn;
            AudioManager.PlayMusic(0);
        }

        public void OnVolumeChanged(float value)
        {
            volumeSlider.value = value;
            AudioListener.volume = value;
        }

        /// <summary>
        /// Saves all player selections chosen in the Settings window on the device.
        /// </summary>
        public void CloseSettings()
        {
            PlayerPrefs.SetString(PrefsKeys.playerName, nameField.text);
            PlayerPrefs.SetInt(PrefsKeys.networkMode, networkDrop.value);
            PlayerPrefs.SetString(PrefsKeys.serverAddress, serverField.text);
            PlayerPrefs.SetString(PrefsKeys.playMusic, musicToggle.isOn.ToString());
            PlayerPrefs.SetFloat(PrefsKeys.appVolume, volumeSlider.value);
            PlayerPrefs.Save();
        }
    }
}