using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp.CrestronIO
	{
	public class AsyncResult : IAsyncResult
		{
		private readonly CEvent cevent = new CEvent (false, false);

		#region IAsyncResult Members

		public object AsyncState
			{
			get;
			internal set;
			}

		public bool CompletedSynchronously
			{
			get;
			internal set;
			}

		public bool IsCompleted
			{
			get;
			internal set;
			}

		public CEventHandle AsyncWaitHandle
			{
			get { return cevent; }
			}

		public object InnerObject
			{
			get { throw new NotImplementedException (); }
			}

		#endregion

		public bool EndInvokeCalled
			{
			get;
			internal set;
			}

		public Delegate AsyncDelegate
			{
			get;
			internal set;
			}

		internal object Result
			{
			get;
			set;
			}

		internal AsyncResult (Delegate asyncDelegate, object obj)
			{
			AsyncDelegate = asyncDelegate;
			AsyncState = obj;
			}
		}
	}