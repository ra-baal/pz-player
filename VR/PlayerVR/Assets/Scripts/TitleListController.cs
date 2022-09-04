using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TitleListController : MonoBehaviour
{
    [SerializeField] private GameObject titleTemplate;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private MainMenuController mmc;
    void Start()
    {
        Debug.Log("Reading titles...");
        GenerateTitleList();
    }
    void GenerateTitleList()
    {
        DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath + "/LoopReality");

        try
        {
            DirectoryInfo[] directories = dataDir.GetDirectories();
            Debug.Log(Application.persistentDataPath + "/LoopReality\nFound titles: " + directories.Length);

            foreach (var directory in directories)
            {
                var name = directory.Name;
                Debug.Log("Found title: " + name);

                GameObject title = Instantiate(titleTemplate) as GameObject;
                title.SetActive(true);
                title.GetComponent<TitleListElement>().SetText(name);
                title.transform.SetParent(titleTemplate.transform.parent, false);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
    
    public void TitleClicked(string selectedTitle)
    {
        title.text = selectedTitle;
        mmc.LoadFile(selectedTitle);
    }
}
