using System;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using Routine = System.Collections.IEnumerator;

namespace KreliStudio
{
    public static class CoroutineExtensions
    {
        public static Routine WithProgress(this Routine source, string title, string message, bool sticky = false)
        {
            int taskId = Progress.Start(
                name: title,
                description: message,
                options: (sticky ? Progress.Options.Sticky : Progress.Options.None) | Progress.Options.Indefinite | Progress.Options.Managed
            );

            try
            {
                Progress.Report(taskId, 0.5f);

                while (source.MoveNext())
                {
                    yield return source.Current;

                    Progress.Report(taskId, 0.5f);
                }
            }
            finally
            {
                Progress.Finish(taskId);
            }
        }

        public static Routine Then(this Routine source, Action callback)
        {
            yield return source;

            if (callback == null)
                yield break;

            try
            {
                callback();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static Routine Then(this Routine source, Func<Routine> callback)
        {
            yield return source;

            if (callback == null)
                yield break;

            var routine = default(Routine);
            try
            {
                routine = callback();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            if (routine != null)
                yield return routine;
        }

        public static EditorCoroutine StartEditorCoroutine(this Routine coroutine, object owner)
            => EditorCoroutineUtility.StartCoroutine(coroutine, owner);

        public static EditorCoroutine StartEditorCoroutine(this Routine coroutine)
            => EditorCoroutineUtility.StartCoroutineOwnerless(coroutine);

        public static void ExecuteEditorCoroutineSynchronously(this Routine coroutine, string title = null, string message = null)
        {
            try
            {
                while (coroutine.MoveNext())
                {
                    if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(message))
                        EditorUtility.DisplayProgressBar(title, message, 0.5f);

                    System.Threading.Thread.Sleep(millisecondsTimeout: 10);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
