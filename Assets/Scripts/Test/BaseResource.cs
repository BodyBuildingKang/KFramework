 /** 
*    Class Description: XXX
*
*    CreateTime: 2023-03-08 11:21:36
*
*    Author : Ankh
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BaseResource : IDisposable
{
    // 指向外部非托管资源
    private IntPtr handle;
    // 此类使用的其它托管资源.
    private Component Components;
    // 跟踪是否调用.Dispose方法，标识位，控制垃圾收集器的行为
    private bool disposed = false;
 
    // 构造函数
    public BaseResource()
    {
        // Insert appropriate constructor code here.
    }
 
    // 实现接口IDisposable.
    // 不能声明为虚方法virtual.
    // 子类不能重写这个方法.
    public void Dispose()
    {
        Dispose(true);
        // 离开终结队列Finalization queue 
        // 设置对象的阻止终结器代码
        GC.SuppressFinalize(this);
    }
 
    // Dispose(bool disposing) 执行分两种不同的情况.
    // 如果disposing 等于 true, 方法已经被调用
    // 或者间接被用户代码调用. 托管和非托管的代码都能被释放
    // 如果disposing 等于false, 方法已经被终结器 finalizer 从内部调用过，
    //你就不能在引用其他对象，只有非托管资源可以被释放。
    protected virtual void Dispose(bool disposing)
    {
        // 检查Dispose 是否被调用过.
        if (!this.disposed)
        {
            // 如果等于true, 释放所有托管和非托管资源 
            if (disposing)
            {
                // 释放托管资源.
                // Components.Dispose();
                this.Dispose();
            }
            // 释放非托管资源，如果disposing为 false, 
            // 只会执行下面的代码.
            CloseHandle(handle);
            handle = IntPtr.Zero;
            // 注意这里是非线程安全的.
            // 在托管资源释放以后可以启动其它线程销毁对象，
            // 但是在disposed标记设置为true前
            // 如果线程安全是必须的，客户端必须实现。
 
        }
        disposed = true;
    }
    // 使用interop 调用方法 
    // 清除非托管资源.
    [System.Runtime.InteropServices.DllImport("Kernel32")]
    private extern static Boolean CloseHandle(IntPtr handle);
 
    // 使用C# 析构函数来实现终结器代码
    // 这个只在Dispose方法没被调用的前提下，才能调用执行。
    // 如果你给基类终结的机会.
    // 不要给子类提供析构函数.
    ~BaseResource()
    {
        // 不要重复创建清理的代码.
        // 基于可靠性和可维护性考虑，调用Dispose(false) 是最佳的方式
        Dispose(false);
    }
 
    // 允许你多次调用Dispose方法,
    // 但是会抛出异常如果对象已经释放。
    // 不论你什么时间处理对象都会核查对象的是否释放, 
    // check to see if it has been disposed.
    public void DoSomething()
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(this.ToString());
        }
    }
 
 
    // 不要设置方法为virtual.
    // 继承类不允许重写这个方法
    public void Close()
    {
        // 无参数调用Dispose参数.
        Dispose();
    }
 
    public static void Main()
    {
        // Insert code here to create
        // and use a BaseResource object.
    }
}
