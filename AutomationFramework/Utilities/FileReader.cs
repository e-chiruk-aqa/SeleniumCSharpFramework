using System;
using System.IO;
using System.Reflection;

namespace AutomationFramework.Utilities
{
    public static class FileReader
    {
        private const string ResourcesFolder = "Resources";
        public static string GetTextFromEmbeddedResource(string embeddedResourcePath, Assembly resourceAssembly = null)
        {
            var assembly = resourceAssembly ?? Assembly.GetCallingAssembly();
            var resourcePath = $"{assembly.GetName().Name}.{embeddedResourcePath}";
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException(
                        $"Assembly {assembly.FullName} doesn't contain EmbeddedResource at path {resourcePath}. Resource file cannot be loaded");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string GetTextFromResource(string fileName)
        {
            return GetTextFromFile(GetResourceFile(fileName));
        }

        public static bool IsResourceFileExist(string fileName)
        {
            var fileInfo = GetResourceFile(fileName);
            return fileInfo.Exists;
        }

        public static string GetTextFromFile(FileInfo fileInfo)
        {
            using (var reader = fileInfo.OpenText())
            {
                return reader.ReadToEnd();
            }
        }

        private static FileInfo GetResourceFile(string fileName)
        {
            return new FileInfo(Path.Combine(AppContext.BaseDirectory, ResourcesFolder, fileName));
        }
    }
}
