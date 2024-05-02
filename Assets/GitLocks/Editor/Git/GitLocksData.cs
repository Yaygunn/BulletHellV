#if !DISABLE_GIT_LOCKS
using System;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEditor;
using Object = UnityEngine.Object;

namespace KreliStudio
{
    public struct GitLocksData
    {
        [JsonProperty("ours")]
        public GitLocksDataItem[] Ours;

        [JsonProperty("theirs")]
        public GitLocksDataItem[] Theirs;

        public GitLocksDataItem[] All;

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            for (int i = 0; i < Ours.Length; ++i)
            {
                ref var item = ref Ours[i];
                item.isLocalOwner = true;
            }

            for (int i = 0; i < Theirs.Length; ++i)
            {
                ref var item = ref Theirs[i];
                item.isLocalOwner = false;
            }


            All = Ours.Concat(Theirs).ToArray();
        }
    }

    [Serializable]
    public struct GitLocksDataItem
    {

        [JsonProperty("id")]
        public long ID;

        public GitOwnerData owner;

        public bool isLocalOwner;

        [JsonProperty("locked_at")]
        public DateTimeOffset lockedAt;

        public string path;

        public Object Object => AssetDatabase.LoadAssetAtPath<Object>(path);

        public string FileName => System.IO.Path.GetFileName(path);

    }

    [Serializable]
    public struct GitOwnerData
    {
        public string name;
    }
}
#endif