using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimationControllerPLY : MonoBehaviour
{
    public MeshFilter meshFilter;
    public FileReader fileReader;
    public Animator animator;

    public Mesh[] meshes;
    private int meshIterator;
    private int meshArraySize = 3;
    private Mesh currentMesh;

    private int fileIterator;
    bool readFinised;
    bool isPaused = false;

    void Start()
    {
        readFinised = false;
        meshes = new Mesh[meshArraySize];
        meshIterator = 0;
        fileIterator = 0;
        fileReader = FindObjectOfType<FileReader>();
        animator = GetComponent<Animator>();
        if (fileReader != null)
        {
            ReadFile();
            currentMesh = meshes[0];
        }
    }

    public void ReadFile()
    {
        for(int it = 0; it < fileReader.fileCount; it++)
        {
            string fileName = fileReader.fileNames[it];
            //File.Copy(fileReaderPLY.folderPath + "/" + fileName, Application.dataPath + "/Resources/" + fileName); <- odkomentowac jezeli chcemy faktycznie uzywac innych plikow niz te w assetach yadayada
            //AssetDatabase.ImportAsset("Assets/Resources/" + fileName);
            //AssetDatabase.Refresh();
            //Debug.Log("dodano asset");

            if (it < meshArraySize)
            {
                Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Resources/" + fileName, typeof(Mesh));
                meshes[it] = mesh;

                if (mesh == null)
                {
                    Debug.Log("NULL");
                }
                else
                {
                    Debug.Log("NOT NULL");
                }
            }
        }
        fileIterator = meshArraySize;
        readFinised = true;
    }

    public void ReplaceAsset()
    {
        /*File.Copy(fileReaderPLY.folderPath + "/" + fileReaderPLY.fileNames[fileIterator], Application.dataPath + "/Resources/" + fileReaderPLY.fileNames[fileIterator]);
        //AssetDatabase.ImportAsset("Assets/Resources/" + fileName);
        AssetDatabase.Refresh();
        Debug.Log("dodano asset");

        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Resources/" + fileReaderPLY.fileNames[fileIterator], typeof(Mesh));
        meshes[meshIterator] = mesh;

        if (mesh == null)
        {
            Debug.Log("NULL");
        }
        else
        {
            Debug.Log("NOT NULL");
        }
        fileIterator++;*/

        string fileName = fileReader.fileNames[fileIterator];
        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Resources/" + fileName, typeof(Mesh));
        meshes[meshIterator] = mesh;
        fileIterator++;
    }

    /*public void RemoveFrame()
    {
        AssetDatabase.DeleteAsset("Assets/Resources/" + fileReaderPLY.fileNames[0]);
        //FileUtil.DeleteFileOrDirectory("Assets/Resources/" + fileReaderPLY.fileNames[0]);
        AssetDatabase.Refresh();
        Debug.Log("usunieto");
    }*/

    public void ChangeModelAnimEvent()
    {
        if (readFinised && !isPaused)
        {
            if (fileIterator < fileReader.fileCount + meshArraySize)
            {
                currentMesh = meshes[meshIterator];
                meshFilter.mesh = currentMesh;

                Debug.Log(currentMesh);

                if(fileIterator < fileReader.fileCount)
                    ReplaceAsset();
                else fileIterator++;

                meshIterator++;
                if (meshIterator == meshArraySize) meshIterator = 0;
            }
            //else animation has finished
        }
    }
}
