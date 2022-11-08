using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Owlet
{
    public class Patcher
    {
        /// <summary>
        /// 获取本地的清单文件
        /// </summary>
        /// <returns></returns>
        public static AssetManifest LoadLocalAssetManifest()
        {
            var localManifestPath = Path.Combine(Application.persistentDataPath, "asset.bin");
            if(File.Exists(localManifestPath) == false)
            {
                localManifestPath = Path.Combine(Application.streamingAssetsPath, "asset.bin");
            }
            Debug.Log(localManifestPath);

            if (File.Exists(localManifestPath) == false)
            {
                Debug.LogWarning($"Can not load local version file at:{localManifestPath}");
                return null;
            }

            var file = File.Open(localManifestPath, FileMode.Open);
            var data = new byte[file.Length];
            file.Read(data, 0, (int)file.Length);
            var manifest = new AssetManifest(data);
            file.Close();
            file.Dispose();

            return manifest;
        }

        /// <summary>
        /// 过滤新增和变更的文件
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        public static List<AssetInfo> FilterUpdateAssetInfo(AssetManifest local, AssetManifest remote)
        {
            var list = new List<AssetInfo>();

            for(int i = 0; i < remote.list.Count; i++)
            {
                var remote_info = remote.list[i];
                var isNewAsset = true;
                for(int j = 0; j < local.list.Count; j ++)
                {
                    var local_info = local.list[i];
                    if(local_info.name == remote_info.name)
                    {
                        if(remote_info.time > local_info.time &&
                            remote_info.md5 != local_info.md5)
                        {
                            isNewAsset = false;
                            list.Add(remote_info);
                        }
                        break;
                    }
                }
                if(isNewAsset)
                {
                    list.Add(remote_info);
                }
            }

            return list;
        }

        /// <summary>
        /// 对比过滤出过时的资源文件列表
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        public static List<AssetInfo> FilterUselessAssetInfo(AssetManifest local, AssetManifest remote)
        {
            var list = new List<AssetInfo>();

            for (int i = 0; i < local.list.Count; i++)
            {
                var local_info = local.list[i];
                var isUseless = false;
                for (int j = 0; j < remote.list.Count; j++)
                {
                    var remote_info = remote.list[j];

                    if(local_info.name == remote_info.name)
                    {
                        isUseless = true;
                        break;
                    }
                }

                if(isUseless)
                {
                    list.Add(local_info);
                }
            }

            return list;
        }
    }
}