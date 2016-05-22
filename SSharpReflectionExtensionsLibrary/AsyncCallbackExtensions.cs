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