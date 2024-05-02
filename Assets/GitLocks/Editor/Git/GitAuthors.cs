#if !DISABLE_GIT_LOCKS
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KreliStudio
{
    static class GitAuthors
    {
        static readonly Dictionary<int, string> authorDictionary
            = new Dictionary<int, string>(capacity: 4096);

        internal static void AddAuthor(int instanceId, string author)
        {
            authorDictionary[instanceId] = string.Intern(author);
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        internal static string GetAuthor(Object asset, int instanceId)
        {
            if (authorDictionary.TryGetValue(instanceId, out var author))
                return author;

            // set placeholder until git command executes
            authorDictionary[instanceId] = "Loading author...";

            var assetPath = AssetDatabase.GetAssetPath(asset);

            GitCommands.GitUpdateAssetAuthor(
                assetPath: assetPath, 
                assetInstanceId: instanceId
            );

            return "Loading author...";
        }
    }
}
#endif