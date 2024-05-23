using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using AudioRoomRep.Rooms;



namespace AudioRoomRep.RoomsEditor
{
    
    [CustomEditor(typeof(AudioRoomPoint))]
    public class AudioRoomPointEditor : Editor
    {
        bool edit = false;
        AudioRoomPoint a;
        

        private void Awake()
        {
            a = (target as AudioRoomPoint);
           
        }


        public void OnSceneGUI()
        {
            if (edit) a.pos = Handles.PositionHandle(a.pos + a.transform.position, Quaternion.identity) - a.transform.position;

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