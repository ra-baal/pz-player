using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Parent objects")]
    [SerializeField] private List<GameObject> frames;
    private GameObject currentFrame;
    private PointCloudMonoBehaviour pluginPCL;

    private int frameIterator;
    private int frameCount;

    private bool canAnimate;

    [SerializeField] private string[] fileNames;
    private int fileCount;
    private int fileIterator;

    void Start()
    {
        //get a list of all the filenames from another file or something c:

        if (frames.Count != 0 && fileNames.Length != 0)
        {
            frameCount = frames.Count;
            frameIterator = 0;

            currentFrame = frames[0];
            pluginPCL = frames[0].GetComponent<PointCloudMonoBehaviour>();
            pluginPCL.ShowCloud();
            currentFrame.SetActive(true);

            fileCount = fileNames.Length;

            //sets the filenames for first (frameCount) frames
            for (int i = 1; i < frameCount; i++)
            {
                frames[i].GetComponent<PointCloudMonoBehaviour>().filename = fileNames[i];
                //we read the clouds beforehand to allow bigger framerates
                frames[i].GetComponent<PointCloudMonoBehaviour>().ShowCloud();
            }
            fileIterator = frameCount;

            canAnimate = true;
            //StartCoroutine("FixedFrameUpdate");
        }
        else
        {
            Debug.LogError("No frames added to the list!");
            canAnimate = false;
        }

        if (pluginPCL.useKinect)
        {
            Debug.Log("This script is used only for non kinect animation!");
            canAnimate = false;
        }
    }

    private void DisableCurrentFrame()
    {
        currentFrame.SetActive(false);
        //frame has to be reseted to 0 rotation (rotation will not work on children if we just set it once in the inspector)
        currentFrame.transform.Rotate(0, 0, 180, Space.Self);
        //destroy all points from this frame
        foreach (Transform child in currentFrame.transform)
        {
            Destroy(child.gameObject);
        }
        //load next cloud that will be held on this frame
        pluginPCL.ShowCloud();
    }

    private void SetupNewCurrentFrame()
    {
        frameIterator++;
        if (frameIterator == frameCount) frameIterator = 0;

        //setup next frame
        currentFrame = frames[frameIterator];
        pluginPCL = currentFrame.GetComponent<PointCloudMonoBehaviour>();
        currentFrame.SetActive(true);
    }

    public void UpdateFrames()
    {
        if (canAnimate)
        {
            fileIterator++;
            if (fileIterator < fileCount)
            {
                pluginPCL.filename = fileNames[fileIterator];
            }
            else if (fileIterator == fileCount + frameCount)
            {
                canAnimate = false;
                return;
            }

            DisableCurrentFrame();
            SetupNewCurrentFrame();

            if (pluginPCL.cloudReady)
            {
                currentFrame.transform.Rotate(0, 0, 180, Space.Self);
                pluginPCL.cloudReady = false;
            }
        }
    }

    //to zostaje na poxniejsze testowanie czy uda nam sie przewija� klatkki szybciej
    IEnumerator FixedFrameUpdate()
    {
        while (true)
        {
            for (int i = 0; i < frameCount; i++)
            {
                Debug.Log("file number: " + fileIterator);
                UpdateFrames();
                //Frames will be updated frameCount Times
            }
            yield return null; // continue next frame (Same as new WaitForSeconds(0) but more efficient)
        }
    }

    private void Update()
    {
        /*if(!canAnimate)
        {
            StopCoroutine("FixedFrameUpdate");
        }*/
        UpdateFrames();
    }
}