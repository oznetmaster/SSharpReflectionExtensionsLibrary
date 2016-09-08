// System.Reflection/TypeDelegator.cs
//
// Paolo Molaro (lupus@ximian.com)
//
// (C) 2002 Ximian, Inc.

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Crestron.SimplSharp.Reflection
	{
	[ComVisible (true)]
	[Serializable]
	public class TypeDelegator :
#if NET_4_5
		TypeInfo, IReflectableType
#else
		CType
#endif
		{
		protected CType typeImpl;

		protected TypeDelegator ()
			{
			}

		public TypeDelegator (Type delegatingType)
			{
			if (delegatingType == null)
				throw new ArgumentNullException ("delegatingType must be non-null");
			typeImpl = delegatingType;
			}

		public override Assembly Assembly
			{
			get { return typeImpl.Assembly; }
			}

		public override string AssemblyQualifiedName
			{
			get { return typeImpl.AssemblyQualifiedName; }
			}

		public override CType BaseType
			{
			get { return typeImpl.BaseType; }
			}

		public override string FullName
			{
			get { return typeImpl.FullName; }
			}

		public override Guid GUID
			{
			get { return typeImpl.GUID; }
			}

		public override Module Module
			{
			get { return typeImpl.Module; }
			}

		public override string Name
			{
			get { return typeImpl.Name; }
			}

		public override string Namespace
			{
			get { return typeImpl.Namespace; }
			}

		public override RuntimeTypeHandle TypeHandle
			{
			get { return typeImpl.TypeHandle; }
			}

		public override CType UnderlyingSystemType
			{
			get { return typeImpl.UnderlyingSystemType; }
			}

		protected override TypeAttributes GetAttributeFlagsImpl ()
			{
			return typeImpl.Attributes;
			}

		protected override ConstructorInfo GetConstructorImpl (BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types,
		                                                       ParameterModifier[] modifiers)
			{
			return typeImpl.GetConstructor (bindingAttr, binder, callConvention, types, modifiers);
			}

		[ComVisible (true)]
		public override ConstructorInfo[] GetConstructors (BindingFlags bindingAttr)
			{
			return typeImpl.GetConstructors (bindingAttr);
			}

		public override object[] GetCustomAttributes (bool inherit)
			{
			return typeImpl.GetCustomAttributes (inherit);
			}

		public override object[] GetCustomAttributes (CType attributeType, bool inherit)
			{
			return typeImpl.GetCustomAttributes (attributeType, inherit);
			}

		public override CType GetElementType ()
			{
			return typeImpl.GetElementType ();
			}

		public override EventInfo GetEvent (string name, BindingFlags bindingAttr)
			{
			return typeImpl.GetEvent (name, bindingAttr);
			}

		public override EventInfo[] GetEvents ()
			{
			return GetEvents (BindingFlags.Public);
			}

		public override EventInfo[] GetEvents (BindingFlags bindingAttr)
			{
			return typeImpl.GetEvents (bindingAttr);
			}

		public override FieldInfo GetField (string name, BindingFlags bindingAttr)
			{
			return typeImpl.GetField (name, bindingAttr);
			}

		public override FieldInfo[] GetFields (BindingFlags bindingAttr)
			{
			return typeImpl.GetFields (bindingAttr);
			}

		public override Type GetInterface (string name, bool ignoreCase)
			{
			return typeImpl.GetInterface (name, ignoreCase);
			}

		[ComVisible (true)]
		public override InterfaceMapping GetInterfaceMap (Type interfaceType)
			{
			return typeImpl.GetInterfaceMap (interfaceType);
			}

		public override Type[] GetInterfaces ()
			{
			return typeImpl.GetInterfaces ();
			}

		public override MemberInfo[] GetMember (string name, MemberTypes type, BindingFlags bindingAttr)
			{
			return typeImpl.GetMember (name, type, bindingAttr);
			}

		public override MemberInfo[] GetMembers (BindingFlags bindingAttr)
			{
			return typeImpl.GetMembers (bindingAttr);
			}

		protected override MethodInfo GetMethodImpl (string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types,
		                                             ParameterModifier[] modifiers)
			{
			// Can't call GetMethod since it makes restrictive argument checks
			return typeImpl.GetMethodImplInternal (name, bindingAttr, binder, callConvention, types, modifiers);
			}

		public override MethodInfo[] GetMethods (BindingFlags bindingAttr)
			{
			return typeImpl.GetMethods (bindingAttr);
			}

		public override CType GetNestedType (string name, BindingFlags bindingAttr)
			{
			return typeImpl.GetNestedType (name, bindingAttr);
			}

		public override CType[] GetNestedTypes (BindingFlags bindingAttr)
			{
			return typeImpl.GetNestedTypes (bindingAttr);
			}

		public override PropertyInfo[] GetProperties (BindingFlags bindingAttr)
			{
			return typeImpl.GetProperties (bindingAttr);
			}

		protected override PropertyInfo GetPropertyImpl (string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types,
		                                                 ParameterModifier[] modifiers)
			{
			// Can't call GetProperty since it makes restrictive argument checks
			return typeImpl.GetPropertyImplInternal (name, bindingAttr, binder, returnType, types, modifiers);
			}

		protected override bool HasElementTypeImpl ()
			{
			return typeImpl.HasElementType;
			}

		public override object InvokeMember (string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers,
		                                     CultureInfo culture, string[] namedParameters)
			{
			return typeImpl.InvokeMember (name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
			}

		protected override bool IsArrayImpl ()
			{
			return typeImpl.IsArray;
			}

		protected override bool IsByRefImpl ()
			{
			return typeImpl.IsByRef;
			}

		protected override bool IsCOMObjectImpl ()
			{
			return typeImpl.IsCOMObject;
			}

		public override bool IsDefined (CType attributeType, bool inherit)
			{
			return typeImpl.IsDefined (attributeType, inherit);
			}

		protected override bool IsPointerImpl ()
			{
			return typeImpl.IsPointer;
			}

		protected override bool IsPrimitiveImpl ()
			{
			return typeImpl.IsPrimitive;
			}

		protected override bool IsValueTypeImpl ()
			{
			return typeImpl.IsValueType;
			}

		public override int MetadataToken
			{
			get { return typeImpl.MetadataToken; }
			}

#if NET_4_5
		public override bool IsConstructedGenericType {
			get { return typeImpl.IsConstructedGenericType; }
		}

		public override bool IsAssignableFrom (TypeInfo typeInfo)
		{
			if (typeInfo == null)
				throw new ArgumentNullException ("typeInfo");

			return IsAssignableFrom (typeInfo.AsType ());
		}
#endif

		public override TypeAttributes Attributes
			{
			get { return typeImpl.Attributes; }
			}

		public override bool ContainsGenericParameters
			{
			get { return typeImpl.ContainsGenericParameters; }
			}

		public override bool Equals (Type T)
			{
			throw new NotImplementedException ();
			}

		public override bool Equals (CType o)
			{
			throw new NotImplementedException ();
			}

		public override int GetArrayRank ()
			{
			return typeImpl.GetArrayRank ();
			}

		public override ConstructorInfo GetConstructor (BindingFlags bindingAttr, Binder binder, CType[] types, ParameterModifier[] modifiers)
			{
			return typeImpl.GetConstructor (bindingAttr, binder, types, modifiers);
			}

		public override ConstructorInfo GetConstructor (CType[] types)
			{
			return typeImpl.GetConstructor (types);
			}

		public override ConstructorInfo[] GetConstructors ()
			{
			return typeImpl.GetConstructors ();
			}

		public override MemberInfo[] GetDefaultMembers ()
			{
			throw new NotImplementedException ();
			}

		public override EventInfo GetEvent (string name)
			{
			return typeImpl.GetEvent (name);
			}

		public override FieldInfo GetField (string name)
			{
			return typeImpl.GetField (name);
			}

		public override FieldInfo[] GetFields ()
			{
			return typeImpl.GetFields ();
			}

		public override CType[] GetGenericArguments ()
			{
			return typeImpl.GetGenericArguments ();
			}

		public override CType GetGenericTypeDefinition ()
			{
			return typeImpl.GetGenericTypeDefinition ();
			}

		public override int GetHashCodeImplementation ()
			{
			return typeImpl.GetHashCodeImplementation ();
			}

		public override MemberInfo[] GetMember (string name, BindingFlags bindingAttr)
			{
			return typeImpl.GetMember (name, bindingAttr);
			}

		public override MemberInfo[] GetMember (string name)
			{
			return typeImpl.GetMember (name);
			}

		public override MemberInfo[] GetMembers ()
			{
			return typeImpl.GetMembers ();
			}

		public override MethodInfo GetMethod (string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, CType[] types, ParameterModifier[] modifiers)
			{
			return typeImpl.GetMethod (name, bindingAttr, binder, callConvention, types, modifiers);
			}

		public override MethodInfo GetMethod (string name, BindingFlags bindingAttr, Binder binder, CType[] types, ParameterModifier[] modifiers)
			{
			return typeImpl.GetMethod (name, bindingAttr, binder, types, modifiers);
			}

		public override MethodInfo GetMethod (string name, CType[] types, ParameterModifier[] modifiers)
			{
			return typeImpl.GetMethod (name, types, modifiers);
			}

		public override MethodInfo GetMethod (string name, CType[] types)
			{
			return typeImpl.GetMethod (name, types);
			}

		public override MethodInfo GetMethod (string name, BindingFlags bindingAttr)
			{
			return typeImpl.GetMethod (name, bindingAttr);
			}

		public override MethodInfo GetMethod (string name)
			{
			return typeImpl.GetMethod (name);
			}

		public override MethodInfo[] GetMethods ()
			{
			return typeImpl.GetMethods ();
			}

		public override PropertyInfo[] GetProperties ()
			{
			return typeImpl.GetProperties ();
			}

		public override PropertyInfo GetProperty (string name, BindingFlags bindingAttr, Binder binder, CType returnType, CType[] types, ParameterModifier[] modifiers)
			{
			return typeImpl.GetProperty (name, bindingAttr, binder, returnType, types, modifiers);
			}

		public override PropertyInfo GetProperty (string name, CType returnType, CType[] types, ParameterModifier[] modifiers)
			{
			return typeImpl.GetProperty (name, returnType, types, modifiers);
			}

		public override PropertyInfo GetProperty (string name, CType returnType, CType[] types)
			{
			return typeImpl.GetProperty (name, returnType, types);
			}

		public override PropertyInfo GetProperty (string name, CType returnType)
			{
			return typeImpl.GetProperty (name, returnType);
			}

		public override PropertyInfo GetProperty (string name, BindingFlags bindingAttr)
			{
			return typeImpl.GetProperty (name, bindingAttr);
			}

		public override PropertyInfo GetProperty (string name)
			{
			return typeImpl.GetProperty (name);
			}

		public override bool HasElementType
			{
			get { return typeImpl.HasElementType; }
			}

		public override bool IsAbstract
			{
			get { return typeImpl.IsAbstract; }
			}

		public override bool IsAnsiClass
			{
			get { return typeImpl.IsAnsiClass; }
			}

		public override bool IsArray
			{
			get { return typeImpl.IsArray; }
			}

		public override bool IsAssignableFrom (CType c)
			{
			return typeImpl.IsAssignableFrom (c);
			}

		public override bool IsAutoClass
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsAutoLayout
			{
			get { return typeImpl.IsAutoLayout; }
			}

		public override bool IsByRef
			{
			get { return typeImpl.IsByRef; }
			}

		public override bool IsCOMObject
			{
			get { return typeImpl.IsCOMObject; }
			}

		public override bool IsClass
			{
			get { return typeImpl.IsClass; }
			}

		public override bool IsEnum
			{
			get { return typeImpl.IsClass; }
			}

		public override bool IsGenericParameter
			{
			get { return typeImpl.IsGenericParameter; }
			}

		public override bool IsGenericType
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsGenericTypeDefinition
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsImport
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsInstanceOfType (object o)
			{
			throw new NotImplementedException ();
			}

		public override bool IsInterface
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsNestedAssembly
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsNestedFamANDAssem
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsNestedFamORAssem
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsNestedFamily
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsNestedPrivate
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsNestedPublic
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsNotPublic
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsPointer
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsPrimitive
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsPublic
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsSealed
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsSpecialName
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsSubclassOf (CType c)
			{
			throw new NotImplementedException ();
			}

		public override bool IsUnicodeClass
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsValueType
			{
			get { throw new NotImplementedException (); }
			}

		public override bool IsVisible
			{
			get { throw new NotImplementedException (); }
			}

		public override CType MakeGenericType (params CType[] typeArguments)
			{
			throw new NotImplementedException ();
			}

		public override CType DeclaringType
			{
			get { throw new NotImplementedException (); }
			}

		public override MemberTypes MemberType
			{
			get { throw new NotImplementedException (); }
			}

		public override CType ReflectedType
			{
			get { throw new NotImplementedException (); }
			}
		}
	}