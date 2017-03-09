#region License
/*
 * MethodBaseEx.cs
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
	public static class MethodBaseEx
		{
		public static MethodBase GetCurrentMethod ()
			{
			return GetStackMethod (0);
			}

		public static MethodBase GetStackMethod (int level)
			{
			if (level < 0)
				throw new ArgumentOutOfRangeException ("level");

			var caller = ReflectionUtilities.GetCaller (level + 2);
			var callingType = ReflectionUtilities.GetCallingType (caller);
			if (callingType == null)
				return null;

			var ix = caller.IndexOf ('(');
			string fullMethodName = caller.Substring (0, ix);
			string methodName = fullMethodName.Substring (fullMethodName.LastIndexOf ('.') + 1);
			if (methodName[methodName.Length - 1] == ']')
				methodName = methodName.Substring (0, methodName.IndexOf ('['));
			string parString = caller.Substring (ix + 1, caller.Length - ix - 2);

			CType[] types;
			if (parString.Length != 0)
				{
				string[] pars = parString.Split (',');
				types = pars.Select (p => (CType)ReflectionUtilities.FindType (p.Split (' ')[0])).ToArray ();
				}
			else
				types = CTypeExtensions.CTypeEmptyArray;

			if (types.Any (t => t == null))
				{
				var members = callingType.GetMember (methodName);
				var methods = members.Where (m => m.MemberType == MemberTypes.Method).ToArray ();

				if (methods.Length != 1)
					return null;

				return (MethodBase)methods[1];
				}

			return callingType.GetMethod (methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, types, null);
			}

		public static string GetCurrentMethodNameWithParameters ()
			{
			return GetStackMethodNameWithParameters (0);
			}

		public static string GetStackMethodNameWithParameters (int level)
			{
			return ReflectionUtilities.GetCaller (level + 2);
			}

		public static string GetCurrentMethodName ()
			{
			return GetStackMethodName (0);
			}

		public static string GetStackMethodName (int level)
			{
			var caller = ReflectionUtilities.GetCaller (level + 2);
			var ix = caller.IndexOf ('(');
			return caller.Substring (0, ix);
			}

		public static MethodBase GetCallingMethod ()
			{
			return GetStackMethod (1);         
			}

		public static string GetCallingMethodNameWithParameters ()
			{
			return GetStackMethodNameWithParameters (1);
			}

		public static string GetCallingMethodName ()
			{
			return GetStackMethodName (1);
			}
		}
	}