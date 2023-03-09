 /** 
*    Class Description: 资源(ResRequest 单位)队列结构（单位）
*
*    CreateTime: 2023-03-08 13:04:49
*
*    Author : Ankh
*
*/

 using System.Collections.Generic;

 namespace kAsset 
 {
    public class RequestQueue 
    {
        private readonly List<Request> processing = new List<Request>();
        private readonly Queue<Request> queue = new Queue<Request>();
        public string key;
        public byte maxRequests { get; set; } = 10;
        public bool working => processing.Count > 0 || queue.Count > 0;

        public void Enqueue(Request request)
        {
            queue.Enqueue(request);
        }

        public bool Update()
        {
            while(queue.Count > 0 && (processing.Count < maxRequests || maxRequests == 0))
            {
                var item = queue.Dequeue();
                processing.Add(item);
                if(item.status == Request.Status.Wait) item.Start();
                if(Scheduler.Busy) return false;
            }//while_end

            for(int i = 0; i < processing.Count; i++)
            {
                var item = processing[i];
                if(item.Update()) continue;
                processing.RemoveAt(i);
                i--;
                item.Completed();
                if(Scheduler.Busy) return false;
            }
            return true;
        }
    }//class_end
 }//namespace_end
