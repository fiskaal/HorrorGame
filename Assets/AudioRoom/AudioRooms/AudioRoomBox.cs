using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioRoomRep;


namespace AudioRoomRep.Rooms
{
    public class AudioRoomBox : MonoBehaviour, AudioRoomArea
    {
        public string key;

        public Vector3 pos = Vector3.zero;
        public Vector3 size = new Vector3(1, 1, 1);

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
            delta.x = Mathf.Max(Mathf.Min(delta.x, size.x), -size.x);
            delta.y = Mathf.Max(Mathf.Min(delta.y, size.y), -size.y);
            delta.z = Mathf.Max(Mathf.Min(delta.z, size.z), -size.z);
            return actualPos - delta;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.43921568f, 0.43921568f, 1f);
            Gizmos.DrawWireCube(transform.position + pos, size * 2);
        }



    }
}
