namespace MasterInjection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class MyServiceProvider : IMyServiceProvider
	{
		private readonly Dictionary<Type, Type> _mappings;

		public MyServiceProvider()
		{
			_mappings = new Dictionary<Type, Type>();
		}

		public void Add<TSource, TDestination>()
			where TDestination : TSource
		{
			Type sourceType = typeof(TSource);
			Type destinationType = typeof(TDestination);

			if(_mappings.ContainsKey(sourceType))
			{
				//Don't replace the type if already exists mapping for it
				return;
			}

			_mappings.Add(sourceType, destinationType);
		}

		public object CreateInstance(Type type)
		{
			Type target = ResolveType(type);
			ConstructorInfo[] constructors = target.GetConstructors();

			int current = 0;
			int min = int.MaxValue;
			int index = 0;
			for(int i =0; i< constructors.Length; i++)
			{
				current = constructors[i].GetParameters().Length;
				if (min > current )
				{
					min = current;
					index = i;
				}
			}

			ConstructorInfo constructor = constructors[index];
			ParameterInfo[] parameters = constructor.GetParameters();

			var resolvedParameters = new List<object>();
			foreach (var parameter in parameters)
			{
				resolvedParameters.Add(CreateInstance(parameter.ParameterType));
			}

			return constructor.Invoke(resolvedParameters.ToArray());
		}

		public T CreateInstance<T>()
		{
			var type = typeof(T);
			return (T)CreateInstance(type);
		}

		private Type ResolveType(Type type)
		{
			if (_mappings.Keys.Contains(type))
			{
				return _mappings[type];
			}
			else if(_mappings.ContainsValue(type))
			{
				return type;
			}
			else
			{
				throw new ResolveTypeException();
			}
		}
	}
}
