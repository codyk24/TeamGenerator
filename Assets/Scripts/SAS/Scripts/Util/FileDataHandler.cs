using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAS.Models;
using SAS.Managers;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Text;

public static class FileDataHandler
{
    private static string m_saveDataDir = Application.persistentDataPath;
    private static string m_fileName = "";

    public static bool SaveCategory(CategoryModel model)
    {
        m_fileName = string.Format("category_{0}_{1}.json", model.Name.Replace(" ", string.Empty), DateTime.Now.ToString("MM'_'dd'_'yy'_'hh'_'mm"));
        try
        {
            // Create JSON from category model
            string categoryModelJson = JsonConvert.SerializeObject(model, Formatting.Indented);
            Debug.LogFormat("DEBUG... Category model JSON string: {0}", categoryModelJson);
            WriteJSONToFile(categoryModelJson);
        }
        catch (Exception e)
        {
            Debug.LogFormat("DEBUG... Error occurred saving category model with message: {1}", e.Message);
        }
        return true;
    }

    public static bool SaveCategories()
    {
        m_fileName = string.Format("categoryList_{0}.json", DateTime.Now.ToString("MM'_'dd'_'yy'_'hh'_'mm"));
        try
        {
            // Create JSON from category model
            string categoriesJson = JsonConvert.SerializeObject(CategoryManager.Instance.Categories, Formatting.Indented);
            Debug.LogFormat("DEBUG... Category model JSON string: {0}", categoriesJson);
            WriteJSONToFile(categoriesJson);
        }
        catch (Exception e)
        {
            Debug.LogFormat("DEBUG... Error occurred saving category model with message: {0}", e.Message);
        }
        return true;
    }

    public static bool SaveTeams()
    {
        m_fileName = string.Format("teamsList_{0}.json", DateTime.Now.ToString("MM'_'dd'_'yy'_'hh'_'mm"));
        try
        {
            // Create JSON from category model
            string teamsJson = JsonConvert.SerializeObject(TeamManager.Instance.Teams, Formatting.Indented);
            Debug.LogFormat("DEBUG... Category model JSON string: {0}", teamsJson);
            WriteJSONToFile(teamsJson);
        }
        catch (Exception e)
        {
            Debug.LogFormat("DEBUG... Error occurred saving category model with message: {0}", e.Message);
        }
        return true;
    }

    public static bool SaveEventJson()
    {
        m_fileName = string.Format("event_{0}_{1}.json", TeamEventManager.Instance.EventModel.Name.Replace(" ", string.Empty), DateTime.Now.ToString("MM'_'dd'_'yy"));
        try
        {
            // Create JSON from category model
            string eventJson = JsonConvert.SerializeObject(TeamEventManager.Instance.EventModel, Formatting.Indented);
            Debug.LogFormat("DEBUG... Event JSON path: {0} string: {1}", m_fileName, eventJson);
            WriteJSONToFile(eventJson);
        }
        catch (Exception e)
        {
            Debug.LogFormat("DEBUG... Error occurred saving category model with message: {0}", e.Message);
        }
        return true;
    }

    public static bool WriteJSONToFile(string json)
    {
        string fullFilePath = Path.Combine(m_saveDataDir, m_fileName);
        Debug.LogFormat("DEBUG... Attempting to save JSON model to: {0}", fullFilePath);

        try
        {
            //Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath))
            File.WriteAllText(fullFilePath, json);

            // Save the data to a file
            //using (FileStream fileStream = File.Open(fullFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            //{
            //    // Store the text in a byte array with
            //    // UTF8 encoding (8-bit Unicode
            //    // Transformation Format)
            //    byte[] writeArr = Encoding.UTF8.GetBytes(json);

            //    // Using the Write method write
            //    // the encoded byte array to
            //    // the textfile
            //    fileStream.Write(writeArr, 0, json.Length);

            //    // Close the FileStream object
            //    fileStream.Close();
            //}
            return true;
        }
        catch (Exception e)
        {
            Debug.LogFormat("DEBUG... Error occurred saving JSON to directory: {0} with message: {1}", fullFilePath, e.Message);
            return false;
        }
    }
}
