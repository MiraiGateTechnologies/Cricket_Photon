using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
public class NameManager : MonoBehaviour
{
    private NamesData namesData;

    void Start()
    {
        LoadNamesFromResources();
       
    }

    private void LoadNamesFromResources()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Names");

        if (jsonFile != null)
        {
            namesData = JsonUtility.FromJson<NamesData>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Failed to load JSON file from Resources folder.");
        }
    }

    public string GetRandomName()
    {
        if (namesData != null && namesData.names != null && namesData.names.Count > 0)
        {
            int seed = (int)DateTime.Now.Ticks;
            UnityEngine.Random.InitState(seed);
            int randomIndex = UnityEngine.Random.Range(0, namesData.names.Count);
            return namesData.names[randomIndex];
        }
        else
        {
            Debug.LogError("Names list is null or empty.");
            return null;
        }
    }
}




[System.Serializable]
public class NamesData
{
    public List<string> names;
}