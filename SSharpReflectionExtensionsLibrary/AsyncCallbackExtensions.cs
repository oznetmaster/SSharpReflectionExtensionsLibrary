#region License
/*
 * AsyncCallbackExtensions.cs
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

namespace Crestron.SimplSharp.CrestronIO
	{
	public static class AsyncCallbackExtensions
		{
		private class AsyncCallbackState
			{
			public IAsyncResult IAsyncResult;
			public AsyncCallback Callback;
			public AsyncResult Result;

			public AsyncCallbackState (IAsyncResult iAsyncResult, AsyncCallback callback, AsyncResult result)
				{
				IAsyncResult = iAsyncResult;
				Callback = callback;
				Result = result;
				}
			}

		public static IAsyncResult BeginInvokeEx (this AsyncCallback cb, IAsyncResult asyncResult, AsyncCallback callback, object @object)
			{
			var newAr = new AsyncResult (cb, @object);
			var acs = new AsyncCallbackState (asyncResult, callback, newAr);

			CrestronInvoke.BeginInvoke (DoCallback, acs);

			return newAr;
			}

		public static void EndInvokeEx (this AsyncCallback cb, IAsyncResult asyncResult)
			{
			var ar = (AsyncResult)asyncResult;

			if (ar.EndInvokeCalled)
				throw new InvalidOperationException ("EndInvoke already called");

			ar.EndInvokeCalled = true;

			if (!ar.CompletedSynchronously)
				ar.AsyncWaitHandle.Wait ();

			if (ar.Exception != null)
				throw ar.Exception;
			}

		private static void DoCallback (object state)
			{
			var acs = (AsyncCallbackState)state;
			var newAr = acs.Result;

			try
				{
				((AsyncCallback)newAr.AsyncDelegate) (acs.IAsyncResult);
				}
			catch (Exception ex)
				{
				newAr.Exception = ex;
				}

			((CEvent)newAr.AsyncWaitHandle).Set ();
			newAr.IsCompleted = true;

			if (acs.Callback != null)
				acs.Callback (newAr);
			}
		}
	}