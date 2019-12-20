using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using AutomationFramework.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutomationFramework.Utilities
{
    public class JsonSettingsFile : ISettingsFile
    {
        private readonly string fileContent;
        private readonly string resourceName;

        private JObject JsonObject => JsonConvert.DeserializeObject<JObject>(fileContent);

        public JsonSettingsFile(FileInfo fileInfo)
        {
            resourceName = fileInfo.Name;
            fileContent = FileProvider.GetTextFromFile(fileInfo);
        }

        public JsonSettingsFile(string resourceFileName)
        {
            resourceName = resourceFileName;
            fileContent = FileProvider.GetTextFromResource(resourceFileName);
        }

        public JsonSettingsFile(string embededResourceName, Assembly assembly)
        {
            resourceName = embededResourceName;
            fileContent = FileProvider.GetTextFromEmbeddedResource(embededResourceName, assembly);
        }

        public T GetValue<T>(string path)
        {
            var envValue = GetEnvironmentValue(path);
            if (envValue != null)
            {
                return ConvertEnvVar(() => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(envValue),
                    envValue, path);
            }

            var node = GetJsonNode(path);
            return node.ToObject<T>();
        }

        public IReadOnlyList<T> GetValueList<T>(string path)
        {
            var envValue = GetEnvironmentValue(path);
            if (envValue != null)
            {
                return ConvertEnvVar(() =>
                {
                    return envValue.Split(',').Select(value => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value.Trim())).ToList();
                }, envValue, path);
            }

            var node = GetJsonNode(path);
            return node.ToObject<IReadOnlyList<T>>();
        }

        public IReadOnlyDictionary<string, T> GetValueDictionary<T>(string path)
        {
            var dict = new Dictionary<string, T>();
            var node = GetJsonNode(path);
            foreach (var child in node.Children<JProperty>())
            {
                dict.Add(child.Name, GetValue<T>($".{child.Path}"));
            }

            return dict;
        }

        public bool IsValuePresent(string path)
        {
            return GetEnvironmentValue(path) != null || JsonObject.SelectToken(path) != null;
        }

        private static string GetEnvironmentValue(string jsonPath)
        {
            var key = jsonPath.Replace("['", ".").Replace("']", string.Empty).Substring(1);
            return EnvironmentConfiguration.GetVariable(key);
        }

        private JToken GetJsonNode(string jsonPath)
        {
            var node = JsonObject.SelectToken(jsonPath);
            if (node == null)
            {
                throw new ArgumentException($"There are no values found by path '{jsonPath}' in JSON file '{resourceName}'");
            }
            return node;
        }

        private static T ConvertEnvVar<T>(Func<T> convertMethod, string envValue, string jsonPath)
        {
            Logger.Instance.Debug($"***** Using variable passed from environment {jsonPath.Substring(1)}={envValue}");
            try
            {
                return convertMethod();
            }
            catch (ArgumentException ex)
            {
                var message = $"Value of '{jsonPath}' environment variable has incorrect format: {ex.Message}";
                throw new ArgumentException(message);
            }
        }
    }
}
