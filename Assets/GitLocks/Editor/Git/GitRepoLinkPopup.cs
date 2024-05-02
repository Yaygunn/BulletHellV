#if !DISABLE_GIT_LOCKS
using UnityEngine;
using UnityEditor;

namespace KreliStudio 
{
    public class GitRepoLinkPopup : PopupWindowContent
    {
        const string ExampleRepoLink = "https://github.com/GitUser/RepositoryName/commits/main/";

        public override Vector2 GetWindowSize()
        {
            return new Vector2(500, 50);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Link To Repository's History", EditorStyles.boldLabel);
            var currentLink = GitCommands.RepositoryURL;

            if (string.IsNullOrWhiteSpace(currentLink))
                currentLink = ExampleRepoLink;

            currentLink = EditorGUILayout.DelayedTextField(GUIContent.none, currentLink);

            GitCommands.RepositoryURL = currentLink;
        }
    } 
}
#endif