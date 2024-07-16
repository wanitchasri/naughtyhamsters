using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace NaughtyHamster
{
    public class RoleManager : MonoBehaviour
    {
        private static RoleManager instance;

        public void Awake()
        {
            instance = this;
        }
        public RoleManager GetInstance()
        {
            return instance;
        }

        public string checkRole(int playerIndex)
        {
            int round = PhotonNetwork.CurrentRoom.GetRound();
            int[] roleRecord = PhotonNetwork.CurrentRoom.GetRoleRecord();
            int role = 0;

            for (int i = 0; i < roleRecord.Length; i++)
            {
                if (i == playerIndex)
                {
                    role = roleRecord[i];
                }
            }

            if (role == round) { return "Guard"; }
            else { return "Seeker"; }
        }
    }
}
