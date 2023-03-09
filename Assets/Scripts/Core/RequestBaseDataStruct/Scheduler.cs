/** 
*    Class Description: 资源请求调度处理
*
*    CreateTime: 2023-03-08 14:18:53
*
*    Author : Ankh
*
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace kAsset
{
    public class Scheduler : MonoBehaviour
    {
        private static readonly Dictionary<string, RequestQueue> _dictionarys = new Dictionary<string, RequestQueue>();
        private static readonly List<RequestQueue> _lists = new List<RequestQueue>();
        private static readonly Queue<RequestQueue> _queues = new Queue<RequestQueue>();
        private static float _realTimeSinceStartUp;

        public static bool Working => _lists.Exists(o => o.working);
        public static bool Busy => AutoSlicing && Time.realtimeSinceStartup - _realTimeSinceStartUp > MaxUpdateTimeSlice;

        [SerializeField] [Tooltip("是否开启自动切片")] private bool autoSlicing = true;
        public static bool AutoSlicing { get; set; } = true;

        [SerializeField] [Tooltip("自动切片时间,值越大处理的请求越多,值越小处理请求数量越小,可以根据目标帧率分配")]
        private float maxUpdateTimeSlice = 1 / 60f;
        public static float MaxUpdateTimeSlice { get; set; }

        [SerializeField] [Tooltip("每个队列最大单帧更新数量")]
        private byte maxRequest = 10;
        public static byte MaxRequests { get; set; } = 10;

        private void Start()
        {
            AutoSlicing = autoSlicing;
            MaxUpdateTimeSlice = maxUpdateTimeSlice;
            MaxRequests = maxRequest;
        }

        private void Update()
        {
            _realTimeSinceStartUp = Time.realtimeSinceStartup;
            while(_queues.Count > 0)
            {
                var item = _queues.Dequeue();
                _lists.Add(item);
            }

            for(int i = 0; i < _lists.Count; i++)
            {
                if(!_lists[i].Update())
                    break;
            }
        }//fun_end

        public static void Enqueue(Request request)
        {
            var key = request.GetType().Name;
            if(!_dictionarys.TryGetValue(key, out var requestqueueQueue))
            {
                requestqueueQueue = new RequestQueue() { key = key, maxRequests = MaxRequests };
                _dictionarys.Add(key,requestqueueQueue);
                _queues.Enqueue(requestqueueQueue);
                // TODO: 这里可以考虑给 Request 加个优先级。 可以用权重(符合项目的策划加，比如枚举，数字等)来标记优先级
            }
            requestqueueQueue.Enqueue(request);
        }

    }//class_end
}//namespace_end
