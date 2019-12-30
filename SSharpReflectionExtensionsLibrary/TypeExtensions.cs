#region License

/*
 * TypeExtensions.cs
 *
 * The MIT License
 *
 * Copyright © 2017 Nivloc Enterprises Ltd
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Crestron.SimplSharp;
using System.Globalization;

namespace Crestron.SimplSharp.Reflection
	{
	public static class TypeEx
		{
		public static CType[] GetCTypes (this Type[] types)
			{
			return types.Select (t => t.GetCType ()).ToArray ();
			}

		public static ConstructorInfo GetConstructor (this Type type, CType[] types)
			{
			return type.GetCType ().GetConstructor (types);
			}

		public static ConstructorInfo GetConstructor (this Type type, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetConstructor (bindingAttr, binder, types.GetCTypes (), modifiers);
			}

		public static ConstructorInfo[] GetConstructors (this Type type, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetConstructors (bindingAttr);
			}

		public static object[] GetCustomAttribute (this Type type, CType attributeType, bool inherit)
			{
			return type.GetCType ().GetCustomAttributes (attributeType, inherit);
			}

		public static EventInfo GetEvent (this Type type, string name, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetEvent (name, bindingAttr);
			}

		public static EventInfo[] GetEvents (this Type type, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetEvents (bindingAttr);
			}

		public static FieldInfo GetField (this Type type, string name, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetField (name, bindingAttr);
			}

		public static FieldInfo[] GetFields (this Type type, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetFields (bindingAttr);
			}

		public static MemberInfo[] GetMember (this Type type, string name, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetMember (name, bindingAttr);
			}

		public static MemberInfo[] GetMember (this Type type, string name, MemberTypes memberType, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetMember (name, bindingAttr).Where (m => m.MemberType == memberType).ToArray ();
			}

		public static MemberInfo[] GetMembers (this Type type, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetMembers (bindingAttr);
			}

		public static MethodInfo GetMethod (this Type type, string name, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetMethod (name, bindingAttr);
			}

		public static MethodInfo GetMethod (this Type type, string name, CType[] types)
			{
			return type.GetCType ().GetMethod (name, types);
			}

		public static MethodInfo GetMethod (this Type type, string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetMethod (name, bindingAttr, binder, types.GetCTypes (), modifiers);
			}

		public static MethodInfo GetMethod (this Type type, string name, BindingFlags bindingAttr, Binder binder, CType[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetMethod (name, bindingAttr, binder, types, modifiers);
			}

		public static MethodInfo GetMethod (
			this Type type, string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConventions, Type[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetMethod (name, bindingAttr, binder, callConventions, types.GetCTypes (), modifiers);
			}

		public static MethodInfo GetMethod (
			this Type type, string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConventions, CType[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetMethod (name, bindingAttr, binder, callConventions, types, modifiers);
			}

		public static MethodInfo GetMethod (this Type type, string name, bool generic)
			{
			if (type == null)
				throw new ArgumentNullException ("type");
			if (String.IsNullOrEmpty (name))
				throw new ArgumentNullException ("name");
			return type.GetCType ().GetMethods ().FirstOrDefault (method => method.Name == name & method.IsGenericMethod == generic);
			}

		public static MethodInfo GetMethod<T> (this Type type, string name)
			{
			var genMethod = type.GetMethod (name, true);
			return genMethod == null ? null : genMethod.MakeGenericMethod (typeof(T));
			}

		public static MethodInfo GetNonGenericMethod (this Type type, string name)
			{
			return type.GetMethod (name, false);
			}

		public static MethodInfo GetMethod<T> (this Type type, string name, CType[] types)
			{
			return GetMethod<T> (type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, types);
			}

		public static MethodInfo GetMethod<T> (this Type type, string name, BindingFlags bindingAttr, CType[] types)
			{
			if (type == null)
				throw new ArgumentNullException ("type");

			if (types == null)
				types = ArrayEx.Empty<CType> ();

			var methods = type.GetCType ().GetMethods (bindingAttr);
			var listMethods = new List<MethodInfo> ();
			foreach (var oMethod in methods)
				{
				if (!string.Equals (oMethod.Name, name))
					continue;
				if (!oMethod.IsGenericMethod)
					continue;
				var mi = oMethod.MakeGenericMethod (typeof (T));
				var aoParameter = mi.GetParameters ();
				if (aoParameter.Length != types.Length)
					continue;
				int iParamMatch = 0;
				for (int ixParam = 0; ixParam < aoParameter.Length; ixParam++)
					{
					if (aoParameter[ixParam].ParameterType == types[ixParam])
						iParamMatch++;
					}
				if (iParamMatch != aoParameter.Length)
					continue;
				listMethods.Add (mi);
				}

			if (listMethods.Count != 1)
				{
				string sError = "Method with Name '" + name + "' and BindingFlags '" + bindingAttr + "' and Parameter Types '" + types.ToString () + "'";
				if (listMethods.Count == 0)
					throw new MissingMethodException (sError + " has no match.");
				else
					throw new AmbiguousMatchException (sError + " has " + listMethods.Count + " matches.", null);
				}
			return listMethods[0];
			}

		public static MethodInfo GetNonGenericMethod (this Type type, string name, CType[] types)
			{
			return GetNonGenericMethod (type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, types);
			}

		public static MethodInfo GetNonGenericMethod (this Type type, string name, BindingFlags bindingAttr, CType[] types)
			{
			if (type == null)
				throw new ArgumentNullException ("type");

			if (types == null)
				types = ArrayEx.Empty<CType> ();

			var methods = type.GetCType().GetMethods (bindingAttr);
			var listMethods = new List<MethodInfo> ();
			foreach (var mi in methods)
				{
				if (!string.Equals (mi.Name, name))
					continue;
				if (mi.IsGenericMethod)
					continue;
				var aoParameter = mi.GetParameters ();
				if (aoParameter.Length != types.Length)
					continue;
				int iParamMatch = 0;
				for (int ixParam = 0; ixParam < aoParameter.Length; ixParam++)
					{
					if (aoParameter[ixParam].ParameterType == types[ixParam])
						iParamMatch++;
					}
				if (iParamMatch != aoParameter.Length)
					continue;
				listMethods.Add (mi);
				}

			if (listMethods.Count != 1)
				{
				string sError = "Method with Name '" + name + "' and BindingFlags '" + bindingAttr + "' and Parameter Types '" + types.ToString () + "'";
				if (listMethods.Count == 0)
					throw new MissingMethodException (sError + " has no match.");
				else
					throw new AmbiguousMatchException (sError + " has " + listMethods.Count + " matches.", null);
				}
			return listMethods[0];
			}

		public static MethodInfo[] GetMethods (this Type type, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetMethods (bindingAttr);
			}

		public static CType GetNestedType (this Type type, string name, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetNestedType (name, bindingAttr);
			}

		public static CType[] GetNestedTypes (this Type type, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetNestedTypes (bindingAttr);
			}

		public static PropertyInfo GetProperty (this Type type, string name, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetProperty (name, bindingAttr);
			}

		public static PropertyInfo GetProperty (this Type type, string name, CType returnType)
			{
			return type.GetCType ().GetProperty (name, returnType);
			}

		public static PropertyInfo GetProperty (this Type type, string name, CType returnType, Type[] types)
			{
			return type.GetCType ().GetProperty (name, returnType, types.GetCTypes ());
			}

		public static PropertyInfo GetProperty (this Type type, string name, CType returnType, CType[] types)
			{
			return type.GetCType ().GetProperty (name, returnType, types);
			}

		public static PropertyInfo GetProperty (this Type type, string name, Type returnType, CType[] types)
			{
			return type.GetCType ().GetProperty (name, returnType, types);
			}

		public static PropertyInfo GetProperty (this Type type, string name, CType returnType, Type[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetProperty (name, returnType, types.GetCTypes (), modifiers);
			}

		public static PropertyInfo GetProperty (this Type type, string name, CType returnType, CType[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetProperty (name, returnType, types, modifiers);
			}

		public static PropertyInfo GetProperty (this Type type, string name, Type returnType, CType[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetProperty (name, returnType, types, modifiers);
			}

		public static PropertyInfo GetProperty (
			this Type type, string name, BindingFlags bindingAttr, Binder binder, CType returnType, Type[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetProperty (name, bindingAttr, binder, returnType, types.GetCTypes (), modifiers);
			}

		public static PropertyInfo GetProperty (
			this Type type, string name, BindingFlags bindingAttr, Binder binder, CType returnType, CType[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetProperty (name, bindingAttr, binder, returnType, types, modifiers);
			}

		public static PropertyInfo GetProperty (
			this Type type, string name, BindingFlags bindingAttr, Binder binder, Type returnType, CType[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetProperty (name, bindingAttr, binder, returnType, types, modifiers);
			}

		public static PropertyInfo GetProperty (
			this Type type, string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
			{
			return type.GetCType ().GetProperty (name, bindingAttr, binder, returnType, types.GetCTypes (), modifiers);
			}

		public static PropertyInfo[] GetProperties (this Type type, BindingFlags bindingAttr)
			{
			return type.GetCType ().GetProperties (bindingAttr);
			}

		public static bool IsRestricted (this Type type)
			{
			try
				{
				CType ct = null;
				ct = type;
				if (ct == null)
					return true;
				MemberTypes mt = MemberTypes.All;
				mt = ct.MemberType;
				return mt == MemberTypes.All;
				}
			catch (RestrictionViolationException)
				{
				return true;
				}
			catch (NullReferenceException)
				{
				return true;
				}
			}

		public static string AssemblyFullName (this Type type)
			{
			string typeName, assemblyName;
			SplitFullyQualifiedTypeName (type.AssemblyQualifiedName, out typeName, out assemblyName);

			return assemblyName;
			}

		public static string AssemblySimpleName (this Type type)
			{
			var assemblyFullName = type.AssemblyFullName ();
			var ix = assemblyFullName.IndexOf (',');
			return ix == -1 ? assemblyFullName : assemblyFullName.Substring (0, ix);
			}

		public static Version AssemblyVersion (this Type type)
			{
			var assemblyFullName = type.AssemblyFullName ();
			var ix = assemblyFullName.IndexOf ("Version=", StringComparison.InvariantCulture);
			var version = assemblyFullName.Substring (ix + 8, assemblyFullName.IndexOf (',', ix + 8) - (ix + 8));
			return new Version (version);
			}

		public static CultureInfo AssemblyCultureInfo (this Type type)
			{
			var assemblyFullName = type.AssemblyFullName ();
			var ix = assemblyFullName.IndexOf ("Culture=", StringComparison.InvariantCulture);
			if (ix == -1)
				return CultureInfo.InvariantCulture;

			var culture = assemblyFullName.Substring (ix + 8, assemblyFullName.IndexOf (',', ix + 8) - (ix + 8));
			if (culture == "neutral")
				return CultureInfo.InvariantCulture;

			return CultureInfo.GetCultureInfo (culture);
			}

		public static string AssemblyPublicKeyToken (this Type type)
			{
			var assemblyFullName = type.AssemblyFullName ();
			var ix = assemblyFullName.IndexOf ("PublicKeyToken=", StringComparison.InvariantCulture);
			if (ix == -1)
				return null;

			return assemblyFullName.Substring (ix + 15);
			}

		public static AssemblyName AssemblyName (this Type type)
			{
			var an = new AssemblyName {Name = type.AssemblySimpleName (), CultureInfo = type.AssemblyCultureInfo (), Version = type.AssemblyVersion (),};

			var apkt = type.AssemblyPublicKeyToken ();
			if (apkt != null)
				an.SetPublicKeyToken (apkt.HexToBytes ());

			return an;
			}

		public static string AssemblyFullName (this string aqn)
			{
			string typeName, assemblyName;
			SplitFullyQualifiedTypeName (aqn, out typeName, out assemblyName);

			return assemblyName;
			}

		public static string AssemblySimpleName (this string aqn)
			{
			var assemblyFullName = aqn.AssemblyFullName ();
			var ix = assemblyFullName.IndexOf (',');
			return ix == -1 ? assemblyFullName : assemblyFullName.Substring (0, ix);
			}

		public static Version AssemblyVersion (this string aqn)
			{
			var assemblyFullName = aqn.AssemblyFullName ();
			var ix = assemblyFullName.IndexOf ("Version=", StringComparison.InvariantCulture);
			if (ix == -1)
				return new Version (0, 0, 0, 0);
			var version = assemblyFullName.Substring (ix + 8, assemblyFullName.IndexOf (',', ix + 8) - (ix + 8));
			return new Version (version);
			}

		public static CultureInfo AssemblyCultureInfo (this string aqn)
			{
			var assemblyFullName = aqn.AssemblyFullName ();
			var ix = assemblyFullName.IndexOf ("Culture=", StringComparison.InvariantCulture);
			if (ix == -1)
				return CultureInfo.InvariantCulture;

			var culture = assemblyFullName.Substring (ix + 8, assemblyFullName.IndexOf (',', ix + 8) - (ix + 8));
			if (culture == "neutral")
				return CultureInfo.InvariantCulture;

			return CultureInfo.GetCultureInfo (culture);
			}

		public static string AssemblyPublicKeyToken (this string aqn)
			{
			var assemblyFullName = aqn.AssemblyFullName ();
			var ix = assemblyFullName.IndexOf ("PublicKeyToken=", StringComparison.InvariantCulture);
			if (ix == -1)
				return null;

			return assemblyFullName.Substring (ix + 15);
			}

		public static AssemblyName AssemblyName (this string aqn)
			{
			var an = new AssemblyName {Name = aqn.AssemblySimpleName (), CultureInfo = aqn.AssemblyCultureInfo (), Version = aqn.AssemblyVersion (),};

			var apkt = aqn.AssemblyPublicKeyToken ();
			if (apkt != null)
				an.SetPublicKeyToken (apkt.HexToBytes ());

			return an;
			}

		public static void SplitFullyQualifiedTypeName (string fullyQualifiedTypeName, out string typeName, out string assemblyName)
			{
			int? assemblyDelimiterIndex = GetAssemblyDelimiterIndex (fullyQualifiedTypeName);

			if (assemblyDelimiterIndex != null)
				{
				typeName = fullyQualifiedTypeName.Substring (0, assemblyDelimiterIndex.GetValueOrDefault ()).Trim ();
				assemblyName =
					fullyQualifiedTypeName.Substring (assemblyDelimiterIndex.GetValueOrDefault () + 1,
						fullyQualifiedTypeName.Length - assemblyDelimiterIndex.GetValueOrDefault () - 1).Trim ();
				}
			else
				{
				typeName = fullyQualifiedTypeName;
				assemblyName = null;
				}
			}

		private static int? GetAssemblyDelimiterIndex (string fullyQualifiedTypeName)
			{
			// we need to get the first comma following all surrounded in brackets because of generic types
			// e.g. System.Collections.Generic.Dictionary`2[[System.String, mscorlib,Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
			int scope = 0;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
				{
				char current = fullyQualifiedTypeName[i];
				switch (current)
					{
					case '[':
						scope++;
						break;
					case ']':
						scope--;
						break;
					case ',':
						if (scope == 0)
							return i;
						break;
					}
				}

			return null;
			}

		public static CType GetInterface (this Type type, string name)
			{
			return type.GetInterface (name, false);
			}

		public static CType GetInterface (this Type type, string name, bool ignoreCase)
			{
			var comp = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
			return type.GetCType ().GetInterfaces ().FirstOrDefault (iface => iface.Name.Equals (name, comp));
			}

		public static Type MakeArrayType (this Type type)
			{
			return Array.CreateInstance (type, 0).GetType ();
			}

		public static Type MakeArrayType (this Type type, int rank)
			{
			return Array.CreateInstance (type, new int[rank]).GetType ();
			}

		public static bool IsSerializable (this Type type)
			{
			if (type.GetCustomAttributes (typeof(SerializableAttribute), false).Length != 0)
				return true;

			for (Type t = type.UnderlyingSystemType; t != null; t = t.BaseType)
				{
				if ((t == typeof(Enum)) || (t == typeof(Delegate)))
					return true;
				}

			return false;
			}

		public static readonly Type[] EmptyTypes = new Type[0];

		public static bool IsEquivalentTo (this Type type, Type other)
			{
			return (type == other);
			}

		public static bool IsEquivalentTo (this Type type, CType other)
			{
			return (type == other);
			}

		public static Type GetEnumUnderlyingType (this Type type)
			{
			if (!type.IsEnum)
				throw new ArgumentException ("Must be an enum type", "type");

			var size = Marshal.SizeOf (type);
			switch (size)
				{
				case 1:
					return typeof(byte);
				case 2:
					return typeof(short);
				case 4:
					return typeof(int);
				case 8:
					return typeof(long);
				}

			throw new InvalidOperationException ("unknow enum type");
			}

		public static bool IsConstructedGenericType (this Type type)
			{
			return type.IsGenericType && !type.IsGenericTypeDefinition;
			}

		[ComVisible (true)]
		public static InterfaceMapping GetInterfaceMap (this Type type, Type interfaceType)
			{
			//if (!type.IsSystemType)
			//	throw new NotSupportedException ("Derived classes must provide an implementation.");
			if (interfaceType == null)
				throw new ArgumentNullException ("interfaceType");
			//if (!interfaceType.IsSystemType)
			//	throw new ArgumentException ("interfaceType", "Type is an user type");
			InterfaceMapping res;
			if (!interfaceType.IsInterface)
				throw new ArgumentException ("Argument must be an interface.", "interfaceType");
			if (type.IsInterface)
				throw new ArgumentException ("'this' type cannot be an interface itself");
			if (type.IsGenericParameter)
				throw new InvalidOperationException ("'this' type cannot be a generic parameter");
			res.TargetType = type;
			res.InterfaceType = interfaceType;
			GetInterfaceMapData (res.TargetType, res.InterfaceType, out res.TargetMethods, out res.InterfaceMethods);
			if (res.TargetMethods == null)
				throw new ArgumentException ("Interface not found", "interfaceType");

			return res;
			}

		private static void GetInterfaceMapData (CType type, CType interfaceType, out MethodInfo[] targetMethods, out MethodInfo[] interfaceMethods)
			{
			targetMethods = type.GetMethods ().Where (mi => interfaceType.IsAssignableFrom (mi.DeclaringType)).ToArray ();
			interfaceMethods = interfaceType.GetMethods ();
			}
		}
	}
