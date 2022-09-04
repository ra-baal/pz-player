using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileReader : MonoBehaviour
{
    public string folderPath;
    public string iniFilePath;
    public int fileCount;
    public List<string> fileNames;

    public void SetVariables(string path)
    {
        folderPath = path;
        iniFilePath = path + "/kinectv2-settings.vrfilm";

        int counter = 0;
        bool firstLine = true;
        bool secondLine = false;
        bool thirdLine = false;

        foreach (string line in System.IO.File.ReadLines(iniFilePath))
        {
            Debug.Log(line);
            if (!firstLine && !secondLine && !thirdLine)
            {
                fileNames.Add(line);
                counter++;
            }
            else if(firstLine)
            {
                firstLine = false;
                secondLine = true;
            }
            else if(secondLine)
            {
                secondLine = false;
                thirdLine = true;
            }
            else if (thirdLine)
            {
                fileCount = int.Parse(line);
                thirdLine = false;
            }
        }
    }

    public int GetFileCount()
    {
        return fileCount;
    }
}
