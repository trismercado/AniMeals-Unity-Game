using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DatabaseLoader : MonoBehaviour
{


    [SerializeField]
    public List<FoodData> _food = new List<FoodData>();
    private const string FILE_EXTENSION = @".json";
    // private const string DATABASE_NAME = @"ResourceLoader/foodDatabase.json";
    // Start is called before the first frame update

    void Start()
    {
        // _food = ReturnDatabase<FoodData>(DATABASE_NAME);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string RemoveFileExtension(string path)
    {
        if (path.Length >= FILE_EXTENSION.Length)
        {
            if (path.ToLower().Substring(path.Length - FILE_EXTENSION.Length, FILE_EXTENSION.Length) == FILE_EXTENSION.ToLower())
                return path.Substring(0, path.Length - FILE_EXTENSION.Length);
            else
                return path;
        }
        else
        {
            return path;
        }   
    }

    private string RemoveLeadingDirectorySeparator(string path) {
        if (char.Parse(path.Substring(0, 1)) == Path.DirectorySeparatorChar || char.Parse(path.Substring(0, 1)) == Path.AltDirectorySeparatorChar)
            return path.Substring(1);
        else
            return path;
    }

    private string ReturnFileResource(string path) 
    {
        path = RemoveFileExtension(path);
        path = RemoveLeadingDirectorySeparator(path);

        if (path == string.Empty)
        {
            Debug.LogError("ReturnFileResource -> path is empty.");
            return string.Empty;
        }

        TextAsset textAsset = Resources.Load(path) as TextAsset;

        if (textAsset != null)
            return textAsset.text;
        else
            return string.Empty;

    }

    public List<T> ReturnDatabase<T>(string path)
    {
        string result = ReturnFileResource(path);
        if (result.Length != 0)
        {
            return JsonConvert.DeserializeObject<List<T>>(result).ToList();
        }
        else
        {
            Debug.LogWarning("ReturnDatabase -> result text is empty.");
            return new List<T>();
        }
    }
}
