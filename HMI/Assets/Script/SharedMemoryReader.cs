using System;
using System.IO.MemoryMappedFiles;
using System.Threading;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Runtime.InteropServices;


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct SharedData
{
    public double speed;
    public double rpm;
    
}
public class SharedMemoryReader : MonoBehaviour
{
    private MemoryMappedFile sharedMemory;
    private MemoryMappedViewAccessor accessor;
    private Mutex mutex;

    public UnityEvent<SharedMemoryReader> OnSpeedChanged;

    private bool isRunning = true;

    public SharedData Data { get; private set; }


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
                    // Create a handle for the shared memory
                    IntPtr ptr = accessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
                    // Marshal the data from the shared memory
                    Data = (SharedData)Marshal.PtrToStructure(ptr, typeof(SharedData));
                    currentSpeed =  (int)Data.speed;
                    //Debug.Log( currentSpeed ); 
                    OnSpeedChanged?.Invoke(this); // trigger speed changed event
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