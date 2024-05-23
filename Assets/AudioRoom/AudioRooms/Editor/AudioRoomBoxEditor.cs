using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using AudioRoomRep.Rooms;

namespace AudioRoomRep.RoomsEditor
{
    [CustomEditor(typeof(AudioRoomBox))]
    public class AudioRoomBoxEditor : Editor
    {
        BoxBoundsHandle b = new BoxBoundsHandle();
        AudioRoomBox a;
        bool edit = false;

        private void Awake()
        {
            a = (target as AudioRoomBox);
            b.center = a.pos + a.transform.position;
            b.size = a.size * 2;
        }
        public void OnSceneGUI()
        {
            if (edit)
            {
                a = (target as AudioRoomBox);
                Handles.color = Color.blue;
                a.pos = b.center - a.transform.position;
                a.size = b.size / 2;
                b.DrawHandle();
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            string s = "Enable editing";
            if (edit) s = "Disable editing";
            if (GUILayout.Button(s))
            {
                Awake();
                edit = !edit;
            }

        }
    }
}