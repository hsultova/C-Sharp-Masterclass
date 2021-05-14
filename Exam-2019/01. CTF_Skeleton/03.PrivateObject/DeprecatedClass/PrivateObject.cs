namespace DeprecatedClass
{
	using System;
	using System.Reflection;

	public class PrivateObject
	{
		private readonly object _object;
		private readonly Type _objectType;

		public PrivateObject(object obj)
		{
			_object = obj;
			_objectType = _object.GetType();
		}

		public object Invoke(string methodName, params object[] parameters)
		{
			MethodInfo method = _objectType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
			object result = method.Invoke(_object, parameters);
			return result;
		}
	}
}
