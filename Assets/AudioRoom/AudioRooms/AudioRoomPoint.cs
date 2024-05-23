using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioRoomRep;



namespace AudioRoomRep.Rooms
{
    public class AudioRoomPoint : MonoBehaviour, AudioRoomArea
    {
        public string key;
        public Vector3 pos = Vector3.up;





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



        Vector3 AudioRoomArea.Locate(Vector3 position)
        {
            return pos+transform.position;
        }


        public void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.43921568f, 0.43921568f, 1f);
            Gizmos.DrawIcon(pos + transform.position,"AudioRoomPoint");
            Gizmos.DrawLine(transform.position, transform.position + pos);
        }
    }
}