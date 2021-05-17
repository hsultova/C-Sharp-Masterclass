using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DWAsync
{
	public class Program
	{
		private const string  _filesFolder = "Files/";
		private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		public static void Main(string[] args)
		{
			string newFilePath;
			DirectoryInfo directory = new DirectoryInfo(_filesFolder);
			FileInfo[] files = directory.GetFiles("*.txt");
			foreach(var file in files)
			{
				newFilePath =$"{_filesFolder}{Path.GetFileNameWithoutExtension(file.Name)}_async.txt";
				_ = IOProvider.DuplicateFileAsync(file.FullName, newFilePath, _cancellationTokenSource.Token);
			}

			Task.WaitAll();
		}
	}
}
