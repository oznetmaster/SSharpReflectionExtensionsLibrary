using System.Linq;
using System;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.CrestronXmlLinq;
using Crestron.SimplSharp.CrestronXml;
using System.Text;

namespace Crestron.SimplSharp.Reflection
	{
	public class AssemblyEx
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

		private static string GetCaller ()
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
			if (stackFrames.Length < 4)
				return null;

			string callingFrame = stackFrames[3].Trim ().Substring (3);
			return callingFrame;
			}

		public static Assembly GetCallingAssembly ()
			{
			string caller = GetCaller ();
			string methodName = caller.Substring (0, caller.IndexOf ('('));
			string callingTypeName = methodName.Substring (0, methodName.LastIndexOf ('.'));
			if (callingTypeName.EndsWith ("."))
				callingTypeName = callingTypeName.Substring (0, callingTypeName.Length - 1);
			CType callingType = Type.GetType (callingTypeName) ?? FindType (callingTypeName);
			return callingType == null ? null : callingType.Assembly;
			}

		private static Type FindType (string typeName)
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
				var type = assembly.GetType (typeName);
				if (type != null)
					return type;
				}

			return null;
			}
		}
	}