#region License
/*
 * ReflectionUtilities.cs
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
using Crestron.SimplSharp.CrestronIO;

namespace Crestron.SimplSharp.Reflection
	{
	internal static class ReflectionUtilities
		{
		private static readonly List<Assembly> Assemblies;

		static ReflectionUtilities ()
			{
			Assemblies = new List<Assembly> ();

			GetAllAssemblies ();

			Cache = new Dictionary<string, TypeCacheEntry> ();
			}

		internal static string GetStackFrame (int level)
			{
			string stackTrace;
			try
				{
				throw new Exception ();
				}
			catch (Exception ex)
				{
				stackTrace = ex.StackTrace;
				}

			return GetStackFrame (stackTrace, level + 1);
			}

		internal static string GetStackFrame (string stackTrace, int level)
			{
			string[] stackFrames = stackTrace.Split ('\n');
			if (stackFrames.Length <= level)
				return null;

			string callingFrame = stackFrames[level].Trim ().Substring (3);
			return callingFrame;
			}

		internal static CType GetCallingType (int level)
			{
			return GetCallingType (GetStackFrame (level + 1));
			}

		internal static CType GetCallingType (string callingFrame)
			{
			string caller = callingFrame;
			if (caller == null)
				return null;

			string methodName = caller.Substring (0, caller.IndexOf ('('));
			string callingTypeName = methodName.Substring (0, methodName.LastIndexOf ('.'));

			CType callingType;
			while (true)
				{
				if (callingTypeName.EndsWith ("."))
					callingTypeName = callingTypeName.Substring (0, callingTypeName.Length - 1);
				callingType = Type.GetType (callingTypeName) ?? FindType (callingTypeName);
				if (callingType != null)
					break;

				int ix = callingTypeName.LastIndexOf ('.');
				if (ix == -1)
					break;

				callingTypeName = callingTypeName.Substring (0, ix);
				}

			return callingType;
			}

		private static void GetAllAssemblies ()
			{
			var path = InitialParametersClass.ProgramDirectory.ToString ();
			var dlls = Directory.GetFiles (path, "*.dll").Concat (Directory.GetFiles (path, "*.exe"));
			foreach (var dll in dlls)
				{
				Assembly assembly;
				try
					{
					assembly = Assembly.LoadFrom (dll);
					}
				catch
					{
					continue;
					}

				if (assembly == null)
					continue;

				Assemblies.Add (assembly);
				}
			}

		private static readonly Dictionary<string, Type> DictTypes = new Dictionary<string, Type>
			{
				{"bool", typeof (bool)},
				{"byte", typeof (byte)},
				{"sbyte", typeof (sbyte)},
				{"char", typeof (char)},
				{"decimal", typeof (decimal)},
				{"double", typeof (double)},
				{"float", typeof (float)},
				{"int", typeof (int)},
				{"uint", typeof (uint)},
				{"long", typeof (long)},
				{"ulong", typeof (ulong)},
				{"object", typeof (object)},
				{"short", typeof (short)},
				{"ushort", typeof (ushort)},
				{"string", typeof (string)},
				{"Boolean", typeof (Boolean)},
				{"Byte", typeof (Byte)},
				{"SByte", typeof (SByte)},
				{"Char", typeof (Char)},
				{"Decimal", typeof (Decimal)},
				{"Double", typeof (Double)},
				{"Single", typeof (Single)},
				{"Int32", typeof (Int32)},
				{"UInt32", typeof (UInt32)},
				{"Int64", typeof (Int64)},
				{"UInt64", typeof (UInt64)},
				{"Object", typeof (Object)},
				{"Int16", typeof (Int16)},
				{"UInt16", typeof (UInt16)},
				{"String", typeof (String)},
				{"DateTime", typeof (DateTime)},
				{"System.Boolean", typeof (Boolean)},
				{"System.Byte", typeof (Byte)},
				{"System.SByte", typeof (SByte)},
				{"System.Char", typeof (Char)},
				{"System.Decimal", typeof (Decimal)},
				{"System.Double", typeof (Double)},
				{"System.Single", typeof (Single)},
				{"System.Int32", typeof (Int32)},
				{"System.UInt32", typeof (UInt32)},
				{"System.Int64", typeof (Int64)},
				{"System.UInt64", typeof (UInt64)},
				{"System.Object", typeof (Object)},
				{"System.Int16", typeof (Int16)},
				{"System.UInt16", typeof (UInt16)},
				{"System.String", typeof (String)},
				{"System.DateTime", typeof (DateTime)},
			};

		private static readonly string[] SystemNamespaces = { "System.", "System.Text.", "System.Collections.", "System.Collections.Generic.", "System.Collections.ObjectModel.", "System.Globalization.", "System.Configuration.Assemblies." };

		private class TypeCacheEntry
			{
			public Type Type;
			public uint Order;
			}

		private static readonly Dictionary<string, TypeCacheEntry> Cache;
		private static uint _cacheOrder = 0;

		private const int Cachemaxsize = 1000;
		private const uint Cachemaxorder = UInt32.MaxValue - Cachemaxsize;


		internal static Type FindType (string typeName)
			{
			Type easyType;
			if (DictTypes.TryGetValue (typeName, out easyType))
				return easyType;

			lock (Cache)
				{
				TypeCacheEntry cachedType;
				if (Cache.TryGetValue (typeName, out cachedType))
					{
					if (cachedType.Order != _cacheOrder)
						{
						cachedType.Order = ++_cacheOrder;
						if (_cacheOrder > Cachemaxorder)
							ReorderCache ();
						}

					return cachedType.Type;
					}
				}

			if (!typeName.Contains ('.'))
				foreach (var sys in SystemNamespaces)
				{
				var sysName = sys + typeName;
				easyType = Type.GetType (sysName);
				if (easyType != null)
					{
					AddToCache (sysName, easyType);

					return easyType;
					}
				}

			var foundType = Assemblies.Select (assembly => assembly.GetType (typeName)).FirstOrDefault (type => type != null);

			if (foundType != null)
				AddToCache(typeName, foundType);

			return foundType;
			}

		private static void AddToCache (string typeName, Type type)
			{
			lock (Cache)
				{
				Cache[typeName] = new TypeCacheEntry { Type = type, Order = ++_cacheOrder };
				if (_cacheOrder > Cachemaxorder)
					ReorderCache ();
				}

			if (Cache.Count > Cachemaxsize)
				{
				lock (Cache)
					{
					if (Cache.Count <= Cachemaxsize)
						return;

					var orderedTypeNames = Cache.OrderBy (c => c.Value.Order).Take (Cache.Count - Cachemaxsize).Select (c => c.Key).ToArray ();
					foreach (var cachedTypeName in orderedTypeNames)
						Cache.Remove (cachedTypeName);
					}
				}
			}

		private static void ReorderCache ()
			{
			var orderedTypeNames = Cache.OrderBy (c => c.Value.Order).Select (c => c.Key).ToArray ();

			_cacheOrder = 0;

			foreach (var cachedTypeName in orderedTypeNames)
				Cache[cachedTypeName].Order = ++_cacheOrder;
			}
		}
	}