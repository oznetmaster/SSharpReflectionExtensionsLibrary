// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CustomAttributeData = System.Attribute;
using Internal.Reflection.Extensions.NonPortable;

namespace Crestron.SimplSharp.Reflection
	{
	internal static class CustomAttributeInheritanceRules
		{
		//==============================================================================================================================
		// Api helpers: Computes the effective set of custom attributes for various Reflection elements and returns them
		//              as CustomAttributeData objects.
		//==============================================================================================================================
		public static IEnumerable<CustomAttributeData> GetMatchingCustomAttributes (this Assembly element, Type optionalAttributeTypeFilter)
			{
			return GetMatchingCustomAttributes (element, optionalAttributeTypeFilter, false);
			}

		public static IEnumerable<CustomAttributeData> GetMatchingCustomAttributes (this Assembly element, Type optionalAttributeTypeFilter,
		                                                                            bool skipTypeValidation)
			{
			return AssemblyCustomAttributeSearcher.Default.GetMatchingCustomAttributes (element, optionalAttributeTypeFilter, /*inherit:*/ false, /*skipTypeValidation:*/
			                                                                            skipTypeValidation);
			}

		public static IEnumerable<CustomAttributeData> GetMatchingCustomAttributes (this Module element, Type optionalAttributeTypeFilter)
			{
			return GetMatchingCustomAttributes (element, optionalAttributeTypeFilter, false);
			}

		public static IEnumerable<CustomAttributeData> GetMatchingCustomAttributes (this Module element, Type optionalAttributeTypeFilter,
		                                                                            bool skipTypeValidation)
			{
			return ModuleCustomAttributeSearcher.Default.GetMatchingCustomAttributes (element, optionalAttributeTypeFilter, /*inherit:*/ false, /*skipTypeValidation:*/
			                                                                          skipTypeValidation);
			}

		public static IEnumerable<CustomAttributeData> GetMatchingCustomAttributes (this ParameterInfo element, Type optionalAttributeTypeFilter, bool inherit)
			{
			return GetMatchingCustomAttributes (element, optionalAttributeTypeFilter, inherit, false);
			}

		public static IEnumerable<CustomAttributeData> GetMatchingCustomAttributes (this ParameterInfo element, Type optionalAttributeTypeFilter, bool inherit,
		                                                                            bool skipTypeValidation)
			{
			return ParameterCustomAttributeSearcher.Default.GetMatchingCustomAttributes (element, optionalAttributeTypeFilter, inherit, /*skipTypeValidation:*/
			                                                                             skipTypeValidation);
			}

		public static IEnumerable<CustomAttributeData> GetMatchingCustomAttributes (this MemberInfo element, Type optionalAttributeTypeFilter, bool inherit)
			{
			return GetMatchingCustomAttributes (element, optionalAttributeTypeFilter, inherit, false);
			}

		public static IEnumerable<CustomAttributeData> GetMatchingCustomAttributes (this MemberInfo element, Type optionalAttributeTypeFilter, bool inherit,
		                                                                            bool skipTypeValidation)
			{
				{
				TypeInfo typeInfo = element as TypeInfo;
				if (typeInfo != null)
					{
					return TypeCustomAttributeSearcher.Default.GetMatchingCustomAttributes (typeInfo, optionalAttributeTypeFilter, inherit, /*skipTypeValidation:*/
					                                                                        skipTypeValidation);
					}
				}
				{
				ConstructorInfo constructorInfo = element as ConstructorInfo;
				if (constructorInfo != null)
					{
					return ConstructorCustomAttributeSearcher.Default.GetMatchingCustomAttributes (constructorInfo, optionalAttributeTypeFilter, /*inherit:*/ false,
					                                                                               /*skipTypeValidation:*/ skipTypeValidation);
					}
				}
				{
				MethodInfo methodInfo = element as MethodInfo;
				if (methodInfo != null)
					{
					return MethodCustomAttributeSearcher.Default.GetMatchingCustomAttributes (methodInfo, optionalAttributeTypeFilter, inherit,
					                                                                          /*skipTypeValidation:*/ skipTypeValidation);
					}
				}
				{
				FieldInfo fieldInfo = element as FieldInfo;
				if (fieldInfo != null)
					{
					return FieldCustomAttributeSearcher.Default.GetMatchingCustomAttributes (fieldInfo, optionalAttributeTypeFilter, /*inherit:*/ false,
					                                                                         /*skipTypeValidation:*/ skipTypeValidation);
					}
				}
				{
				PropertyInfo propertyInfo = element as PropertyInfo;
				if (propertyInfo != null)
					{
					return PropertyCustomAttributeSearcher.Default.GetMatchingCustomAttributes (propertyInfo, optionalAttributeTypeFilter, inherit,
					                                                                            /*skipTypeValidation:*/ skipTypeValidation);
					}
				}
				{
				EventInfo eventInfo = element as EventInfo;
				if (eventInfo != null)
					{
					return EventCustomAttributeSearcher.Default.GetMatchingCustomAttributes (eventInfo, optionalAttributeTypeFilter, inherit,
					                                                                         /*skipTypeValidation:*/ skipTypeValidation);
					}
				}

			if (element == null)
				throw new ArgumentNullException ();

			throw new NotSupportedException (); // Shouldn't get here.
			}

		//==============================================================================================================================
		// Searcher class for Assemblies.
		//==============================================================================================================================
		private sealed class AssemblyCustomAttributeSearcher : CustomAttributeSearcher<Assembly>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (Assembly element)
				{
				return element.GetCustomAttributes ();
				}

			public static readonly AssemblyCustomAttributeSearcher Default = new AssemblyCustomAttributeSearcher ();
			}

		//==============================================================================================================================
		// Searcher class for Modules.
		//==============================================================================================================================
		private sealed class ModuleCustomAttributeSearcher : CustomAttributeSearcher<Module>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (Module element)
				{
				return element.GetCustomAttributes ();
				}

			public static readonly ModuleCustomAttributeSearcher Default = new ModuleCustomAttributeSearcher ();
			}

		//==============================================================================================================================
		// Searcher class for TypeInfos.
		//==============================================================================================================================
		private sealed class TypeCustomAttributeSearcher : CustomAttributeSearcher<TypeInfo>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (TypeInfo element)
				{
				return element.GetCustomAttributes ();
				}

			public override sealed TypeInfo GetParent (TypeInfo e)
				{
				Type baseType = e.BaseType;
				if (baseType == null)
					return null;

				// Optimization: We shouldn't have any public inheritable attributes on Object or ValueType so don't bother scanning this one.
				//  Since many types derive directly from Object, this should a lot of type.
				//if (baseType.Equals (CommonRuntimeTypes.Object) || baseType.Equals (CommonRuntimeTypes.ValueType))
				if (baseType.Equals (typeof (object)) || baseType.IsValueType)
					return null;

				return baseType.GetTypeInfo ();
				}

			public static readonly TypeCustomAttributeSearcher Default = new TypeCustomAttributeSearcher ();
			}

		//==============================================================================================================================
		// Searcher class for FieldInfos.
		//==============================================================================================================================
		private sealed class FieldCustomAttributeSearcher : CustomAttributeSearcher<FieldInfo>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (FieldInfo element)
				{
				return element.GetCustomAttributes ();
				}

			public static readonly FieldCustomAttributeSearcher Default = new FieldCustomAttributeSearcher ();
			}

		//==============================================================================================================================
		// Searcher class for ConstructorInfos.
		//==============================================================================================================================
		private sealed class ConstructorCustomAttributeSearcher : CustomAttributeSearcher<ConstructorInfo>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (ConstructorInfo element)
				{
				return element.GetCustomAttributes ();
				}

			public static readonly ConstructorCustomAttributeSearcher Default = new ConstructorCustomAttributeSearcher ();
			}

		//==============================================================================================================================
		// Searcher class for MethodInfos.
		//==============================================================================================================================
		private sealed class MethodCustomAttributeSearcher : CustomAttributeSearcher<MethodInfo>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (MethodInfo element)
				{
				return element.GetCustomAttributes ();
				}

			public override sealed MethodInfo GetParent (MethodInfo e)
				{
				return e.GetImplicitlyOverriddenBaseClassMember ();
				}

			public static readonly MethodCustomAttributeSearcher Default = new MethodCustomAttributeSearcher ();
			}

		//==============================================================================================================================
		// Searcher class for PropertyInfos.
		//==============================================================================================================================
		private sealed class PropertyCustomAttributeSearcher : CustomAttributeSearcher<PropertyInfo>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (PropertyInfo element)
				{
				return element.GetCustomAttributes ();
				}

			public override sealed PropertyInfo GetParent (PropertyInfo e)
				{
				return e.GetImplicitlyOverriddenBaseClassMember ();
				}

			public static readonly PropertyCustomAttributeSearcher Default = new PropertyCustomAttributeSearcher ();
			}

		//==============================================================================================================================
		// Searcher class for EventInfos.
		//==============================================================================================================================
		private sealed class EventCustomAttributeSearcher : CustomAttributeSearcher<EventInfo>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (EventInfo element)
				{
				return element.GetCustomAttributes ();
				}

			public override sealed EventInfo GetParent (EventInfo e)
				{
				return e.GetImplicitlyOverriddenBaseClassMember ();
				}

			public static readonly EventCustomAttributeSearcher Default = new EventCustomAttributeSearcher ();
			}

		//==============================================================================================================================
		// Searcher class for ParameterInfos.
		//==============================================================================================================================
		private sealed class ParameterCustomAttributeSearcher : CustomAttributeSearcher<ParameterInfo>
			{
			protected override sealed IEnumerable<CustomAttributeData> GetDeclaredCustomAttributes (ParameterInfo element)
				{
				return element.GetCustomAttributes ();
				}

			public override sealed ParameterInfo GetParent (ParameterInfo e)
				{
				MethodInfo method = e.Member as MethodInfo;
				if (method == null)
					return null; // This is a constructor parameter.
				MethodInfo methodParent = new MethodCustomAttributeSearcher ().GetParent (method);
				if (methodParent == null)
					return null;
				return methodParent.GetParameters ()[e.Position];
				}

			public static readonly ParameterCustomAttributeSearcher Default = new ParameterCustomAttributeSearcher ();
			}
		}
	}