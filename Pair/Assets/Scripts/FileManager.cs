using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;

public static class FileManager
{
	static BinaryFormatter GetFormatter() {
		var bf = new BinaryFormatter();

		SurrogateSelector ss = new SurrogateSelector();

		ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
		ss.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector2SerializationSurrogate());
		ss.AddSurrogate(typeof(Ray), new StreamingContext(StreamingContextStates.All), new RaySerializationSurrogate());

		bf.SurrogateSelector = ss;

		return bf;
	}

    public static T Deserialize<T>(byte[] data) where T : class {
        var bf = new BinaryFormatter();
        var stream = new MemoryStream(data);
        var result = bf.Deserialize(stream) as T;        
        return result;
    }

	public static string FullName(string filename) {
		return Application.persistentDataPath + "/" + filename;
	}

	public static T LoadFromFile<T>(string filename, bool full = false) where T : class {
		if (!full) {
			filename = FullName(filename);
		}
		Debug.Log("Loading from: " + filename);
		T result = null;
		FileStream fs = null;
		try {
			var bf = GetFormatter();
			fs = new FileStream(filename, FileMode.Open);
			result = bf.Deserialize(fs) as T;
		} catch (Exception e) {
			Debug.LogException(e);
			Debug.Log(e.StackTrace);
		} finally {
			if (fs != null) fs.Close();
		}
		return result;
	}

	public static void SaveToFile<T>(T data, string filename) {
		filename = FullName(filename);
		Debug.Log("Saving to: " + filename);
		FileStream fs = null;
		try {
			var bf = GetFormatter();

			fs = new FileStream(filename, FileMode.Create);
			bf.Serialize(fs, data);
		} catch (Exception e) {
			Debug.LogException(e);
			Debug.Log(e.StackTrace);
		} finally {
			if (fs != null) fs.Close();
		}
	}
}
