 /** 
*    Class Description: 循环列表
*
*    CreateTime: 2023-03-09 11:16:21
*
*    Author : Ankh
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class ListCycle<T> : IList<T>
 {

     int curIndex = -1;
     List<T> list;
     int nMax;

     public ListCycle(int n)
     {
         list = new List<T>(n);
         nMax = n;
     }

     public int CurIndex => curIndex;
     public int IndexOf(T item) => list.IndexOf(item);
     public bool Contains(T item) => list.Contains(item);
     public int Count => list.Count;
     public bool IsReadOnly => false;
     public IEnumerator<T> GetEnumerator() { return list.GetEnumerator(); }
     IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }

     public T this[int index]
     {
         get { return list[index]; }
         set { list[index] = value; }
     }

     public void Add(T item)
     {
         curIndex++; if (curIndex >= nMax) curIndex = 0;
         if (curIndex < list.Count)
             list[curIndex] = item;
         else
             list.Add(item);
     }

     public void Clear()
     {
         list.Clear();
         curIndex = -1;
     }
     
     // todo list 其他方法
     public void Insert(int index, T item) {  }
     public bool Remove(T item) { return false;}
     public void RemoveAt(int index) {  }
     public void CopyTo(T[] array, int arrayIndex) {  }

 }