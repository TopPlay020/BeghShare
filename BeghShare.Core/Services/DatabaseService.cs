using BeghCore;
using System.Text.Json;

namespace BeghShare.Core.Services
{
    public class DatabaseService : ITransient
    {
        private const string _filePath = "App.json";
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public void Set<T>(string key, T value)
        {
            var data = LoadData();
            data[key] = JsonSerializer.SerializeToElement(value, _jsonOptions);
            SaveData(data);
        }

        public T Get<T>(string key)
        {
            var data = LoadData();
            if (data.TryGetValue(key, out var element))
            {
                return element.Deserialize<T>(_jsonOptions);
            }
            return default;
        }

        public bool Exists(string key)
        {
            var data = LoadData();
            return data.ContainsKey(key);
        }

        public void Remove(string key)
        {
            var data = LoadData();
            data.Remove(key);
            SaveData(data);
        }

        private Dictionary<string, JsonElement> LoadData()
        {
            if (!File.Exists(_filePath))
                return new Dictionary<string, JsonElement>();

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, _jsonOptions)
                       ?? new Dictionary<string, JsonElement>();
            }
            catch
            {
                return new Dictionary<string, JsonElement>();
            }
        }

        private void SaveData(Dictionary<string, JsonElement> data)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
    }
}