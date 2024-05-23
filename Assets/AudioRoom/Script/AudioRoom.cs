using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioRoomRep
{

    public static class AudioRoom
    {
        public static List<AudioRoomStructure> structures = new List<AudioRoomStructure>();

        public static void Register(AudioRoomArea ara, string key)
        {
            foreach (AudioRoomStructure a in structures)
                if (a.key == key)       // group
                {
                    a.Add(ara);
                    return;
                }

            //new Structure
            AudioRoomStructure f = new AudioRoomStructure();
            f.key = key;
            f.Add(ara);
            structures.Add(f);
        }

        public static void Unregister(AudioRoomArea ara, string key)
        {
            foreach (AudioRoomStructure a in structures)
                if (a.key == key)
                {
                    a.Remove(ara);
                    if (a.audioRooms.Count == 0) structures.Remove(a);
                    return;
                }
        }

        public static AudioRoomStructure GetStructure(string key)
        {
            foreach (AudioRoomStructure a in structures)
                if (a.key == key) return a;
            return null;
        }

    }
    public class AudioRoomStructure
    {
        public string key;
        public List<AudioRoomArea> audioRooms = new List<AudioRoomArea>();
        public int count=0;


        public void Add(AudioRoomArea a)
        {
            count++;
            audioRooms.Add(a);
        }

        public void Remove(AudioRoomArea a)
        {
            for (int i = 0; i < count; i++)
                if (audioRooms[i] == a)
                {
                    audioRooms.RemoveAt(i);
                    count--;
                    return;
                }
        }

        public Vector3 SingleLocate(Vector3 target)
        {
            return SpecialLocate(target, 0);
        }

        public Vector3 SpecialLocate(Vector3 target, int i)
        {
            return audioRooms[i].Locate(target);
        }

        public Vector3 OptimalLocate(Vector3 target)
        {
            float disWinner, disCurrent;
            disWinner = float.PositiveInfinity;
            Vector3 winner, current;
            winner = Vector3.zero;
            foreach (AudioRoomArea a in audioRooms)
            {
                current = a.Locate(target);
                disCurrent = FastDistance(target, current);
                if (disCurrent < disWinner)
                {
                    disWinner = disCurrent;
                    winner = current;
                }
            }
            return winner;
        }

        public Vector3[] PoolLocate(Vector3 target, int count)
        {
            if (count == 0) return null;
            count = Mathf.Min(count, this.count);

            Vector3[] pool = new Vector3[count];
            float[] poolDistance = new float[count];
            for (int i = 0; i < count; i++)
                poolDistance[i] = float.MaxValue;


            Vector3 current;
            float currentDistance;


            foreach (AudioRoomArea a in audioRooms)
            {
                current = a.Locate(target);
                currentDistance = FastDistance(target, current);

                for (int i = 0; i < count; i++)
                    if (currentDistance < poolDistance[i])
                    {

                        for (int b = count-1; b > i; b--)
                        {
                            poolDistance[b] = poolDistance[b - 1];
                            pool[b] = pool[b - 1];
                        }

                        poolDistance[i] = currentDistance;
                        pool[i] = current;
                        break;
                    }
                
            }



            return pool;
        }

       




        //math
        private float FastDistance(Vector3 a, Vector3 b)
        {
            a = a - b;
            return a.x * a.x + a.y * a.y + a.z * a.z;
        }

    }

    public interface AudioRoomArea
    {
        Vector3 Locate(Vector3 position);
    }

    
}