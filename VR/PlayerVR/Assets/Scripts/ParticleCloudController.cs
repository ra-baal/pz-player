using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleCloudController : MonoBehaviour
{
    [SerializeField] private List<GameObject> frames;
    [SerializeField] private FileReader fileReader;
    [SerializeField] private MainMenuController mmc;

    private GameObject currentFrame;
    private int fileIterator;

    // Start is called before the first frame update
    void Start()
    {
        fileReader = FindObjectOfType<FileReader>();

        if (frames.Count != 0 && fileReader.fileCount != 0)
        {
            ReadPCD(frames[0]);
            fileIterator++;
            currentFrame = frames[0];
            currentFrame.SetActive(true);
        }
        else
        {
            Debug.LogError("No frames added to the list!");
        }
    }

    ParticleSystem particleSystem = null;
    Particle[] particles = null;
    Particle[] particlesTmp = null;
    int pointCount = 0;

    private void ReadPCD(GameObject frame)
    {
        particleSystem = frame.GetComponent<ParticleSystem>();

        string file = fileReader.folderPath + "/" + fileReader.fileNames[fileIterator];
        Debug.Log(file);
        int fileIter = 0;

        int particleIter = 0;

        foreach (string line in System.IO.File.ReadLines(file))
        {
            if (fileIter == 9)
            {
                string s_pointCount = line.Split(' ')[1];
                pointCount = int.Parse(s_pointCount);
                particles = new Particle[pointCount];
                particlesTmp = new Particle[pointCount];
            }
            else if (fileIter > 11)
            {
                string[] info = line.Split(' ');
                float x = float.Parse(info[0], CultureInfo.InvariantCulture.NumberFormat) * 20;
                float y = float.Parse(info[1], CultureInfo.InvariantCulture.NumberFormat) * 20;
                float z = float.Parse(info[2], CultureInfo.InvariantCulture.NumberFormat) * 20;

                UInt32 color = UInt32.Parse(info[3]);
                byte[] bytes = BitConverter.GetBytes(color);

                particles[particleIter].position = new Vector3(x, y, z);
                particles[particleIter].startColor = new Color32(bytes[0], bytes[1], bytes[2], 255);
                particles[particleIter].color = new Color32(bytes[0], bytes[1], bytes[2], 255);
                particleIter++;
            }
            fileIter++;
        }

        particleSystem.maxParticles = pointCount;
        particleSystem.emissionRate = pointCount;

        Debug.Log("All set");
    }

    float timer = 0.0f;
    int secondsPast = 0;
    int seconds = 0;

    void LateUpdate()
    {
        if (!mmc.Pause)
        {
            if (particleSystem.isPaused) particleSystem.Play();

            int particleCountCurr = particleSystem.GetParticles(particlesTmp, pointCount);

            for (int i = 0; i < particleCountCurr; i++)
            {
                particlesTmp[i].position = particles[i].position;
                particlesTmp[i].startColor = particles[i].startColor;
            }
            particleSystem.SetParticles(particlesTmp, pointCount);

            timer += Time.deltaTime;
            seconds = (int)(timer % 60);

            if (fileIterator <= fileReader.fileCount && seconds > secondsPast)
            {
                secondsPast = seconds;
                ReadPCD(frames[0]);
                fileIterator++;
            }
        }
        else if (!particleSystem.isPaused)
        {
            particleSystem.SetParticles(particlesTmp, 0);
            particleSystem.Pause();
        }
    }
}