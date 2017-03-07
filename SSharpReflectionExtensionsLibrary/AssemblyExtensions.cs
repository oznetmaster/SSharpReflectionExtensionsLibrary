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
			var callingType = ReflectionUtilities.GetCallingType (3);

			return callingType == null ? null : callingType.Assembly;
			}
		}
	}