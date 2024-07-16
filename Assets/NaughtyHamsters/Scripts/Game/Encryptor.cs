using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace NaughtyHamster
{
    /// <summary>
    /// Encrypts strings passed in by using a set obfuscation key as well as
    /// the unique identifier of the running device. Used for storing in-app purchases.
    /// </summary>
    public class Encryptor : MonoBehaviour
    {
        private static Encryptor instance;

        public bool encrypt = false;
        public string secret = "abcd1234";

		void Awake()
		{
			instance = this;
		}

        public static Encryptor GetInstance()
        {
			return instance;
        }


        /// <summary>
        /// Encrypt string based on secret key + device identifier.
        /// </summary>
        public static string Encrypt(string toEncrypt)
        {
            //if encryption is not turned on, just return it back
            if (!instance.encrypt) return toEncrypt;
            //attach device identifier to the encryption string
            toEncrypt += SystemInfo.deviceUniqueIdentifier;
			
            #pragma warning disable 0219
            //convert secret key and input string to byte array
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(instance.secret);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            byte[] resultArray = null;

            //return encrypted string
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

		
        /// </summary>
        /// Decrypt string based on secret key + device identifier.
        /// </summary>
        public static string Decrypt(string toDecrypt)
        {
            //if encryption is not turned on, just return it back
            if (!instance.encrypt) return toDecrypt;
           
            #pragma warning disable 0219
            //convert secret key and input string to byte array
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(instance.secret);
            byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
            byte[] resultArray = null;

            return (UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length))
				   .Replace(SystemInfo.deviceUniqueIdentifier, String.Empty);
        }
    }
}