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
        iniFilePath = path + @"/settings.vrfilm";

        int counter = 0;
        bool pcdLine = true;
        bool secondLine = false;
        bool framesCountLine = false;
        foreach (string line in System.IO.File.ReadLines(iniFilePath))
        {
            Debug.Log(line);
            if (!pcdLine && !secondLine && !framesCountLine)
            {
                fileNames.Add(line);
                counter++;
            }
            else if(pcdLine)
            {
                pcdLine = false;
                secondLine = true;
            }
            else if(secondLine)
            {
                secondLine = false;
                framesCountLine = true;
            }
            else if(framesCountLine)
            {
                fileCount = int.Parse(line);
                framesCountLine = false;
            }
        }
    }

    public int GetFileCount()
    {
        return fileCount;
    }
}
