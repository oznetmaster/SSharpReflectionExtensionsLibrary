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
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;

namespace Crestron.SimplSharp.Reflection
	{
	internal static class ReflectionUtilities
		{
		private static readonly List<Assembly> assemblies;
		static ReflectionUtilities ()
			{
			assemblies = new List<Assembly> ();

			GetAllAssemblies ();
			}

		internal static string GetCaller (int level)
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
			string[] stackFrames = stackTrace.Split ('\n');
			if (stackFrames.Length <= level)
				return null;

			string callingFrame = stackFrames[level].Trim ().Substring (3);
			return callingFrame;
			}

		internal static CType GetCallingType (int level)
			{
			return GetCallingType (GetCaller (level));
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

				assemblies.Add (assembly);
				}
			}
		internal static Dictionary<string, Type> dictTypes = new Dictionary<string, Type>
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
			};

		internal static Type FindType (string typeName)
			{
			Type easyType;
			if (dictTypes.TryGetValue (typeName, out easyType))
				return easyType;
			if (!typeName.Contains ('.'))
				typeName = "System." + typeName;
			easyType= Type.GetType (typeName);
			if (easyType != null)
				return easyType;

			return assemblies.Select (assembly => assembly.GetType (typeName)).FirstOrDefault (type => type != null);
			}

		}
	}