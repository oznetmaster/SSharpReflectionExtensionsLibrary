// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Crestron.SimplSharp.Reflection
	{
	public static partial class RuntimeReflectionExtensions
		{
		private static readonly BindingFlags defaultPublicFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
		private static readonly BindingFlags defaultNonPublicFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

		//=============================================================================================================
		// This group of apis are equivalent to the desktop CLR's:
		//
		//      type.Get***(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance)
		//
		// Returns all directly declared members.
		// Members from base classes are returned if they're not private, static or overridden.
		//=============================================================================================================
		public static IEnumerable<FieldInfo> GetRuntimeFields (this CType type)
			{
			return type.GetFields (defaultNonPublicFlags).AsNothingButIEnumerable ();
			}

		public static IEnumerable<FieldInfo> GetRuntimeFields (this Type type)
			{
			return type.GetFields (defaultNonPublicFlags).AsNothingButIEnumerable ();
			}

		public static IEnumerable<MethodInfo> GetRuntimeMethods (this CType type)
			{
			return type.GetMethods (defaultNonPublicFlags).AsNothingButIEnumerable ();
			}

		public static IEnumerable<MethodInfo> GetRuntimeMethods (this Type type)
			{
			return type.GetMethods (defaultNonPublicFlags).AsNothingButIEnumerable ();
			}

		public static IEnumerable<PropertyInfo> GetRuntimeProperties (this CType type)
			{
			return type.GetProperties (defaultNonPublicFlags).AsNothingButIEnumerable ();
			}

		public static IEnumerable<PropertyInfo> GetRuntimeProperties (this Type type)
			{
			return type.GetProperties (defaultNonPublicFlags).AsNothingButIEnumerable ();
			}

		public static IEnumerable<EventInfo> GetRuntimeEvents (this CType type)
			{
			return type.GetEvents (defaultNonPublicFlags).AsNothingButIEnumerable ();
			}

		public static IEnumerable<EventInfo> GetRuntimeEvents (this Type type)
			{
			return type.GetEvents (defaultNonPublicFlags).AsNothingButIEnumerable ();
			}

		//=============================================================================================================
		// This group of apis are equivalent to the desktop CLR's:
		//
		//      type.Get***(name, BindingFlags.Public|BindingFlags.Static|BindingFlags.Instance)
		//
		// Note that unlike the GetRuntime***() apis defined on this same class, non-public members are excluded from the search.
		//
		// Searches all *public* directly declared and inherited members. If there's a multiple match,
		// the most derived one wins.
		//
		// throws AmbiguousMatchException() if the names two members declared by the same class.
		//=============================================================================================================

		public static FieldInfo GetRuntimeField (this CType type, String name)
			{
			return type.GetField (name, defaultPublicFlags);
			}

		public static FieldInfo GetRuntimeField (this Type type, String name)
			{
			return type.GetField (name, defaultPublicFlags);
			}

		public static MethodInfo GetRuntimeMethod (this CType type, String name, CType[] parameters)
			{
			return type.GetMethod (name, parameters);
			}

		public static MethodInfo GetRuntimeMethod (this Type type, String name, CType[] parameters)
			{
			return type.GetMethod (name, parameters);
			}

		public static PropertyInfo GetRuntimeProperty (this CType type, String name)
			{
			return type.GetProperty (name, defaultPublicFlags);
			}

		public static PropertyInfo GetRuntimeProperty (this Type type, String name)
			{
			return type.GetProperty (name, defaultPublicFlags);
			}

		public static EventInfo GetRuntimeEvent (this CType type, String name)
			{
			return type.GetEvent (name, defaultPublicFlags);
			}

		public static EventInfo GetRuntimeEvent (this Type type, String name)
			{
			return type.GetEvent (name, defaultPublicFlags);
			}

		/*
		private static M MostDerived<M> (this IEnumerable<M> members) where M : MemberInfo
			{
			IEnumerator<M> enumerator = members.GetEnumerator ();
			if (!enumerator.MoveNext ())
				return null;
			M result = enumerator.Current;
			if (!enumerator.MoveNext ())
				return result;
			M anotherResult = enumerator.Current;
			if (anotherResult.DeclaringType.Equals (result.DeclaringType))
				throw new AmbiguousMatchException ();
			return result;
			}
		*/

		//======================================================================================
		// For virtual non-new slot methods, returns the least derived method that occupies the slot.
		// For everything else, return "this".
		//======================================================================================
		public static MethodInfo GetRuntimeBaseDefinition (this MethodInfo method)
			{
			return method.GetBaseDefinition ();
			}

		//======================================================================================

		public static InterfaceMapping GetRuntimeInterfaceMap (this TypeInfo typeInfo, Type interfaceType)
			{
			throw new PlatformNotSupportedException ("InterfaceMapping not supported");
			}

		//======================================================================================

		public static MethodInfo GetMethodInfo (this Delegate del)
			{
			return del.GetMethod ();
			}
		}

	public struct InterfaceMapping
		{
		internal InterfaceMapping (MethodInfo[] interfaceMethods, MethodInfo[] targetMethods, CType interfaceType, CType targetType)
			{
			this.InterfaceMethods = interfaceMethods;
			this.TargetMethods = targetMethods;
			this.InterfaceType = interfaceType;
			this.TargetType = targetType;
			}

		public MethodInfo[] InterfaceMethods;
		public MethodInfo[] TargetMethods;
		public CType InterfaceType;
		public CType TargetType;
		}
	}

namespace System.Collections.Generic
	{
	internal static class EnumerableExtensions
		{
		// Used to prevent returning values out of IEnumerable<>-typed properties
		// that an untrusted caller could cast back to array or List.
		public static IEnumerable<T> AsNothingButIEnumerable<T> (this IEnumerable<T> en)
			{
			foreach (T t in en)
				yield return t;
			}
		}
	}
