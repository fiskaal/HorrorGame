using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using AudioRoomRep.Rooms;

namespace AudioRoomRep.RoomsEditor
{
    [CustomEditor(typeof(AudioRoomSphere))]
    public class AudioRoomSphereEditor : Editor
    {
        SphereBoundsHandle b = new SphereBoundsHandle();
        AudioRoomSphere a;
        bool edit = false;

        private void Awake()
        {
            a = (target as AudioRoomSphere);
            b.center = a.pos + a.transform.position;
            b.radius = a.radius;
        }
        public void OnSceneGUI()
        {
            if (edit)
            {
                a = (target as AudioRoomSphere);
                Handles.color = Color.blue;
                a.pos = b.center - a.transform.position;
                a.radius = b.radius;
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