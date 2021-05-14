namespace CTF.Framework.Asserts
{
	using System;
	using System.Reflection;
	using CTF.Framework.Exceptions;

	// ReSharper disable once InconsistentNaming
	public abstract class CTFAssert
	{
		public static void AreEqual(object a, object b)
		{
			if ((a == null || b == null))
			{
				throw new TestException();
			}

			if (ReferenceEquals(a, b))
			{
				return;
			}

			Type aType = a.GetType();
			Type bType = b.GetType();

			if (!aType.Equals(bType))
			{
				throw new TestException();
			}

			//After it is clear that aType = bType, use only aType for checking
			if (!aType.IsPrimitive && aType != typeof(string))
			{
				MemberInfo[] members = aType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

				foreach (var member in members)
				{
					object aValue = null;
					object bValue = null;
					switch (member.MemberType)
					{
						case MemberTypes.Field:
							aValue = aType.GetField(member.Name, BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(a);
							bValue = bType.GetField(member.Name, BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(b);
							break;
						case MemberTypes.Property:
							aValue = aType.GetProperty(member.Name)?.GetValue(a);
							bValue = bType.GetProperty(member.Name)?.GetValue(b);
							break;
					}

					if (aValue == null || bValue == null)
					{
						continue;
					}

					if (!aValue.Equals(bValue))
					{
						throw new TestException();
					}
				}
			}
			else
			{
				if (!a.Equals(b))
				{
					throw new TestException();
				}
			}
		}

		public static void AreNotEqual(object a, object b)
		{
			if ((a == null || b == null))
			{
				throw new TestException();
			}

			Type aType = a.GetType();
			Type bType = b.GetType();

			//After it is clear that aType = bType, use only aType for checking
			if (aType.Equals(bType) && !aType.IsPrimitive && aType != typeof(string))
			{
				MemberInfo[] members = aType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

				bool areEquals = true;
				foreach (var member in members)
				{
					object aValue = null;
					object bValue = null;
					switch (member.MemberType)
					{
						case MemberTypes.Field:
							aValue = aType.GetField(member.Name, BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(a);
							bValue = bType.GetField(member.Name, BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(b);
							break;
						case MemberTypes.Property:
							aValue = aType.GetProperty(member.Name)?.GetValue(a);
							bValue = bType.GetProperty(member.Name)?.GetValue(b);
							break;
					}

					if (aValue == null || bValue == null)
					{
						continue;
					}

					if (!aValue.Equals(bValue))
					{
						areEquals = false;
					}
				}

				if (areEquals)
				{
					throw new TestException();
				}
			}

			if (a.Equals(b))
			{
				throw new TestException();
			}
		}

		public static void Throws<T>(Func<bool> condition)
					where T : Exception
		{
			try
			{
				condition.Invoke();
			}
			catch (T)
			{
				return;
			}
			catch (Exception)
			{
				throw new TestException();
			}
		}
	}
}
