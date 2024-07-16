using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace NaughtyHamster
{
    public class Player : MonoBehaviourPunCallbacks, IPunObservable
    {
        /* ----------------------------------- */
        /* VALUE COMPONENTS */
        /* ----------------------------------- */

        /// <summary>
        /// Maximum calorie value at game start.
        /// </summary>
        public int maxCalorie = 50;

        /// <summary>
        /// Initial player's role (0=seeker, 1=guard)
        /// </summary>
        public int initialRole = 0;

        /* ----------------------------------- */
        /* UI COMPONENTS */
        /* ----------------------------------- */

        /// <summary>
        /// UI Text displaying the player name.
        /// </summary>
        public Text nameLabel;

        /// <summary>
        /// UI Object indicating player's role
        /// </summary>
        public GameObject roleIndicator;

        public Transform face;

        /// <summary>
        /// MeshRenderers that should be highlighted in team color.
        /// </summary>
        public SkinnedMeshRenderer[] renderers;

        /// <summary>
        /// Reference to the camera following component.
        /// </summary>
        [HideInInspector] public FollowTarget camFollow;

        //reference to this rigidbody
        #pragma warning disable 0649
        private Rigidbody rb;
#pragma warning restore 0649

        [HideInInspector] [SerializeField]
        Animator animator;

        void Awake()
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            //set players current health value after joining
            GetView().SetCalorie(maxCalorie);
            GetView().SetRole(initialRole);
        }

        void Start()
        {           
			//get corresponding team and colorize renderers in team color
            Team team = GameManager.GetInstance().teams[GetView().GetTeam()];
            for(int i = 0; i < renderers.Length; i++)
                renderers[i].material = team.material;

            animator = this.gameObject.GetComponent<Animator>();
            animator.Play("Base Layer.Bye", 0);

            nameLabel.text = GetView().GetName();

            if (!photonView.IsMine)
                return;

			//set a global reference to the local player
            GameManager.GetInstance().localPlayer = this;

			//get components and set camera target
            rb = GetComponent<Rigidbody>();
            camFollow = Camera.main.GetComponent<FollowTarget>();
            camFollow.target = face;

        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player player, ExitGames.Client.Photon.Hashtable playerAndUpdatedProps)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (playerAndUpdatedProps.ContainsKey("cheek")) { OnCheekChanged(player, player.GetCheek()); }
            }

            //only react on property changes for this player
            if (player != photonView.Owner)
                return;

            //update values that could change any time for visualization to stay up to date
            if (playerAndUpdatedProps.ContainsKey("role")) { OnRoleChanged(player.GetRole()); }
        }

        public PhotonView GetView()
        {
            return this.photonView;
        }

        /* ----------------------------------- */
        /* ON VALUE CHANGES */
        /* ----------------------------------- */

        //hook for updating role locally (the actual value updates via player properties)
        protected void OnRoleChanged(int value)
        {
            // seeker=0, guard=1
            if (value == 0)
            {
                roleIndicator.SetActive(false);
                this.gameObject.GetComponent<Animator>().Play("Base Layer.Walk", 0);
            } else if (value == 1)
            {
                roleIndicator.SetActive(true);
                this.gameObject.GetComponent<Animator>().Play("Base Layer.Idle_A", 0);
            }
        }

        protected void OnCheekChanged(Photon.Realtime.Player player, string value)
        {
            int teamID = player.GetTeam();
            PhotonNetwork.CurrentRoom.SetCheekRecords(teamID, value);
        }

        /* ----------------------------------- */
        /* GAMEOVER */
        /* ----------------------------------- */

        //called on all clients on game end providing the winning team
        //[PunRPC]
        //protected void RpcGameOver(byte teamIndex)
        //{
        //    //display game over window
        //    // GameManager.GetInstance().DisplayGameOver(teamIndex);
        //}

        public void ResetPosition()
        {
            //start following the local player again
            camFollow.target = face;
            camFollow.HideMask(false);

            //get team area and reposition it there
            transform.position = GameManager.GetInstance().GetSpawnPosition(GetView().GetTeam());

            //reset forces modified by input
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        //this method gets called multiple times per second, at least 10 times or more
        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {

            }
            else
            {

            }
        }

    }
}