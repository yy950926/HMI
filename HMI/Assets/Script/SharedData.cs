using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;




namespace BasicClass{
[StructLayout(LayoutKind.Sequential)]
public struct SharedData
{
    public double speed;
    public double rpm;
    public double vehicleYaw;
    public double vehiclePitch;
    public double vehicleRoll;
    [MarshalAs(UnmanagedType.I1)]
    public bool links_Blinker;

    [MarshalAs(UnmanagedType.I1)]
    public bool rechts_Blinker;

    [MarshalAs(UnmanagedType.I1)]
    public bool horn;
    public int roadSpeed;
    public double vehicleWidth;
    public double vehicleLength;
    public double laneWidth;
    public double laneGapleft;
    public double laneGapRight;
    public double pitchRoad;
    public double rollRoad;
    public double visibleRoadLength;
    public int abs;
    public double consumption;
    public double fuel_used;
    
   [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public CarCoordinate[] car_coordinates;
}
}