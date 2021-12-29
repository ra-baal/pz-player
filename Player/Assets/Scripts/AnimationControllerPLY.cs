using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationControllerPLY : MonoBehaviour
{
    public MeshFilter meshFilter;

    public List<Mesh> meshes;
    private int meshIterator;
    private Mesh currentMesh;

    [SerializeField] private List<GameObject> frames;
    private int frameCount;
    private int frameIterator;

    [SerializeField] private string[] fileNames;

    void Start()
    {
        meshIterator = 0;
        //ReadFile();
        currentMesh = meshes[0];
    }

    public void ReadFile()
    {
        //importujemy wszystkie klatki i wczytujemy je jako assety, nastepnie wydobywamy wszystkie meshe i dodajemy je do listy
        //jezeli wszystko pojdzie dobrze samo animowanie powinno opieraæ siê tylko na evencie ponizej
        foreach(string fileName in fileNames)
        {
            AssetDatabase.ImportAsset("pathToFile" + fileName);

            Mesh mesh = Resources.Load("newPathToFile" + fileName) as Mesh;
            mesh.name = fileName;
            meshes.Add(mesh);

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

    public void ChangeModelAnimEvent()
    {
        if (meshIterator < meshes.Count)
        {
            currentMesh = meshes[meshIterator];
            meshFilter.name = currentMesh.name;
            meshFilter.mesh = currentMesh;
            meshIterator++;
        }
    }
}
