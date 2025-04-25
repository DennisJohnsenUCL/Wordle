using System.Reflection;

namespace WordleCore.Utils
{
	public static class WordleCoreUtils
	{
		public static string LoadEmbeddedResource(string resourceName)
		{
			var assembly = Assembly.GetExecutingAssembly();

			using var stream = assembly.GetManifestResourceStream(resourceName);
			if (null == stream) throw new InvalidOperationException($"Resource {resourceName} not found.");
			using var reader = new StreamReader(stream);

			return reader.ReadToEnd();
		}

		public static string[] LoadEmbeddedTxt(string resourceName) =>
			LoadEmbeddedResource(resourceName).Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
	}
}
