using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioRoomRep;

namespace AudioRoomRep.Optional
{
    public class AudioRoomFollow : MonoBehaviour
    {
        private static Transform listener;
        private AudioRoomStructure ars;
        public string key;
        public float height;

        

        private void Start()
        {
            ars = AudioRoom.GetStructure(key);
        }


        public void ChangeKey(string newkey)
        {
            key = newkey;
            ars = AudioRoom.GetStructure(key);
        }

        void Update()
        {
            if (listener == null)
            {
                listener = FindObjectOfType<AudioListener>().transform;
                return;
            }
            if (ars == null)
            {
                Debug.LogError("Can't find any AudioRoom of key:" + key);
                return;
            }

            transform.position = ars.OptimalLocate(listener.position);

            transform.position += new Vector3(0, height, 0);


        }


    }

}