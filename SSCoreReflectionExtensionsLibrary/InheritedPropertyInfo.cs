// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Crestron.SimplSharp.Reflection;
//using System.Diagnostics;
using System.Collections.Generic;
using CustomAttributeData = System.Attribute;

namespace Internal.Reflection.Extensions.NonPortable
	{
	//
	// This class exists for desktop compatibility. If one uses an api such as Type.GetProperty(string) to retrieve a member
	// from a base class, the desktop returns a special MemberInfo object that is blocked from seeing or invoking private
	// set or get methods on that property. That is, the type used to find the member is part of that member's object identity.
	//
	internal sealed class InheritedPropertyInfo : PropertyInfo
		{
		private readonly PropertyInfo _underlyingPropertyInfo;
		private readonly Type _reflectedType;

		internal InheritedPropertyInfo (PropertyInfo underlyingPropertyInfo, Type reflectedType)
			{
			// If the reflectedType is the declaring type, the caller should have used the original PropertyInfo.
			// This assert saves us from having to check this throughout.
			// Debug.Assert(!(reflectedType.Equals(underlyingPropertyInfo.DeclaringType)), "reflectedType must be a proper base type of (and not equal to) underlyingPropertyInfo.DeclaringType.");

			_underlyingPropertyInfo = underlyingPropertyInfo;
			_reflectedType = reflectedType;
			return;
			}

		public override sealed PropertyAttributes Attributes
			{
			get { return _underlyingPropertyInfo.Attributes; }
			}

		public override sealed bool CanRead
			{
			get { return GetMethod != null; }
			}

		public override sealed bool CanWrite
			{
			get { return SetMethod != null; }
			}

		public override sealed ParameterInfo[] GetIndexParameters ()
			{
			return _underlyingPropertyInfo.GetIndexParameters ();
			}

		public override sealed CType PropertyType
			{
			get { return _underlyingPropertyInfo.PropertyType; }
			}

		public override sealed CType DeclaringType
			{
			get { return _underlyingPropertyInfo.DeclaringType; }
			}

		public override sealed String Name
			{
			get { return _underlyingPropertyInfo.Name; }
			}

		public IEnumerable<CustomAttributeData> CustomAttributes
			{
			get { return _underlyingPropertyInfo.GetCustomAttributes (); }
			}

		public override sealed bool Equals (Object obj)
			{
			InheritedPropertyInfo other = obj as InheritedPropertyInfo;
			if (other == null)
				return false;

			if (!(_underlyingPropertyInfo.Equals (other._underlyingPropertyInfo)))
				return false;

			if (!(_reflectedType.Equals (other._reflectedType)))
				return false;

			return true;
			}

		public override sealed int GetHashCode ()
			{
			int hashCode = _reflectedType.GetHashCode ();
			hashCode ^= _underlyingPropertyInfo.GetHashCode ();
			return hashCode;
			}

		public Object GetConstantValue ()
			{
			//return _underlyingPropertyInfo.GetConstantValue ();
			return null;
			}

		public MethodInfo GetMethod
			{
			get
				{
				MethodInfo accessor = _underlyingPropertyInfo.GetGetMethod ();
				return Filter (accessor);
				}
			}

		public override sealed Object GetValue (Object obj, Object[] index)
			{
			if (GetMethod == null)
				throw new ArgumentException ("Get method not found");

			return _underlyingPropertyInfo.GetValue (obj, index);
			}

		public Module Module
			{
			get { return _underlyingPropertyInfo.GetCType ().Module; }
			}

		public override sealed String ToString ()
			{
			return _underlyingPropertyInfo.ToString ();
			}

		public MethodInfo SetMethod
			{
			get
				{
				MethodInfo accessor = _underlyingPropertyInfo.GetSetMethod ();
				return Filter (accessor);
				}
			}

		public override sealed void SetValue (Object obj, Object value, Object[] index)
			{
			if (SetMethod == null)
				throw new ArgumentException ("Set method not found");

			_underlyingPropertyInfo.SetValue (obj, value, index);
			}

		private MethodInfo Filter (MethodInfo accessor)
			{
			//
			// For desktop compat, hide inherited accessors that are marked private.
			//  
			//   Q: Why don't we also hide cross-assembly "internal" accessors?
			//   A: That inconsistency is also desktop-compatible.
			//
			if (accessor == null || accessor.IsPrivate)
				return null;

			return accessor;
			}

		public override MemberTypes MemberType
			{
			get { return MemberTypes.Property; }
			}

		public override MethodInfo[] GetAccessors (bool nonPublic)
			{
			return _underlyingPropertyInfo.GetAccessors (nonPublic);
			}

		public override MethodInfo[] GetAccessors ()
			{
			return _underlyingPropertyInfo.GetAccessors ();
			}

		public override MethodInfo GetGetMethod (bool nonPublic)
			{
			return _underlyingPropertyInfo.GetGetMethod (nonPublic);
			}

		public override MethodInfo GetGetMethod ()
			{
			return _underlyingPropertyInfo.GetGetMethod ();
			}

		public override MethodInfo GetSetMethod (bool nonPublic)
			{
			return _underlyingPropertyInfo.GetSetMethod (nonPublic);
			}

		public override MethodInfo GetSetMethod ()
			{
			return _underlyingPropertyInfo.GetSetMethod ();
			}

		public override object GetValue (object obj, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
			{
			return _underlyingPropertyInfo.GetValue (obj, invokeAttr, binder, index, culture);
			}

		public override bool IsSpecialName
			{
			get { return _underlyingPropertyInfo.IsSpecialName; }
			}

		public override void SetValue (object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
			{
			_underlyingPropertyInfo.SetValue (obj, value, invokeAttr, binder, index, culture);
			}

		public override object[] GetCustomAttributes (CType attributeType, bool inherit)
			{
			return _underlyingPropertyInfo.GetCustomAttributes (attributeType, inherit);
			}

		public override object[] GetCustomAttributes (bool inherit)
			{
			return _underlyingPropertyInfo.GetCustomAttributes (inherit);
			}

		public override bool IsDefined (CType attributeType, bool inherit)
			{
			return _underlyingPropertyInfo.IsDefined (attributeType, inherit);
			}

		public override CType ReflectedType
			{
			get { return _underlyingPropertyInfo.ReflectedType; }
			}
		}
	}