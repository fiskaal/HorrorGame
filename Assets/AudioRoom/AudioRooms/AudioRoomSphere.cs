using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioRoomRep;


namespace AudioRoomRep.Rooms
{
    public class AudioRoomSphere : MonoBehaviour, AudioRoomArea
    {

        public string key;

        public float radius = 10;
        public Vector3 pos = Vector3.zero;

        public void OnDestroy()
        {
            AudioRoom.Unregister(this, key);
        }


        #region it might be too much optimal

        public void OnDisable()
        {
            OnDestroy();
        }

        public void OnEnable()
        {
            AudioRoom.Register(this, key);
        }

        #endregion

        public void ChangeKey(string key)
        {
            OnDestroy();
            this.key = key;
            OnEnable();
        }


        Vector3 AudioRoomArea.Locate(Vector3 target)
        {
            Vector3 actualPos = transform.position + pos;
            Vector3 delta = actualPos - target;

            float distance = Mathf.Sqrt(Mathf.Pow(delta.x, 2) + Mathf.Pow(delta.y, 2) + Mathf.Pow(delta.z, 2));

            delta *= Mathf.Min(radius, distance) / ZeroCheck(distance);


            return actualPos - delta;
        }

        private float ZeroCheck(float a)
        {
            if (a == 0f) return 1;
            return a;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.43921568f, 0.43921568f, 1f);
            Gizmos.DrawWireSphere(pos + transform.position, radius);
        }




    }
}