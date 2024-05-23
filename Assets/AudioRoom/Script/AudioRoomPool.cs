using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioRoomRep.Optional
{
    public class AudioRoomPool : MonoBehaviour
    {
        private static Transform listener;
        private AudioRoomStructure ars;
        public string key;
        public float height;
        public Transform[] targets;


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

            Vector3[] pool = ars.PoolLocate(listener.position, targets.Length);
            if (pool == null) return;

            for(int i=0; i<pool.Length;i++)
            {
                targets[i].position = pool[i];
                targets[i].position += new Vector3(0, height, 0);
            }

            


        }

    }
}