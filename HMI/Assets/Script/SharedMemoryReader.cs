using System;
using System.IO.MemoryMappedFiles;
using System.Threading;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using BasicClass;


public class SharedMemoryReader : MonoBehaviour
{
    private MemoryMappedFile sharedMemory;
    private MemoryMappedViewAccessor accessor;
    private Mutex mutex;

    public UnityEvent<SharedMemoryReader> OnSpeedChanged;

    private bool isRunning = true;
    public SharedData Data { get; private set; }

    [HideInInspector]
    public int currentSpeed;

    void Start()
    {
        try
        {
            sharedMemory = MemoryMappedFile.OpenExisting("MySharedMemory");
            accessor = sharedMemory.CreateViewAccessor();
            mutex = Mutex.OpenExisting("MySharedMemoryMutex");
        }
        catch (Exception ex)
        {
            Debug.LogError("SharedMemoryReader failed to initialize: " + ex.Message);
            isRunning = false;
        }

        if (isRunning)
        {
            StartCoroutine(ReadSharedMemory());
        }
    }

    IEnumerator ReadSharedMemory()
    {
        while (isRunning)
        {
            if (mutex.WaitOne(10))
            {
                try
                {
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(SharedData))];
                    accessor.ReadArray(0, buffer, 0, buffer.Length);
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    Data = (SharedData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(SharedData));
                    handle.Free();

                    
                    currentSpeed = (int)Data.speed;
                    foreach (var coord in Data.car_coordinates)
                    {
                        Debug.Log($"Car Coordinate: X={coord.X}, Y={coord.Y}");
                    }

                    OnSpeedChanged?.Invoke(this); // 触发速度变化事件
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return null;
            }
        }
    }

    void OnDestroy()
    {
        isRunning = false;
        accessor?.Dispose();
        sharedMemory?.Dispose();
        mutex?.Close();
    }
}