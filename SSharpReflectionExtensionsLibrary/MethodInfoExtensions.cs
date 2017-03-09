#region License
/*
 * MethodInfoExtensions.cs
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

namespace Crestron.SimplSharp.Reflection
	{
	public static class MethodInfoExtensions
		{
		public static object CreateDelegate (this MethodInfo mi, Type delegateType)
			{
			return CDelegate.CreateDelegate (delegateType, null, mi);
			}

		public static object CreateDelegate (this MethodInfo mi, CType delegateType)
			{
			return CDelegate.CreateDelegate (delegateType, null, mi);
			}

		public static object CreateDelegate (this MethodInfo mi, Type delegateType, object target)
			{
			return CDelegate.CreateDelegate (delegateType, target, mi);
			}

		public static object CreateDelegate (this MethodInfo mi, CType delegateType, object target)
			{
			return CDelegate.CreateDelegate (delegateType, target, mi);
			}
		}
	}