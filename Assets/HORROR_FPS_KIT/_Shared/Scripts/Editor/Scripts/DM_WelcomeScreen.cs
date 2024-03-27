/*
============================
Unity Assets by Dizzy Media
============================
*/

using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DizzyMedia.Welcome {

    [HelpURL("http://dizmedia.net")]
    [InitializeOnLoad]
    public class DM_WelcomeScreen : EditorWindow {

        private static DM_WelcomeScreen window;
        private static Vector2 windowsSize = new Vector2(500f, 490f);
        private Vector2 scrollPosition;

        private static string windowHeaderText = "Welcome Screen";
        private string copyright = "© Copyright " + DateTime.Now.Year + " Dizzy Media";

        private const string isShowAtStartEditorPrefs = "WelcomeScreenShowAtStart";
        private static bool isShowAtStart = true;

        private static bool isInited;

        private static GUIStyle headerStyle;
        private static GUIStyle copyrightStyle;

        private static Texture2D allOurAssetsIcon;
        private static Texture2D discordIcon;
        private static Texture2D docsIcon;
        private static Texture2D youTubeIcon;
        private static Texture2D vkIcon;
        private static Texture2D facebookIcon;
        private static Texture2D supportIcon;
        private static Texture2D instagramIcon;
        private static Texture2D twitterIcon;

        static DM_WelcomeScreen() {

            EditorApplication.update -= GetShowAtStart;
            EditorApplication.update += GetShowAtStart;

        }

        private void OnGUI() {

            if(!isInited) {

                Init();

            }

            if(GUI.Button(new Rect(0f, 0f, windowsSize.x, 64f), "", headerStyle))

                Process.Start("http://dizmedia.net");

            GUILayoutUtility.GetRect(position.width, 78f);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            if(DrawButton(discordIcon, "Discord", "Dizzy Media's Discord Channel")) 

                Process.Start("https://discord.gg/VEBfEJZ7UR");

            if(DrawButton(docsIcon, "Docs", "Online version of the documentations.")) 

                Process.Start("http://dizmedia.net/");

            if(DrawButton(allOurAssetsIcon, "Our Assets", "Checkout our other assets!")) 

                Process.Start("https://assetstore.unity.com/publishers/16435");

            if(DrawButton(supportIcon, "Support", "First of all, read the docs. If it didn't help, email us.")) 

                Process.Start("http://dizmedia.net/contact-us/");

            if(DrawButton(facebookIcon, "Facebook page", "Check out our FB page!")) 

                Process.Start("https://www.facebook.com/DizzyMedia/");

            if(DrawButton(twitterIcon, "Twitter page", "Check out our twitter feed!")) 

                Process.Start("https://twitter.com/DizzyMediaInc");

            if(DrawButton(youTubeIcon, "YouTube Channel", "Our video materials."))

                Process.Start("https://www.youtube.com/@DizzyMediaYT/featured");

            EditorGUILayout.EndScrollView();

            EditorGUILayout.LabelField(copyright, copyrightStyle);

        }

        private static bool Init() {

            try {

                headerStyle = new GUIStyle();

                headerStyle.normal.background = (Texture2D)Resources.Load("EditorContent/Welcome/HeaderLogo");

                headerStyle.normal.textColor = Color.white;
                headerStyle.fontStyle = FontStyle.Bold;
                headerStyle.padding = new RectOffset(340, 0, 34, 0);
                headerStyle.margin = new RectOffset(0, 0, 0, 0);

                copyrightStyle = new GUIStyle();
                copyrightStyle.alignment = TextAnchor.MiddleRight;

                discordIcon = (Texture2D)Resources.Load("EditorContent/Welcome/Discord");
                docsIcon = (Texture2D)Resources.Load("EditorContent/Welcome/Docs");
                allOurAssetsIcon = (Texture2D)Resources.Load("EditorContent/Welcome/Asset_Circ");
                supportIcon = (Texture2D)Resources.Load("EditorContent/Welcome/Support");
                youTubeIcon = (Texture2D)Resources.Load("EditorContent/Welcome/YouTube");
                facebookIcon = (Texture2D)Resources.Load("EditorContent/Welcome/Facebook");
                twitterIcon = (Texture2D)Resources.Load("EditorContent/Welcome/Twitter");

                isInited = true;

            }

            catch(Exception e) {

                Debug.Log("WELCOME SCREEN INIT: " + e);
                return false;

            }

            return true;

        }

        private static bool DrawButton(Texture2D icon, string title = "", string description = "") {

            GUILayout.BeginHorizontal();

            GUILayout.Space(34f);
            GUILayout.Box(icon, GUIStyle.none, GUILayout.MaxWidth(48f), GUILayout.MaxHeight(48f));
            GUILayout.Space(10f);

            GUILayout.BeginVertical();

            GUILayout.Space(1f);
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.Label(description);

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            GUILayout.Space(10f);

            return Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition);

        }


        private static void GetShowAtStart() {

            EditorApplication.update -= GetShowAtStart;

            isShowAtStart = EditorPrefs.GetBool(isShowAtStartEditorPrefs, true);

            if(isShowAtStart) {

                EditorApplication.update -= OpenAtStartup;
                EditorApplication.update += OpenAtStartup;

            }

        }

        private static void OpenAtStartup() {

            if(isInited && Init())  {

                OpenWindow();

                EditorApplication.update -= OpenAtStartup;

            }

        }

        [MenuItem("Tools/Dizzy Media/Welcome Screen", false)]
        public static void OpenWindow() {

            if(window == null) {

                window = GetWindow<DM_WelcomeScreen> (false, windowHeaderText, true);
                window.maxSize = window.minSize = windowsSize;

            }

        }

        private void OnEnable() {

            window = this;

        }

        private void OnDestroy() {

            window = null;

            EditorPrefs.SetBool(isShowAtStartEditorPrefs, false);

        }

    }

}//namespace