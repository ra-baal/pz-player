using System;
using System.Runtime.InteropServices;

/// Imports functions from dll.
public static class KinectPclLib
{
    /// Name of dll file to import (cpp plugin pcl).
    private const string _dll = "KinectPCLLib";

    [DllImport(_dll)]
    public static extern bool readPointCloud(ref IntPtr ptrResultVertsX,
                                              ref IntPtr ptrResultVertsY,
                                              ref IntPtr ptrResultVertsZ,
                                              ref IntPtr ptrResultColorR,
                                              ref IntPtr ptrResultColorG,
                                              ref IntPtr ptrResultColorB,
                                              ref int resultVertLength);

    [DllImport(_dll)]
    public static extern bool extractClusters();

    [DllImport(_dll)]
    public static extern bool readCloud(string filename);

    [DllImport(_dll)]
    public static extern bool readKinectCloud();

    [DllImport(_dll)]
    public static extern bool removeBiggestPlane(int maxIterations,
                                                  double distanceThreshold);

    [DllImport(_dll)]
    public static extern bool getClusters(double clusterTolerance,
                                           int minClusterSize,
                                           int maxClusterSize);

    [DllImport(_dll)]
    public static extern int getCloudSize();

    [DllImport(_dll)]
    public static extern int getClustersCount();

    [DllImport(_dll)]
    public static extern bool getCluster(int clusterIndex,
                                          ref IntPtr ptrResultVertsX,
                                          ref IntPtr ptrResultVertsY,
                                          ref IntPtr ptrResultVertsZ,
                                          ref IntPtr ptrResultColorR,
                                          ref IntPtr ptrResultColorG,
                                          ref IntPtr ptrResultColorB,
                                          ref int resultVertLength);

    [DllImport(_dll)]
    public static extern bool getClusterIndices(int clusterIndex,
                                                 ref IntPtr indices,
                                                 ref int indicesLength);

    [DllImport(_dll)]
    public static extern bool getCloud(ref IntPtr ptrResultVertsX,
                                        ref IntPtr ptrResultVertsY,
                                        ref IntPtr ptrResultVertsZ,
                                        ref IntPtr ptrResultColorR,
                                        ref IntPtr ptrResultColorG,
                                        ref IntPtr ptrResultColorB,
                                        ref int resultVertLength);

    [DllImport(_dll)]
    public static extern void freePointers(IntPtr ptrResultVertsX,
                                            IntPtr ptrResultVertsY,
                                            IntPtr ptrResultVertsZ,
                                            IntPtr ptrResultColorR,
                                            IntPtr ptrResultColorG,
                                            IntPtr ptrResultColorB);

    [DllImport(_dll)]
    public static extern void freeClusterIndices(IntPtr ptrIndices);

    [DllImport(_dll)]
    public static extern bool structureTest(ref IntPtr myCloudStructure);

}
