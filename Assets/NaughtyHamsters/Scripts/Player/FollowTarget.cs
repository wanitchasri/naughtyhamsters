using UnityEngine;

namespace NaughtyHamster
{
    public class FollowTarget : MonoBehaviour
    {
        [HideInInspector] public Camera cam;
        [HideInInspector] public Transform camTransform;

        public Transform target;      
        
        /// Layers to hide after calling HideMask().
        public LayerMask respawnMask;

        public float distance = 8.0f;
        public float height = 8.0f;

        void Start()
        {
            cam = GetComponent<Camera>();
            camTransform = transform;

            Transform listener = GetComponentInChildren<AudioListener>().transform;
            listener.position = transform.position + transform.forward * distance;
        }


        void LateUpdate()
        {
            if (!target)
                return;

            Quaternion currentRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

            Vector3 pos = target.position;
            pos -= currentRotation * Vector3.forward * Mathf.Abs(distance);
            pos.y = target.position.y + Mathf.Abs(height);
            transform.position = pos;

            transform.LookAt(target);
            transform.position = target.position - (transform.forward * Mathf.Abs(distance));
        }
        
        
        /// <summary>
        /// Culls the specified layers of 'respawnMask' by the camera.
        /// </summary>
        public void HideMask(bool shouldHide)
        {
            if(shouldHide) cam.cullingMask &= ~respawnMask;
            else cam.cullingMask |= respawnMask;
        }
    }
}