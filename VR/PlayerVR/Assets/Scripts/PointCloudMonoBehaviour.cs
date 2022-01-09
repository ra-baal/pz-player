using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.VersionControl;

public class PointCloudMonoBehaviour : MonoBehaviour
{

    class Cloud
    {
        public Vector3[] points;
        public Color[] colors;
        public int length;

        // int cloudTag;

        public Cloud(int newLength)
        {
            length = newLength;
            points = new Vector3[length];
            colors = new Color[length];
        }
    }

    class Clouds
    {
        public Cloud[] clouds;
        public int length;

        public Clouds(int newLength)
        {
            length = newLength;
            clouds = new Cloud[length];
        }
    }

    // Struct test begin
    public struct MyCloud
    {
        public Int32 Size;
        public IntPtr pointsX, pointsY, pointsZ;
    }

    public static void CallFunction()
    {
        MyCloud managedObj;
        managedObj.pointsX = IntPtr.Zero;
        managedObj.pointsY = IntPtr.Zero;
        managedObj.pointsZ = IntPtr.Zero;
        managedObj.Size = 0;

        IntPtr unmanagedAddr = Marshal.AllocHGlobal(Marshal.SizeOf(managedObj));
        Marshal.StructureToPtr(managedObj, unmanagedAddr, true);

        KinectPclLib.structureTest(ref unmanagedAddr);

        Marshal.PtrToStructure(unmanagedAddr, managedObj);

        Debug.Log("Structure = " + managedObj);

        Marshal.FreeHGlobal(unmanagedAddr);
        unmanagedAddr = IntPtr.Zero;
    }

    // Struct test end 

    void drawMyCloud()
    {
        IntPtr ptrResultVertsX = IntPtr.Zero;
        IntPtr ptrResultVertsY = IntPtr.Zero;
        IntPtr ptrResultVertsZ = IntPtr.Zero;
        IntPtr ptrResultColorR = IntPtr.Zero;
        IntPtr ptrResultColorG = IntPtr.Zero;
        IntPtr ptrResultColorB = IntPtr.Zero;

        int resultVertLength = 0;

        bool success = KinectPclLib.getCloud(ref ptrResultVertsX, ref ptrResultVertsY, ref ptrResultVertsZ,
            ref ptrResultColorR, ref ptrResultColorG, ref ptrResultColorB, ref resultVertLength);
        if (success)
        {
            // Load the results into a managed array.
            float[] resultVerticesX = new float[resultVertLength];
            float[] resultVerticesY = new float[resultVertLength];
            float[] resultVerticesZ = new float[resultVertLength];
            int[] resultColorsR = new int[resultVertLength];
            int[] resultColorsG = new int[resultVertLength];
            int[] resultColorsB = new int[resultVertLength];

            Marshal.Copy(
                ptrResultVertsX,
                resultVerticesX,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultVertsY,
                resultVerticesY,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultVertsZ,
                resultVerticesZ,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultColorR,
                resultColorsR,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultColorG,
                resultColorsG,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultColorB,
                resultColorsB,
                0,
                resultVertLength
            );

            // should probably pass colors to cube as well
            for (int i = 0; i < resultVertLength; i++)
            {
                createCube(resultVerticesX[i], resultVerticesY[i], resultVerticesZ[i], new Color(resultColorsR[i] / 255F, resultColorsG[i] / 255F, resultColorsB[i] / 255F));
            }

            KinectPclLib.freePointers(ptrResultVertsX, ptrResultVertsY, ptrResultVertsZ,
                ptrResultColorR, ptrResultColorG, ptrResultColorB);
            Debug.Log("Memory deallocation succesful");

            return;
        }
        else
        {
            Debug.Log("Ended not sucessfully.");
            return;
        }
    }

    Cloud readMyCloud()
    {
        IntPtr ptrResultVertsX = IntPtr.Zero;
        IntPtr ptrResultVertsY = IntPtr.Zero;
        IntPtr ptrResultVertsZ = IntPtr.Zero;
        IntPtr ptrResultColorR = IntPtr.Zero;
        IntPtr ptrResultColorG = IntPtr.Zero;
        IntPtr ptrResultColorB = IntPtr.Zero;

        int resultVertLength = 0;

        bool success = KinectPclLib.readPointCloud(ref ptrResultVertsX, ref ptrResultVertsY, ref ptrResultVertsZ,
            ref ptrResultColorR, ref ptrResultColorG, ref ptrResultColorB, ref resultVertLength);
        if (success)
        {
            // Load the results into a managed array.
            float[] resultVerticesX = new float[resultVertLength];
            float[] resultVerticesY = new float[resultVertLength];
            float[] resultVerticesZ = new float[resultVertLength];
            int[] resultColorsR = new int[resultVertLength];
            int[] resultColorsG = new int[resultVertLength];
            int[] resultColorsB = new int[resultVertLength];

            Marshal.Copy(
                ptrResultVertsX,
                resultVerticesX,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultVertsY,
                resultVerticesY,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultVertsZ,
                resultVerticesZ,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultColorR,
                resultColorsR,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultColorG,
                resultColorsG,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultColorB,
                resultColorsB,
                0,
                resultVertLength
            );

            Cloud result = new Cloud(resultVertLength);

            for (int i = 0; i < resultVertLength; i++)
            {
                result.points[i] = new Vector3(resultVerticesX[i], resultVerticesY[i], resultVerticesZ[i]);
                result.colors[i] = new Color(resultColorsR[i] / 255F, resultColorsG[i] / 255F, resultColorsB[i] / 255F);
            }

            KinectPclLib.freePointers(ptrResultVertsX, ptrResultVertsY, ptrResultVertsZ,
                ptrResultColorR, ptrResultColorG, ptrResultColorB);
            Debug.Log("Memory deallocation succesful");

            return result;
        }
        else
        {
            Debug.Log("Ended not sucessfully.");
            return null;
        }
    }

    Clouds readCloudAndExtractClusters(Cloud cloud)
    {
        return null;
        // TODO
    }

    Vector3[] getPointData()
    {
        IntPtr ptrResultVertsX = IntPtr.Zero;
        IntPtr ptrResultVertsY = IntPtr.Zero;
        IntPtr ptrResultVertsZ = IntPtr.Zero;
        IntPtr ptrResultColorR = IntPtr.Zero;
        IntPtr ptrResultColorG = IntPtr.Zero;
        IntPtr ptrResultColorB = IntPtr.Zero;

        int resultVertLength = 0;

        // bool success = getPseudoPointCloud (ref ptrResultVertsX, ref ptrResultVertsY, ref ptrResultVertsZ, ref resultVertLength);
        bool success = KinectPclLib.readPointCloud(ref ptrResultVertsX, ref ptrResultVertsY, ref ptrResultVertsZ,
            ref ptrResultColorR, ref ptrResultColorG, ref ptrResultColorB, ref resultVertLength);

        if (success)
        {
            // Load the results into a managed array.
            float[] resultVerticesX = new float[resultVertLength];
            float[] resultVerticesY = new float[resultVertLength];
            float[] resultVerticesZ = new float[resultVertLength];
            int[] resultColorsR = new int[resultVertLength];
            int[] resultColorsG = new int[resultVertLength];
            int[] resultColorsB = new int[resultVertLength];

            Marshal.Copy(
                ptrResultVertsX,
                resultVerticesX,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultVertsY,
                resultVerticesY,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultVertsZ,
                resultVerticesZ,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultColorR,
                resultColorsR,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultColorG,
                resultColorsG,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultColorB,
                resultColorsB,
                0,
                resultVertLength
            );

            Vector3[] pseudoCloud = new Vector3[resultVertLength];

            for (int i = 0; i < resultVertLength; i++)
            {
                pseudoCloud[i] = new Vector3(resultVerticesX[i], resultVerticesY[i], resultVerticesZ[i]);
                createCube(resultVerticesX[i], resultVerticesY[i], resultVerticesZ[i], new Color(resultColorsR[i] / 255F, resultColorsG[i] / 255F, resultColorsB[i] / 255F));
            }

            KinectPclLib.freePointers(ptrResultVertsX, ptrResultVertsY, ptrResultVertsZ,
                ptrResultColorR, ptrResultColorG, ptrResultColorB);
            Debug.Log("Memory deallocation succesful");

            return pseudoCloud;
        }
        else
        {
            Debug.Log("Ended not sucessfully.");
            return null;
        }
    }

    void createCube(float x, float y, float z, Color color)
    {
        createCube(x, y, z, color, 0);
    }

    public Transform point;
    public bool usePrefabs = true;
    public float pointScale = 0.005F;

    void createCube(float x, float y, float z, Color color, int tag)
    {
        if (usePrefabs)
        {
            Transform t = ((Transform)Instantiate(point, new Vector3(x, y, z), Quaternion.identity));
            //Transform t = ((Transform)Instantiate(point, new Vector3(x, y, z), Quaternion.identity, this.transform));
            t.GetComponent<Renderer>().material.color = color;
            t.name = "point_in_cloud_" + tag;

            t.parent = frame.transform;
        }
        else
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(x, y, z);
            cube.transform.parent = frame.transform;

            cube.name = "point_in_cloud_" + tag;
            cube.GetComponent<Renderer>().material.color = color;

            float scale = pointScale;
            cube.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    void drawCluster(int clusterIndex)
    {
        IntPtr ptrResultVertsX = IntPtr.Zero;
        IntPtr ptrResultVertsY = IntPtr.Zero;
        IntPtr ptrResultVertsZ = IntPtr.Zero;
        IntPtr ptrResultColorR = IntPtr.Zero;
        IntPtr ptrResultColorG = IntPtr.Zero;
        IntPtr ptrResultColorB = IntPtr.Zero;

        int resultVertLength = 0;

        bool success = KinectPclLib.getCluster(clusterIndex, ref ptrResultVertsX, ref ptrResultVertsY, ref ptrResultVertsZ,
            ref ptrResultColorR, ref ptrResultColorG, ref ptrResultColorB, ref resultVertLength);
        if (success)
        {
            // Debug.Log ("ResultVertLenght = " + resultVertLength);

            // Load the results into a managed array.
            float[] resultVerticesX = new float[resultVertLength];
            float[] resultVerticesY = new float[resultVertLength];
            float[] resultVerticesZ = new float[resultVertLength];
            int[] resultColorsR = new int[resultVertLength];
            int[] resultColorsG = new int[resultVertLength];
            int[] resultColorsB = new int[resultVertLength];

            Marshal.Copy(
                ptrResultVertsX,
                resultVerticesX,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultVertsY,
                resultVerticesY,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultVertsZ,
                resultVerticesZ,
                0,
                resultVertLength
                );

            Marshal.Copy(
                ptrResultColorR,
                resultColorsR,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultColorG,
                resultColorsG,
                0,
                resultVertLength
            );

            Marshal.Copy(
                ptrResultColorB,
                resultColorsB,
                0,
                resultVertLength
            );

            int tag = clusterIndex;

            // TODO create unique tag and add it into tags
            for (int i = 0; i < resultVertLength; i++)
            {
                Color color = new Color(resultColorsR[i] / 255F, resultColorsG[i] / 255F, resultColorsB[i] / 255F, 1f);
                createCube(resultVerticesX[i], resultVerticesY[i], resultVerticesZ[i], color, tag);
            }

            resultVerticesX = null;
            resultVerticesY = null;
            resultVerticesZ = null;
            resultColorsR = null;
            resultColorsG = null;
            resultColorsB = null;

            KinectPclLib.freePointers(ptrResultVertsX, ptrResultVertsY, ptrResultVertsZ,
                ptrResultColorR, ptrResultColorG, ptrResultColorB);
            Debug.Log("Memory deallocation succesful");

        }
        else
        {
            Debug.Log("Ended not sucessfully.");
        }

        ptrResultVertsX = IntPtr.Zero;
        ptrResultVertsY = IntPtr.Zero;
        ptrResultVertsZ = IntPtr.Zero;
        ptrResultColorR = IntPtr.Zero;
        ptrResultColorG = IntPtr.Zero;
        ptrResultColorB = IntPtr.Zero;

        return;
    }

    void debugIndices(int clusterIndex)
    {
        IntPtr ptrIndices = IntPtr.Zero;
        int resultIndicesLength = 0;

        bool success = KinectPclLib.getClusterIndices(clusterIndex, ref ptrIndices, ref resultIndicesLength);
        if (success)
        {
            // Debug.Log ("ResultVertLenght = " + resultVertLength);

            // Load the results into a managed array.
            float[] resultIndices = new float[resultIndicesLength];

            Marshal.Copy(
                ptrIndices,
                resultIndices,
                0,
                resultIndicesLength
            );

            String indicesStr = "Indices Debug : ";

            for (int i = 0; i < resultIndicesLength; i++)
            {
                indicesStr += " : " + resultIndices[i];
            }

            // Debug.Log(indicesStr + ".");

            resultIndices = null;

            KinectPclLib.freeClusterIndices(ptrIndices);
            Debug.Log("Memory deallocated successfully");

        }
        else
        {
            Debug.Log("Ended not sucessfully.");
        }

        ptrIndices = IntPtr.Zero;

        return;
    }

    void refreshKinect()
    {
        Debug.Log("Refreshing kinect ..");

        // Clean pointcloud
        KinectPclLib.readKinectCloud();

        /*
		// Clean scene (delete old cubes)
		// Move whole cloud to left
		GameObject[] myPoints  = (GameObject[])FindObjectsOfType (typeof(GameObject));
		for (int i=0; i < myPoints.Length; i++) {

			if (myPoints[i].name.StartsWith("point_in_cloud")) {
				Destroy (myPoints[i]);
			}
		}
		
		getCloudSize ();
		removeBiggestPlane ();
		getClusters ();
		getClustersCount ();
		
		for (int i = 0; i < getClustersCount(); i++) {
			drawCluster (i);
		}
		*/
    }

    float totaltime;
    string printTimeDelta()
    {
        float ms = Time.realtimeSinceStartup - totaltime;
        totaltime = Time.realtimeSinceStartup;
        return " in " + ms + " s.";
    }

    public bool useKinect;
    public bool kinectRealtime;
    public bool debug_indices;
    public bool drawSegmentedCloud = true;

    // removeBiggestPlane parameters
    public int maxIterations = 100;
    public double distanceThreshold = 0.02;

    // getClusters parameters
    public double clusterTolerance = 0.02;
    public int minClusterSize = 100;
    public int maxClusterSize = 25000;


    public string filename;

    [Header("Parent object")]
    [SerializeField] private Transform frame;
    public bool cloudReady = false;

    public void ShowCloud()
    {
        totaltime = Time.realtimeSinceStartup;

        if (useKinect)
        {
            Debug.Log("readKinectCloud() : " + KinectPclLib.readKinectCloud() + printTimeDelta());
        }
        else
        {
            Debug.Log("readCloud() : " + KinectPclLib.readCloud(filename) + printTimeDelta());
        }

        if (drawSegmentedCloud)
        {
            Debug.Log("getCloudSize() : " + KinectPclLib.getCloudSize() + printTimeDelta());
            Debug.Log("removeBiggestPlane() : " + KinectPclLib.removeBiggestPlane(maxIterations, distanceThreshold) + printTimeDelta());
            Debug.Log("getClusters() : " + KinectPclLib.getClusters(clusterTolerance, minClusterSize, maxClusterSize) + printTimeDelta());
            Debug.Log("getClustersCount() : " + KinectPclLib.getClustersCount() + printTimeDelta());


            for (int i = 0; i < KinectPclLib.getClustersCount(); i++)
            {
                drawCluster(i);

                if (debug_indices)
                {
                    debugIndices(i);
                }
            }
            cloudReady = true;
            Debug.Log("Clusters drawn" + printTimeDelta());

        }
        else
        {
            // Just for testing... dont use that
            drawMyCloud();
        }

        totaltime = Time.realtimeSinceStartup;
    }

    private void OnDisable()
    {
        cloudReady = false;
    }

    int selectedTag = -1;
    float keyboardMoveSensitivity = 100f;


    void Update()
    {
        // Debug.Log ("Time = " + (Time.realtimeSinceStartup - totaltime));
        if (useKinect && kinectRealtime)
        {
            if ((Time.realtimeSinceStartup - totaltime) > 5f)
            {
                Debug.Log("refresh now?");
                totaltime = Time.realtimeSinceStartup;

                refreshKinect();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {

            GameObject clickedGmObj = GetClickedGameObject();

            if (clickedGmObj != null)
            {
                String objectName = clickedGmObj.name;
                int underline_index = objectName.LastIndexOf("_");
                if (underline_index >= 0)
                {
                    String tagName = objectName.Substring(underline_index + 1);

                    selectedTag = Convert.ToInt32(tagName);
                    Debug.Log("Selecting tag " + selectedTag);

                }
                else
                {
                    Debug.Log("Name of clicked object has wrong format");
                    selectedTag = -1;
                }
            }
            else
            {
                selectedTag = -1;
            }
        }

        if (Input.GetKey("a"))
        {
            if (selectedTag >= 0)
            {
                // Move whole cloud to left
                GameObject[] myPoints = (GameObject[])FindObjectsOfType(typeof(GameObject));
                Debug.Log("myPoints length = " + myPoints.Length);
                for (int i = 0; i < myPoints.Length; i++)
                {

                    if (myPoints[i].name.Equals("point_in_cloud_" + selectedTag))
                    {
                        myPoints[i].transform.Translate(new Vector3(1 / keyboardMoveSensitivity, 0, 0));
                    }

                }

                Debug.Log("Cloud segment moved!");
            }
        }
    }

    public LayerMask layerMask;

    GameObject GetClickedGameObject()
    {
        // Builds a ray from camera point of view to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            return hit.transform.gameObject;
        else
            return null;
    }
}
