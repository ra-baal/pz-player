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
        List<string> dirs = new List<string>(Directory.EnumerateDirectories(Application.persistentDataPath + "\\LoopReality\\"));

        foreach (var dir in dirs)
        {
            GameObject title = Instantiate(titleTemplate) as GameObject;
            title.SetActive(true);

            title.GetComponent<TitleListElement>().SetText($"{dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)}");
            title.transform.SetParent(titleTemplate.transform.parent, false);
        }
    }
    
    public void TitleClicked(string selectedTitle)
    {
        title.text = selectedTitle;
        mmc.LoadFile(selectedTitle);
    }
}
