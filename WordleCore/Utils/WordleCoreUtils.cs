using System.Reflection;

namespace WordleCore.Utils
{
	internal static class WordleCoreUtils
	{
		internal static string LoadEmbeddedResource(string resourceName)
		{
			var assembly = Assembly.GetExecutingAssembly();

			using var stream = assembly.GetManifestResourceStream(resourceName);
			if (null == stream) throw new FileNotFoundException($"Resource {resourceName} not found.");
			using var reader = new StreamReader(stream);

			return reader.ReadToEnd();
		}

		internal static string[] LoadEmbeddedTxt(string resourceName) =>
			LoadEmbeddedResource(resourceName).Split(['\n', '\r']);
	}
}
