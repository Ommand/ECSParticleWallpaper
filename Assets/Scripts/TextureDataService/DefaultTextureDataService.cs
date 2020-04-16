using System;
using System.Collections;
using System.IO;
using UnityComponents;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace TextureDataService
{
    public class DefaultTextureDataService : ITextureDataService
    {
        public void Add(string id, string path)
        {
            File.Copy(path, GetFileName(id), true);
        }

        private static string GetFileName(string id)
        {
            return id.StartsWith(DefaultTextures.DefaultPrefix)
                ? Path.Combine(Application.streamingAssetsPath, id)
                : Path.Combine(Application.persistentDataPath, id);
        }

        public void LoadTexture(string id, Action<Texture2D> onComplete)
        {
            Globals.CoroutinProcessor.StartCoroutine(LoadImageCoroutine(id, onComplete));
        }

        private static IEnumerator LoadImageCoroutine(string id, Action<Texture2D> onComplete)
        {
            using (var uwr = UnityWebRequestTexture.GetTexture(GetFileName(id)))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                    onComplete?.Invoke(null);
                else
                {
                    Debug.Log($"Image [{id}] loaded successfully");
                    var texture2D = DownloadHandlerTexture.GetContent(uwr);
                    texture2D.name = id;
                    onComplete?.Invoke(texture2D);
                }
            }
        }
    }
}