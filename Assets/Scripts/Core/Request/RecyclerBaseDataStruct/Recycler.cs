 /** 
*    Class Description: 资源回收
*
*    CreateTime: 2023-03-09 09:52:37
*
*    Author : Ankh
*
*/

 using System;
 using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
 using Object = UnityEngine.Object;

 namespace kAsset
 {
     public interface IRecyclable
     {
         void EndRecycle();
         bool CanRecycle();
         void RecycleAsync();
         bool Recycling();
     }
     
     public class Recycler : MonoBehaviour
     {
         private static readonly List<IRecyclable> _listRecyclables = new List<IRecyclable>();
         private static readonly List<IRecyclable> _listRecycling = new List<IRecyclable>();
         private static readonly Queue<Object> _queueUnusedAssets = new Queue<Object>();


         public static void AddRecycleBySch(IRecyclable recyclable)
         {
             if(_listRecyclables.Contains(recyclable))
             {
                 return;
             }
             _listRecyclables.Add(recyclable);
         }

         public static void CancelRecycle(IRecyclable recyclable)
         {
             if(_listRecyclables.Contains(recyclable))
             {
                 _listRecyclables.Remove(recyclable);
             }
             if(_listRecycling.Contains(recyclable))
             {
                 _listRecycling.Remove(recyclable);
             }
         }

         public static void UnloadAsset(Object asset)
         {
             if(!_queueUnusedAssets.Contains(asset))
                 _queueUnusedAssets.Enqueue(asset);
         }

         private void Update()
         {

             #region 卸载指定资源，并卸载一次没有使用的资源
             if(_queueUnusedAssets.Count > 0)
             {
                 while(_queueUnusedAssets.Count > 0)
                 {
                     var item = _queueUnusedAssets.Dequeue();
                     Resources.UnloadAsset(item);
                 }
                 Resources.UnloadUnusedAssets();
             }
             #endregion
            
             if(Scheduler.Working) return; // 正在加载时不卸载资源

             #region 资源回收
             for(int i = 0; i < _listRecyclables.Count; i++)
             {
                 var request = _listRecyclables[i];
                 if(!request.CanRecycle()) continue;
                 _listRecyclables.RemoveAt(i);
                 i--;
                 request.RecycleAsync();
                 _listRecycling.Add(request);
             }

             for(int i = 0; i < _listRecycling.Count; i++)
             {
                 var request = _listRecycling[i];
                 if(request.Recycling()) continue;
                 _listRecycling.RemoveAt(i);
                 i--;
                 if(request.CanRecycle()) request.EndRecycle();
                 if(Scheduler.Busy) return;
             }
             #endregion

         }

     }//class_end
 }//namespace_end