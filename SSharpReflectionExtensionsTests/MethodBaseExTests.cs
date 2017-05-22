using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;

using NUnit.Framework;

namespace SSharpReflectionExtensionsTests
	{
	[TestFixture]
	public class MethodBaseExTests
		{
		private MethodBase ReturnCallingMethod ()
			{
			return MethodBaseEx.GetCallingMethod ();
			}

		public void GetCallingMethodWithOverload (int i)
			{
			}

		public void ThrowException ()
			{
			throw new InvalidOperationException ("Test InvalidOperation");
			}

		private class TestClass
			{
			public int Number;
			}

		private static TestClass[] TestClassSource = new TestClass[] { new TestClass { Number = 1 } };

		[Test]
		public void GetCallingMethod ()
			{
			var rslt = ReturnCallingMethod ();
			Assert.IsNotNull (rslt, "#1");
			Assert.AreEqual ((MethodBase)this.GetType ().GetCType ().GetMethod ("GetCallingMethod"), rslt, "#2");
			}

		[Test]
		public void GetCallingMethodWithOverload ()
			{
			var rslt = ReturnCallingMethod ();
			Assert.IsNotNull (rslt, "#1");
			Assert.AreEqual ((MethodBase)this.GetType ().GetCType ().GetMethod ("GetCallingMethodWithOverload", CTypeExtensions.CTypeEmptyArray), ReturnCallingMethod (), "#2");
			}

		public void GetCurrentMethodWithOverload (int i)
			{
			}

		[Test]
		public void GetCurrentMethod ()
			{
			var rslt = MethodBaseEx.GetCurrentMethod ();
			Assert.IsNotNull (rslt, "#1");
			Assert.AreEqual ((MethodBase)this.GetType ().GetCType ().GetMethod ("GetCurrentMethod"),  rslt, "#2");
			}

		[Test]
		public void GetCurrentMethodWithOverload ()
			{
			var rslt = MethodBaseEx.GetCurrentMethod ();
			Assert.IsNotNull (rslt, "#1");
			Assert.AreEqual ((MethodBase)this.GetType ().GetCType ().GetMethod ("GetCurrentMethodWithOverload", CTypeExtensions.CTypeEmptyArray), rslt,"#2");
			}

		[TestCase (1)]
		[TestCase ("abc")]
		[TestCase (false)]
		[TestCaseSource ("TestClassSource")]
		public void GetCurrentMethodGeneric<T> (T t)
			{
			var rslt = MethodBaseEx.GetCurrentMethod ();
			Assert.IsNotNull (rslt, "#1");
			Assert.AreEqual (((MethodInfo)(this.GetType ().GetCType ().GetGenericMethod ("GetCurrentMethodGeneric", new string[] {"T"}, new CType[] {typeof (T)} ))), rslt, "#2");
			}

		[Test]
		public void GetExceptionMethod ()
			{
			Exception exception = null;

			try
				{
				ThrowException ();
				}
			catch (Exception ex)
				{
				exception = ex;
				}

			var rslt = MethodBaseEx.GetExceptionMethod (exception);
			Assert.IsNotNull (rslt, "#1");
			Assert.AreEqual ((MethodBase)this.GetType ().GetCType ().GetMethod ("ThrowException"), rslt, "#2");
			}
		}
	}