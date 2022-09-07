using System.Collections.Generic;
using UnityEngine;

public class FileReader : MonoBehaviour
{
    public string folderPath;
    public string iniFilePath;
    public int fileCount;
    public float offset;
    public List<string> fileNames;

    public void SetVariables(string path)
    {
        folderPath = path;
        iniFilePath = path + "/settings.vrfilm";

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
                offset = int.Parse(line) / 1000.0f;
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
