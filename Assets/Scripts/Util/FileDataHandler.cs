using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAS.Models;
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
        m_fileName = string.Format("{0}_{1}.json", model.Name.Trim(), DateTime.Now.ToString("mm'_'dd'_'yy'_'hh'_'mm"));
        string fullFilePath = Path.Combine(m_saveDataDir, m_fileName);
        Debug.LogFormat("DEBUG... Attempting to save category model to: {0}", fullFilePath);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));

            // Create JSON from category model
            string categoryModelJson = JsonConvert.SerializeObject(model, Formatting.Indented);
            Debug.LogFormat("DEBUG... Category model JSON string: {0}", categoryModelJson);

            // Save the data to a file
            using (FileStream fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                // Store the text in a byte array with
                // UTF8 encoding (8-bit Unicode
                // Transformation Format)
                byte[] writeArr = Encoding.UTF8.GetBytes(categoryModelJson);

                // Using the Write method write
                // the encoded byte array to
                // the textfile
                fileStream.Write(writeArr, 0, categoryModelJson.Length);

                // Close the FileStream object
                fileStream.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogFormat("DEBUG... Error occurred saving category model to directory: {0} with message: {1}", fullFilePath, e.Message);
        }
        return true;
    }
}
