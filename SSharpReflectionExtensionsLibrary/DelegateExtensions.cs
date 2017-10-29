#region License
/*
 * DelegateExtensions.cs
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
using Crestron.SimplSharp.Reflection;
using IAsyncResult = Crestron.SimplSharp.CrestronIO.IAsyncResult;
using AsyncCallback = Crestron.SimplSharp.CrestronIO.AsyncCallback;
using WaitCallback = Crestron.SimplSharp.CrestronSharpHelperDelegate;

namespace Crestron.SimplSharp
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
		public static TResult DynamicInvoke<TResult> (this Delegate dlg, params object[] args)
			{
			return (TResult)dlg.GetMethod().Invoke (dlg.Target, BindingFlags.Default, null, args, null);
			}

		private class InvokeInfo
			{
			public AsyncResult result;
			public AsyncCallback callback;
			public object state;
			public object[] args;
			}

		private delegate bool DelQueueUserWorkItem (WaitCallback callback, object state);
		private static DelQueueUserWorkItem _delQueueUserWorkItem;

		static DelegateExtensions ()
			{
			if (CrestronEnvironment.RuntimeEnvironment == eRuntimeEnvironment.SimplSharpPro)
				{
				try
					{
					var typeMonoThreadPool = Type.GetType ("SSMono.Threading.ThreadPool, SSMonoProThreadingLibrary", false).GetCType ();
					if (typeMonoThreadPool != null)
						_delQueueUserWorkItem = (DelQueueUserWorkItem)CDelegate.CreateDelegate (typeof (DelQueueUserWorkItem), null,
							typeMonoThreadPool.GetMethod ("QueueUserWorkItem", new CType[] {typeof (WaitCallback), typeof (object)}));
					}
				catch
					{
					}
				}
			}

		public static IAsyncResult BeginInvokeEx (this Delegate dlg, AsyncCallback callback, object obj, params object[] args)
			{
			var iar = new AsyncResult (dlg, obj);
			var invokeInfo = new InvokeInfo {result = iar, callback = callback, state = obj, args = args};

			if (_delQueueUserWorkItem != null)
				_delQueueUserWorkItem (DoDelegate, invokeInfo);
			else
				CrestronInvoke.BeginInvoke (DoDelegate, invokeInfo);
			return iar;
			}

		private static void DoDelegate (object state)
			{
			var invokeInfo = (InvokeInfo)state;

			try
				{
				invokeInfo.result.Result = invokeInfo.result.AsyncDelegate.DynamicInvoke (invokeInfo.args);
				}
			catch (TargetInvocationException tiex)
				{
				invokeInfo.result.Exception = tiex.InnerException;
				}

			invokeInfo.result.IsCompleted = true;
			((CEvent)invokeInfo.result.AsyncWaitHandle).Set ();
			if (invokeInfo.callback != null)
				invokeInfo.callback (invokeInfo.result);
			}

		public static object EndInvokeEx (this Delegate dlg, IAsyncResult result)
			{
			var asyncResult = result as AsyncResult;
			if (asyncResult == null)
				throw new ArgumentException ("invalid IAsyncResult", "result");

			if (asyncResult.EndInvokeCalled)
				throw new InvalidOperationException ("EndInvoke can only be called once");
			asyncResult.EndInvokeCalled = true;

			if (!asyncResult.CompletedSynchronously)
				asyncResult.AsyncWaitHandle.Wait ();

			if (asyncResult.Exception != null)
				throw asyncResult.Exception;

			return asyncResult.Result;
			}

		public static TResult EndInvokeEx<TResult> (this Delegate dlg, IAsyncResult result)
			{
			return (TResult)dlg.EndInvokeEx (result);
			}
		}

	public static class DelegateEx
		{
		public static Delegate Combine (params Delegate[] delegates)
			{
			if (delegates == null || delegates.Length == 0 || delegates.All (del => del == null))
				return null;

			if (delegates.Select (del => del.GetType ()).Distinct ().Count () != 1)
				throw new ArgumentException ("All delegates must be of the same type");

			var newDel = delegates[0];

			for (int ix = 1; ix < delegates.Length; ++ix)
				newDel = Delegate.Combine (newDel, delegates[ix]);

			return newDel;
			}
		}
	}