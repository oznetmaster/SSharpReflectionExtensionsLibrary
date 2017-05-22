using System;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       				// For Basic SIMPL#Pro classes
using Crestron.SimplSharp.Reflection;

using NUnit.Framework;

namespace SSharpReflectionExtensionsTests
	{
	[TestFixture]
	public class AssemblyExTests
		{
		private Assembly ReturnCallingAssembly ()
			{
			return AssemblyEx.GetCallingAssembly ();
			}

		[Test]
		public void GetCallingAssembly ()
			{
			Assert.AreEqual (this.GetType ().GetCType ().Assembly, ReturnCallingAssembly (), "#1");
			}
		}
	}

