#region License
/*
 * AssemblyExtensions.cs
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
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.CrestronXmlLinq;
using Crestron.SimplSharp.CrestronXml;
using System.Text;
using System.Collections.Generic;

namespace Crestron.SimplSharp.Reflection
	{
	public static class AssemblyEx
		{
		public static Assembly GetEntryAssembly ()
			{
			string appDir = InitialParametersClass.ProgramDirectory.ToString ();

			if (CrestronEnvironment.RuntimeEnvironment == eRuntimeEnvironment.SIMPL)
				return null;

			var proginfo = File.ReadToEnd (Path.Combine (appDir, "ProgramInfo.config"), Encoding.UTF8);
			var doc = XDocument.Parse ("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n" + proginfo);
			var entry = doc.Descendants ("EntryPoint").FirstOrDefault ();
			if (entry == null)
				return null;

			return Assembly.Load (entry.Value);
			}

		public static Assembly GetCallingAssembly ()
			{
			var callingType = ReflectionUtilities.GetCallingType (2);

			return callingType == null ? null : callingType.Assembly;
			}
		}
	}