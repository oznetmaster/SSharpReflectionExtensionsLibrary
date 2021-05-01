﻿#region License
/*
 * CTypeExtensions.cs
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

using System.Linq;
using System;
// For Basic SIMPL# Classes

namespace Crestron.SimplSharp.Reflection
	{
	public static class CTypeExtensions
		{
		public static CType GetCType (this object obj)
			{
			return obj.GetType ();
			}

		public static CType GetCType (this CType ctype)
			{
			return ctype;
			}

		public static CType ctypeof<T> ()
			{
			return (typeof (T));
			}

		public static CType[] GetTypeArray (object[] args)
			{
			if (args == null)
				return CTypeEmptyArray;

			return args.Select (a => a == null ? (CType)typeof(object) : (a is Type ? (CType)typeof(CType) : a.GetCType ())).ToArray ();
			}

		public static CType[] MakeTypeArray (Type[] types)
			{
			return types.Select (t => (CType)t).ToArray ();
			}

		public static readonly CType[] CTypeEmptyArray = new CType[0];

		public static bool IsRestricted (this CType ctype)
			{
			return false;
			}

		public static TypeCode GetTypeCode (this CType ctype)
			{
			return CType.GetTypeCode (ctype);
			}

		public static MethodInfo MakeGenericMethod (this MethodInfo mi, params Type[] typeArguments)
			{
			return mi.MakeGenericMethod (typeArguments.GetCTypes ());
			}

		public static string AssemblyFullName (this CType ctype)
			{
			return ((Type)ctype).AssemblyFullName ();
			}

		public static Version AssemblyVersion (this CType ctype)
			{
			return ((Type)ctype).AssemblyVersion ();
			}

		public static ConstructorInfo GetConstructor (this CType ctype, Type[] types)
			{
			return ctype.GetConstructor (MakeTypeArray (types));
			}

		public static MethodInfo GetMethod (this CType ctype, string name, Type[] types)
			{
			return ctype.GetMethod (name, MakeTypeArray (types));
			}

		public static MemberInfo[] GetMember (this CType type, string name, MemberTypes memberType, BindingFlags bindingAttr)
			{
			return type.GetMember (name, bindingAttr).Where (m => m.MemberType == memberType).ToArray ();
			}

		public static MethodInfo GetGenericMethod (this CType ctype, string name)
			{
			var members = ctype.GetMember (name).Where (m => m.MemberType == MemberTypes.Method && ((MethodInfo)m).IsGenericMethod).ToArray ();
			if (members.Length == 0)
				return null;
			if (members.Length == 1)
				return (MethodInfo)members[0];
			throw new AmbiguousMatchException ("name is not unique", null);
			}

		public static MethodInfo GetGenericMethod (this CType ctype, string name, string[] genericArguments, CType[] types)
			{
			if (types == null || types.Any (t => t == null))
				throw new ArgumentNullException ("types");

			if (types.Length == 0)
				return ctype.GetGenericMethod (name);

			var members = ctype.GetMember (name).Where (m => m.MemberType == MemberTypes.Method && ((MethodInfo)m).IsGenericMethod).ToArray ();
			if (members.Length == 0)
				return null;

			var methods = members.Cast<MethodInfo> ().Where (m => MatchParameters (m, genericArguments, types)).ToArray ();
			if (methods.Length == 0)
				return null;
			if (methods.Length == 1)
				return methods[0];
			throw new AmbiguousMatchException ("name is not unique", null);
			}

		public static MethodInfo GetGenericMethod (this CType ctype, string name, BindingFlags bindingAttr)
			{
			var members = ctype.GetMember (name, bindingAttr).Where (m => m.MemberType == MemberTypes.Method && ((MethodInfo)m).IsGenericMethod).ToArray ();
			if (members.Length == 0)
				return null;
			if (members.Length == 1)
				return (MethodInfo)members[0];
			throw new AmbiguousMatchException ("name is not unique", null);
			}

		public static MethodInfo GetGenericMethod (this CType ctype, string name, BindingFlags bindingAttr, string[] genericArguments, CType[] types)
			{
			if (types == null || types.Any (t => t == null))
				throw new ArgumentNullException ("types");

			if (types.Length == 0)
				return ctype.GetGenericMethod (name, bindingAttr);

			var members = ctype.GetMember (name, bindingAttr).Where (m => m.MemberType == MemberTypes.Method && ((MethodInfo)m).IsGenericMethod).ToArray ();
			if (members.Length == 0)
				return null;

			var methods = members.Cast<MethodInfo> ().Where (m => MatchParameters (m, genericArguments, types)).ToArray ();
			if (methods.Length == 0)
				return null;
			if (methods.Length == 1)
				return methods[0];
			throw new AmbiguousMatchException ("name is not unique", null);
			}

		private static bool MatchParameters (MethodInfo mi, string[] genericArguments, CType[] types)
			{
			bool typeContainsGeneric = types.Any (t => t.IsGenericParameter);

			if (!mi.IsGenericMethodDefinition && !mi.ContainsGenericParameters && !typeContainsGeneric)
				return mi.GetParameters ().Select (p => p.ParameterType).SequenceEqual (types);

			if (mi.IsGenericMethodDefinition || mi.ContainsGenericParameters)
				return mi.GetGenericArguments ().Where (p => p.IsGenericParameter).Select (p => p.Name).SequenceEqual (genericArguments);

			return false;
			}

		public static Type MakeArrayType (this CType type)
			{
			return Array.CreateInstance (type, 0).GetType ();
			}

		public static Type MakeArrayType (this CType type, int rank)
			{
			return Array.CreateInstance (type, new int[rank]).GetType ();
			}

		public static Type[] GetTypes (this CType[] ctypes)
			{
			int len = ctypes.Length;
			var types = new Type[len];
			for (int ix = 0; ix < len; ++ix)
				types[ix] = ctypes[ix];
			return types;
			}

		public static CType GetInterface (this CType ctype, string name, bool ignoreCase)
			{
			var ldot = name.LastIndexOf ('.');
			var ns = ldot == -1 ? string.Empty : name.Substring (0, ldot); 
			var na = ldot == -1 ? name : name.Substring (ldot + 1);

			var interfaces = ctype.GetInterfaces ();
			foreach (var @interface in interfaces)
				{
				if (ns.Length != 0 && !@interface.Namespace.Equals (ns, StringComparison.InvariantCulture))
					continue;

				if (@interface.Name.Equals (na, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture))
						return @interface;
				}

			return null;
			}

		public static CType GetInterface (this CType ctype, string name)
			{
			return GetInterface (ctype, name, false);
			}

		public static bool IsEquivalentTo (this CType ctype, CType other)
			{
			return (ctype == other);
			}

		public static bool IsEquivalentTo (this CType ctype, Type other)
			{
			return (ctype == other);
			}
		}
	}
