namespace CTF.Framework.TestRunner
{
	using System;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using CTF.Framework.Attributes;
	using CTF.Framework.Exceptions;

	public class Runner
	{
		private readonly StringBuilder stringBuilder;

		public Runner()
		{
			this.stringBuilder = new StringBuilder();
		}

		public string Run(string assemblyPath)
		{
			if (string.IsNullOrEmpty(assemblyPath))
			{
				return string.Empty;
			}

			var assembly = Assembly.LoadFrom(assemblyPath);
			Type[] testClasses = assembly.GetTypes().Where(c => c.IsClass && c.GetCustomAttributes(typeof(CTFTestClassAttribute)).Any()).ToArray();

			foreach (var testClass in testClasses)
			{
				var methods = testClass.GetMethods().Where(m => m.GetCustomAttributes(typeof(CTFTestMethodAttribute)).Any());

				foreach (var method in methods)
				{
					try
					{
						object classInstance = Activator.CreateInstance(testClass);
						object methodResult = method.Invoke(classInstance, new object[] { });

						stringBuilder.Append($"Class: {testClass.Name} Method: {method.Name} - passed!");
						stringBuilder.AppendLine();
					}
					catch (TargetInvocationException ex)
					{
						TestException innerEx = ex.InnerException as TestException;
						if (innerEx == null)
						{
							stringBuilder.Append($"Unexpected error occurred in {method.Name}!");
							stringBuilder.AppendLine();
						}
						else
						{
							stringBuilder.Append($"Class: {testClass.Name} Method: {method.Name} - failed!");
							stringBuilder.AppendLine();
						}
					}
				}
			}

			return stringBuilder.ToString();
		}
	}
}
