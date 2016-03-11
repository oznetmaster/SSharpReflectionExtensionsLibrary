using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp.Reflection
	{
	public static class DelegateExtensions
		{
		/// <summary>
		/// Dynamically invokes (late-bound) the method represented by the current
		/// delegate. 
		/// </summary>
		/// <param name="dlg">Extended class.</param>
		/// <param name="args">An array of objects that are the arguments to pass to the
		/// method represented by the current delegate.</param>
		/// <returns>The object returned by the method represented by the delegate.
		/// </returns>
		/// <exception cref="MemberAccessException"> The caller does not have access to the
		/// method represented by the delegate (for example, if the method is private); The
		/// number, order, or type of parameters listed in <paramref name="args"/> is invalid. </exception>
		/// <exception cref="TargetInvocationException">One of the encapsulated methods
		/// throws an exception.</exception>
		public static object DynamicInvoke (this Delegate dlg, params object[] args)
			{
			return dlg.GetMethod().Invoke (dlg.Target, BindingFlags.Default, null, args, null);
			}
		}
	}