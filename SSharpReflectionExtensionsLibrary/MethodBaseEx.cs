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
using System.Linq;

namespace Crestron.SimplSharp.Reflection
	{
	public static class MethodBaseEx
		{
		public static MethodBase GetCurrentMethod ()
			{
			return GetStackMethod (1);
			}

		public static MethodBase GetStackMethod (int level)
			{
			if (level < 0)
				throw new ArgumentOutOfRangeException ("level");

			var stackFrame = ReflectionUtilities.GetStackFrame (level + 1);

			return GetMethodFromStackFrame (stackFrame);
			}

		private static MethodBase  GetMethodFromStackFrame (string stackFrame)
			{
			string[] genericArguments;

			var callingType = ReflectionUtilities.GetCallingType (stackFrame);
			if (callingType == null)
				return null;

			var ix = stackFrame.IndexOf ('(');
			string fullMethodName = stackFrame.Substring (0, ix);
			string methodName = fullMethodName.Substring (fullMethodName.LastIndexOf ('.') + 1);
			if (methodName[methodName.Length - 1] == ']')
				{
				var iy = methodName.IndexOf ('[');
				genericArguments = methodName.Substring (iy + 1, methodName.Length - 1 - iy - 1).Split (',');
				methodName = methodName.Substring (0, iy);
				}
			else
				genericArguments = new string[0];

			string parString = stackFrame.Substring (ix + 1, stackFrame.Length - ix - 2);

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
				if (genericArguments.Length != 0)
					return callingType.GetGenericMethod (methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

				return callingType.GetMethod (methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
				}

			if (genericArguments.Length != 0) 
				return callingType.GetGenericMethod (methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, genericArguments, types);

			return callingType.GetMethod (methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, types, null);
			}

		public static string GetCurrentMethodNameWithParameters ()
			{
			return GetStackMethodNameWithParameters (1);
			}

		public static string GetStackMethodNameWithParameters (int level)
			{
			return ReflectionUtilities.GetStackFrame (level + 1);
			}

		public static string GetCurrentMethodName ()
			{
			return GetStackMethodName (1);
			}

		public static string GetStackMethodName (int level)
			{
			var stackFrame = ReflectionUtilities.GetStackFrame (level + 1);
			var ix = stackFrame.IndexOf ('(');
			return stackFrame.Substring (0, ix);
			}

		public static MethodBase GetCallingMethod ()
			{
			return GetStackMethod (2);         
			}

		public static string GetCallingMethodNameWithParameters ()
			{
			return GetStackMethodNameWithParameters (2);
			}

		public static string GetCallingMethodName ()
			{
			return GetStackMethodName (2);
			}

		public static MethodBase GetExceptionMethod (Exception ex)
			{
			if (ex == null)
				throw new ArgumentNullException ("ex");

			var stackFrame = ReflectionUtilities.GetStackFrame (ex.StackTrace, 0);

			return GetMethodFromStackFrame (stackFrame);
			}

		public static string GetExceptionMethodName (Exception ex)
			{
			if (ex == null)
				throw new ArgumentNullException ("ex");

			var stackFrame = ReflectionUtilities.GetStackFrame (ex.StackTrace, 0);
			var ix = stackFrame.IndexOf ('(');

			return stackFrame.Substring (0, ix);
			}

		public static string GetExceptionMethodNameAndParameters (Exception ex)
			{
			if (ex == null)
				throw new ArgumentNullException ("ex");

			return ReflectionUtilities.GetStackFrame (ex.StackTrace, 0);
			}
		}
	}