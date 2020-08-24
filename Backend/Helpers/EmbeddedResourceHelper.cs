using System.IO;
using System.Linq;
using System.Reflection;

namespace Backend.Helpers
{
    public static class EmbeddedResourceHelper
    {
        /// <summary>
        /// Read file contents as string from the specified filename attached as an embedded resource
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Contents of the file if found</returns>
        public static string GetFileContentsAsString(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var resource = assembly.GetManifestResourceStream(resourceNames.Single(x => x.Contains(filename)));

            using var reader = new StreamReader(resource);
            return reader.ReadToEnd();
        }
    }
}
