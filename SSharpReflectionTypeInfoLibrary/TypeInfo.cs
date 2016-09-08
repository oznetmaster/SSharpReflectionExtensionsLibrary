using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp.Reflection
	{
	public abstract class TypeInfo : CType, IReflectableType
		{
		protected internal CType _type;

		private static readonly CType[] emptyCTypeArray = new CType[0];

		public virtual IEnumerable<ConstructorInfo> DeclaredConstructors
			{
			get { return GetConstructors (); }
			}

		public virtual IEnumerable<EventInfo> DeclaredEvents
			{
			get { return GetEvents (); }
			}

		public virtual IEnumerable<FieldInfo> DeclaredFields
			{
			get { return GetFields (); }
			}

		public virtual IEnumerable<MemberInfo> DeclaredMembers
			{
			get { return GetMembers (); }
			}

		public virtual IEnumerable<MethodInfo> DeclaredMethods
			{
			get { return GetMethods (); }
			}

		public virtual IEnumerable<TypeInfo> DeclaredNestedTypes
			{
			get { return GetNestedTypes (BindingFlags.Public).Select (t => (TypeInfo)new TypeInfoImpl (t)); }
			}

		public virtual IEnumerable<PropertyInfo> DeclaredProperties
			{
			get { return GetProperties (); }
			}

		public virtual CType[] GenericTypeArguments
			{
			get { return emptyCTypeArray; }
			}

		public virtual CType[] GenericTypeParameters
			{
			get { return emptyCTypeArray; }
			}

		public virtual IEnumerable<CType> ImplementedInterfaces
			{
			get { return GetInterfaces (); }
			}

		public virtual CType AsType ()
			{
			return _type;
			}

		public virtual EventInfo GetDeclaredEvent (string name)
			{
			return GetEvent (name);
			}

		public virtual FieldInfo GetDeclaredField (string name)
			{
			return GetField (name);
			}

		public virtual MethodInfo GetDeclaredMethod (string name)
			{
			return GetMethod (name);
			}

		public virtual IEnumerable<MethodInfo> GetDeclaredMethods (string name)
			{
			return GetMethods ().Where (m => m.Name.Equals (name));
			}

		public virtual TypeInfo GetDeclaredNestedType (string name)
			{
			var nt = GetNestedType (name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			return new TypeInfoImpl (nt == null ? null : new TypeInfoImpl (nt));
			}

		public virtual PropertyInfo GetDeclaredProperty (string name)
			{
			return GetProperty (name);
			}

		public virtual bool IsAssignableFrom (TypeInfo typeInfo)
			{
			return IsAssignableFrom ((CType)typeInfo);
			}

		#region IReflectableType Members

		TypeInfo IReflectableType.GetTypeInfo ()
			{
			return this;
			}

		#endregion
		}

	public interface IReflectableType
		{
		TypeInfo GetTypeInfo ();
		}

	internal class TypeInfoImpl : TypeInfo
		{
		public TypeInfoImpl (CType ctype)
			{
			_type = ctype;

			CrestronEnvironment.GC.SuppressFinalize (this);
			}

		public override Assembly Assembly
			{
			get { return _type.Assembly; }
			}

		public override string AssemblyQualifiedName
			{
			get { return _type.AssemblyQualifiedName; }
			}

		public override TypeAttributes Attributes
			{
			get { return _type.Attributes; }
			}

		public override CType BaseType
			{
			get { return _type.BaseType; }
			}

		public override bool ContainsGenericParameters
			{
			get { return _type.ContainsGenericParameters; }
			}

		public override bool Equals (Type T)
			{
			return _type.Equals (T);
			}

		public override bool Equals (CType o)
			{
			return _type.Equals (o);
			}

		public override string FullName
			{
			get { return _type.FullName; }
			}

		public override int GetArrayRank ()
			{
			return _type.GetArrayRank ();
			}

		public override ConstructorInfo GetConstructor (BindingFlags bindingAttr, Binder binder, CType[] types, ParameterModifier[] modifiers)
			{
			return _type.GetConstructor (bindingAttr, binder, GetTypes (types), modifiers);
			}

		public override ConstructorInfo GetConstructor (CType[] types)
			{
			return _type.GetConstructor (GetTypes (types));
			}

		public override ConstructorInfo[] GetConstructors (BindingFlags bindingAttr)
			{
			return _type.GetConstructors (bindingAttr);
			}

		public override ConstructorInfo[] GetConstructors ()
			{
			return _type.GetConstructors ();
			}

		public override MemberInfo[] GetDefaultMembers ()
			{
			return _type.GetDefaultMembers ();
			}

		public override CType GetElementType ()
			{
			return _type.GetElementType ();
			}

		public override EventInfo GetEvent (string name, BindingFlags bindingAttr)
			{
			return _type.GetEvent (name, bindingAttr);
			}

		public override EventInfo GetEvent (string name)
			{
			return _type.GetEvent (name);
			}

		public override EventInfo[] GetEvents (BindingFlags bindingAttr)
			{
			return _type.GetEvents (bindingAttr);
			}

		public override EventInfo[] GetEvents ()
			{
			return _type.GetEvents ();
			}

		public override FieldInfo GetField (string name, BindingFlags bindingAttr)
			{
			return _type.GetField (name, bindingAttr);
			}

		public override FieldInfo GetField (string name)
			{
			return _type.GetField (name);
			}

		public override FieldInfo[] GetFields (BindingFlags bindingAttr)
			{
			return _type.GetFields (bindingAttr);
			}

		public override FieldInfo[] GetFields ()
			{
			return _type.GetFields ();
			}

		public override CType[] GetGenericArguments ()
			{
			return _type.GetGenericArguments ();
			}

		public override CType GetGenericTypeDefinition ()
			{
			return _type.GetGenericTypeDefinition ();
			}

		public override int GetHashCodeImplementation ()
			{
			return _type.GetHashCodeImplementation ();
			}

		public override CType[] GetInterfaces ()
			{
			return _type.GetInterfaces ();
			}

		public override MemberInfo[] GetMember (string name, BindingFlags bindingAttr)
			{
			return _type.GetMember (name, bindingAttr);
			}

		public override MemberInfo[] GetMember (string name)
			{
			return _type.GetMember (name);
			}

		public override MemberInfo[] GetMembers (BindingFlags bindingAttr)
			{
			return _type.GetMembers (bindingAttr);
			}

		public override MemberInfo[] GetMembers ()
			{
			return _type.GetMembers ();
			}

		public override MethodInfo GetMethod (string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, CType[] types,
		                                      ParameterModifier[] modifiers)
			{
			return _type.GetMethod (name, bindingAttr, binder, callConvention, GetTypes (types), modifiers);
			}

		public override MethodInfo GetMethod (string name, BindingFlags bindingAttr, Binder binder, CType[] types, ParameterModifier[] modifiers)
			{
			return _type.GetMethod (name, bindingAttr, binder, GetTypes (types), modifiers);
			}

		public override MethodInfo GetMethod (string name, CType[] types, ParameterModifier[] modifiers)
			{
			return _type.GetMethod (name, GetTypes (types), modifiers);
			}

		public override MethodInfo GetMethod (string name, CType[] types)
			{
			return _type.GetMethod (name, GetTypes (types));
			}

		public override MethodInfo GetMethod (string name, BindingFlags bindingAttr)
			{
			return _type.GetMethod (name, bindingAttr);
			}

		public override MethodInfo GetMethod (string name)
			{
			return _type.GetMethod (name);
			}

		public override MethodInfo[] GetMethods (BindingFlags bindingAttr)
			{
			return _type.GetMethods (bindingAttr);
			}

		public override MethodInfo[] GetMethods ()
			{
			return _type.GetMethods ();
			}

		public override CType GetNestedType (string name, BindingFlags bindingAttr)
			{
			return _type.GetNestedType (name, bindingAttr);
			}

		public override CType[] GetNestedTypes (BindingFlags bindingAttr)
			{
			return _type.GetNestedTypes (bindingAttr);
			}

		public override PropertyInfo[] GetProperties (BindingFlags bindingAttr)
			{
			return _type.GetProperties (bindingAttr);
			}

		public override PropertyInfo[] GetProperties ()
			{
			return _type.GetProperties ();
			}

		public override PropertyInfo GetProperty (string name, BindingFlags bindingAttr, Binder binder, CType returnType, CType[] types, ParameterModifier[] modifiers)
			{
			return _type.GetProperty (name, bindingAttr, binder, returnType, GetTypes (types), modifiers);
			}

		public override PropertyInfo GetProperty (string name, CType returnType, CType[] types, ParameterModifier[] modifiers)
			{
			return _type.GetProperty (name, returnType, GetTypes (types), modifiers);
			}

		public override PropertyInfo GetProperty (string name, CType returnType, CType[] types)
			{
			return _type.GetProperty (name, returnType, GetTypes (types));
			}

		public override PropertyInfo GetProperty (string name, CType returnType)
			{
			return _type.GetProperty (name, returnType);
			}

		public override PropertyInfo GetProperty (string name, BindingFlags bindingAttr)
			{
			return _type.GetProperty (name, bindingAttr);
			}

		public override PropertyInfo GetProperty (string name)
			{
			return _type.GetProperty (name);
			}

		public override bool HasElementType
			{
			get { return _type.HasElementType; }
			}

		public override bool IsAbstract
			{
			get { return _type.IsAbstract; }
			}

		public override bool IsAnsiClass
			{
			get { return _type.IsAnsiClass; }
			}

		public override bool IsArray
			{
			get { return _type.IsArray; }
			}

		public override bool IsAssignableFrom (CType c)
			{
			var ti = c as TypeInfo;
			return _type.IsAssignableFrom (ti == null ? c : ti._type);
			}

		public override bool IsAssignableFrom (TypeInfo ti)
			{
			return _type.IsAssignableFrom (ti._type);
			}

		public override bool IsAutoClass
			{
			get { return _type.IsAutoClass; }
			}

		public override bool IsAutoLayout
			{
			get { return _type.IsAutoLayout; }
			}

		public override bool IsByRef
			{
			get { return _type.IsByRef; }
			}

		public override bool IsCOMObject
			{
			get { return _type.IsCOMObject; }
			}

		public override bool IsClass
			{
			get { return _type.IsClass; }
			}

		public override bool IsEnum
			{
			get { return _type.IsEnum; }
			}

		public override bool IsGenericParameter
			{
			get { return _type.IsGenericParameter; }
			}

		public override bool IsGenericType
			{
			get { return _type.IsGenericType; }
			}

		public override bool IsGenericTypeDefinition
			{
			get { return _type.IsGenericTypeDefinition; }
			}

		public override bool IsImport
			{
			get { return _type.IsImport; }
			}

		public override bool IsInstanceOfType (object o)
			{
			var ti = o as TypeInfo;
			return _type.IsInstanceOfType (ti == null ? o : ti._type);
			}

		public override bool IsInterface
			{
			get { return _type.IsInterface; }
			}

		public override bool IsNestedAssembly
			{
			get { return _type.IsNestedAssembly; }
			}

		public override bool IsNestedFamANDAssem
			{
			get { return _type.IsNestedFamANDAssem; }
			}

		public override bool IsNestedFamORAssem
			{
			get { return _type.IsNestedFamORAssem; }
			}

		public override bool IsNestedFamily
			{
			get { return _type.IsNestedFamily; }
			}

		public override bool IsNestedPrivate
			{
			get { return _type.IsNestedPrivate; }
			}

		public override bool IsNestedPublic
			{
			get { return _type.IsNestedPublic; }
			}

		public override bool IsNotPublic
			{
			get { return _type.IsNotPublic; }
			}

		public override bool IsPointer
			{
			get { return _type.IsPointer; }
			}

		public override bool IsPrimitive
			{
			get { return _type.IsPrimitive; }
			}

		public override bool IsPublic
			{
			get { return _type.IsPublic; }
			}

		public override bool IsSealed
			{
			get { return _type.IsSealed; }
			}

		public override bool IsSpecialName
			{
			get { return _type.IsSpecialName; }
			}

		public override bool IsSubclassOf (CType c)
			{
			var ti = c as TypeInfo;
			return _type.IsSubclassOf (ti == null ? c : ti._type);
			}

		public override bool IsUnicodeClass
			{
			get { return _type.IsUnicodeClass; }
			}

		public override bool IsValueType
			{
			get { return _type.IsValueType; }
			}

		public override bool IsVisible
			{
			get { return _type.IsVisible; }
			}

		public override CType MakeGenericType (params CType[] typeArguments)
			{
			return _type.MakeGenericType (typeArguments);
			}

		public override Module Module
			{
			get { return _type.Module; }
			}

		public override string Namespace
			{
			get { return _type.Namespace; }
			}

		public override RuntimeTypeHandle TypeHandle
			{
			get { return _type.TypeHandle; }
			}

		public override CType UnderlyingSystemType
			{
			get { return _type.UnderlyingSystemType; }
			}

		public override CType DeclaringType
			{
			get { return _type.DeclaringType; }
			}

		public override object[] GetCustomAttributes (CType attributeType, bool inherit)
			{
			var ti = attributeType as TypeInfo;
			return _type.GetCustomAttributes (ti == null ? attributeType : ti._type, inherit);
			}

		public override object[] GetCustomAttributes (bool inherit)
			{
			return _type.GetCustomAttributes (inherit);
			}

		public override bool IsDefined (CType attributeType, bool inherit)
			{
			var ti = attributeType as TypeInfo;
			return _type.IsDefined (ti == null ? attributeType : ti._type, inherit);
			}

		public override MemberTypes MemberType
			{
			get { return _type.MemberType; }
			}

		public override string Name
			{
			get { return _type.Name; }
			}

		public override CType ReflectedType
			{
			get { return _type.ReflectedType; }
			}

		private static CType[] GetTypes (CType[] types)
			{
			return types == null ? null : types.Select (t =>
				{
				var ti = t as TypeInfo;
				return ti == null ? t : ti._type;
				}).ToArray ();
			}
		}

	public static class TypeInfoExtensions
		{
		public static TypeInfo GetTypeInfo (this Type type)
			{
			return new TypeInfoImpl (type);
			}

		public static TypeInfo GetTypeInfo (this CType ctype)
			{
			return new TypeInfoImpl (ctype);
			}
		}
	}