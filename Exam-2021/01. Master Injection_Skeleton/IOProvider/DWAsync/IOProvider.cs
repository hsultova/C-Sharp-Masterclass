using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DWAsync
{
	/// <summary>
	/// Handles async IO file operations
	/// </summary>
	public class IOProvider
	{
		/// <summary>
		/// Reads a file asynchronously and write its content to a new file.
		/// </summary>
		/// <param name="filePathToRead">File path to read</param>
		/// <param name="filePathToWrite">File path to write</param>
		/// <param name="token">Cancellation token to cancel the task</param>
		public static async Task DuplicateFileAsync(string filePathToRead, string filePathToWrite, CancellationToken token)
		{
			string text = await ReadAsync(filePathToRead, token);
			await WriteAsync(filePathToWrite, text, token);
		}

		/// <summary>
		/// Reads a file asynchronously.
		/// </summary>
		/// <param name="filePath">File path to read</param>
		/// <param name="token">Cancellation token to cancel the task</param>
		/// <returns>Content of the file</returns>
		public static async Task<string> ReadAsync(string filePath, CancellationToken token)
		{
			Console.WriteLine($"Begin reading file {filePath}");
			string text = await File.ReadAllTextAsync(filePath, token);
			Console.WriteLine($"Complete reading file {filePath}");
			return text;
		}

		/// <summary>
		/// Write a file asynchronously to a new file.
		/// </summary>
		/// <param name="filePath">File path to write</param>
		/// <param name="text">Content to write</param>
		/// <param name="token">Cancellation token to cancel the task</param>
		public static async Task WriteAsync(string filePath, string text, CancellationToken token)
		{
			Console.WriteLine($"Begin writing file {filePath}");
			await File.WriteAllTextAsync(filePath, text, token);
			Console.WriteLine($"Complete writing file {filePath}");
		}
	}
}
