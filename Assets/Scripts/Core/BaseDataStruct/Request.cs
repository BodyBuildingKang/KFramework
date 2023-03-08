
/** 
*    Class Description: 资源(ResRequest 单位)请求数据结构
*
*    CreateTime: 2023-03-07 14:35:33
*
*    Author : Ankh
*
*/

using System;
using System.Collections;

namespace kAsset
{
      public class Request : IEnumerator
      {
            public enum Result {
                Default,
                Success,
                Failed,
                Cancelled
            }

            public enum Status {
                Wait,
                Processing,
                Complete
            }

            public Action<Request> completed;
            public Result result { get; protected set; } = Result.Default;
            public Status status { get; protected set; } = Status.Wait;
            public bool isDone => status == Status.Complete;
            public float propress { get; set; }
            public string error { get; protected set; }
            public object Current => null;
            
            public bool MoveNext(){ return !isDone; }

            public void Reset()
            {
                completed = null;
                status = Status.Wait;
            }

            public void Start()
            {
                if (status != Status.Wait) return;
                status = Status.Processing;
                OnStart();
            }
            
            public bool Update()
            {
                if (isDone) return false;
                OnUpdated();
                return true;
            }

            public void Completed()
            {
                OnCompleted();
                var saved = completed;
                completed?.Invoke(this);
                completed -= saved;
            }

            public void SetResult(Result value, string msg = null)
            {
                propress = 1;
                result = value;
                status = result == Result.Default ? Status.Wait : Status.Complete;
                error = msg;
            }

            protected virtual void OnStart(){}
            protected virtual void OnUpdated(){}
            protected virtual void OnCompleted(){}

            public void SendRequest()
            {
                // Scheduler.Enqueue(this);
            }

            public void Cancel()
            {
                SetResult(Result.Cancelled);
            }
            
      }//class_end
}//namespace_end
