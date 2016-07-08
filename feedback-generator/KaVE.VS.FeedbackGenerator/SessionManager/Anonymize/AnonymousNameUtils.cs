/*
 * Copyright 2014 Technische Universitšt Darmstadt
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.IDEComponents;
using KaVE.Commons.Model.Naming.Impl.v0.CodeElements;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Assertion;
using KaVE.JetBrains.Annotations;
using KaVE.VS.FeedbackGenerator.Utils.Naming;

namespace KaVE.VS.FeedbackGenerator.SessionManager.Anonymize
{
    public static class AnonymousNameUtils
    {
        [ContractAnnotation("notnull => notnull")]
        public static string ToHash(this string value)
        {
            if (value == null || "".Equals(value))
            {
                return value;
            }
            var tmpSource = value.AsBytes();
            var hash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return Convert.ToBase64String(hash).Replace('+', '-').Replace('/', '_');
        }

        [ContractAnnotation("notnull => notnull")]
        public static TName ToAnonymousName<TName>(this TName name) where TName : class, IName
        {
            if (name == null || name.IsUnknown)
            {
                return name;
            }
            return ToAnonymousName<IDocumentName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IWindowName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<ISolutionName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IProjectName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IProjectItemName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IAliasName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IAssemblyName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<ITypeName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<ILocalVariableName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IFieldName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IPropertyName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IEventName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IParameterName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<ILambdaName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IMethodName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<INamespaceName, TName>(name, ToAnonymousName) ??
                   ToAnonymousName<IName, TName>(name, ToAnonymousName) ??
                   Asserts.Fail<TName>("unhandled name type: {0}", name.GetType());
        }

        private static TR ToAnonymousName<TName, TR>(TR name, Func<TName, TName> anonymizer)
            where TName : class, IName where TR : class, IName
        {
            var concreteName = name as TName;
            return (concreteName != null ? anonymizer(concreteName) : null) as TR;
        }

        private static INamespaceName ToAnonymousName(INamespaceName @namespace)
        {
            return Names.Namespace(@namespace.Identifier.ToHash());
        }

        private static ILambdaName ToAnonymousName(ILambdaName lambda)
        {
            var identifier = new StringBuilder();
            identifier.AppendFormat("[{0}] ", lambda.ReturnType.ToAnonymousName())
                      .AppendParameters(lambda.Parameters, true);
            return Names.Lambda(identifier.ToString());
        }

        private static IMethodName ToAnonymousName(IMethodName method)
        {
            var identifier = new StringBuilder();
            identifier.AppendAnonymousMemberName(method, method.ReturnType);
            identifier.AppendIf(method.HasTypeParameters, "`" + method.TypeParameters.Count);
            var inLocalProject = method.DeclaringType.IsDeclaredInEnclosingProjectOrUnknown();
            identifier.AppendTypeParameters(method, inLocalProject);
            identifier.AppendParameters(method.Parameters, inLocalProject);
            return Names.Method(identifier.ToString());
        }

        private static void AppendParameters(this StringBuilder identifier,
            IEnumerable<IParameterName> parameterNames,
            bool anonymizeNames)
        {
            identifier.Append("(");
            identifier.Append(string.Join(", ", parameterNames.Select(p => ToAnonymousName(p, anonymizeNames))));
            identifier.Append(")");
        }

        private static IParameterName ToAnonymousName(IParameterName parameter)
        {
            return ToAnonymousName(parameter, true);
        }

        // TODO NameUpdate: put modifier to central place
        private static IParameterName ToAnonymousName(IParameterName parameter, bool anonymizeName)
        {
            var identifier = new StringBuilder();
            identifier.AppendIf(parameter.IsParameterArray, ParameterName.VarArgsModifier + " ");
            identifier.AppendIf(parameter.IsOutput, ParameterName.OutputModifier + " ");
            identifier.AppendIf(parameter.IsOptional, ParameterName.OptionalModifier + " ");
            identifier.AppendIf(parameter.HasPassByReferenceModifier(), ParameterName.PassByReferenceModifier + " ");
            identifier.AppendAnonymousTypeName(parameter.ValueType, anonymizeName).Append(' ');
            identifier.Append(anonymizeName ? parameter.Name.ToHash() : parameter.Name);
            return ParameterName.Get(identifier.ToString());
        }

        private static bool HasPassByReferenceModifier(this IParameterName parameter)
        {
            return parameter.IsPassedByReference && !parameter.ValueType.IsReferenceType;
        }

        private static StringBuilder AppendAnonymousTypeName(this StringBuilder identifier,
            ITypeName type,
            bool parameterNameWasAnonymized = false)
        {
            identifier.Append('[');
            if (type.IsTypeParameter && parameterNameWasAnonymized)
            {
                identifier.Append(((ITypeParameterName) type).TypeParameterShortName.ToHash());
            }
            else
            {
                identifier.Append(type.ToAnonymousName());
            }
            identifier.Append(']');
            return identifier;
        }

        private static IEventName ToAnonymousName(IEventName @event)
        {
            var identifier = new StringBuilder();
            identifier.AppendAnonymousMemberName(@event, @event.HandlerType);
            return Names.Event(identifier.ToString());
        }

        private static IPropertyName ToAnonymousName(IPropertyName property)
        {
            var identifier = new StringBuilder();
            identifier.AppendIf(property.HasSetter, PropertyName.SetterModifier + " ");
            identifier.AppendIf(property.HasGetter, PropertyName.GetterModifier + " ");
            identifier.AppendAnonymousMemberName(property, property.ValueType);
            return Names.Property(identifier.ToString());
        }

        private static IFieldName ToAnonymousName(IFieldName field)
        {
            var identifier = new StringBuilder();
            identifier.AppendAnonymousMemberName(field, field.ValueType);
            return Names.Field(identifier.ToString());
        }

        private static void AppendAnonymousMemberName(this StringBuilder identifier,
            IMemberName member,
            ITypeName valueType)
        {
            identifier.AppendIf(member.IsStatic, MemberName.StaticModifier + " ");
            identifier.AppendAnonymousTypeName(valueType).Append(' ');
            identifier.AppendAnonymousTypeName(member.DeclaringType).Append('.');
            var originatesInAssembly = !member.DeclaringType.IsDeclaredInEnclosingProjectOrUnknown();
            var isCtor = member.Name.Equals(".ctor");
            var isCctor = member.Name.Equals(".cctor");
            var nameShouldNotBeHashed = originatesInAssembly || isCctor || isCtor;
            identifier.Append(nameShouldNotBeHashed ? member.Name : member.Name.ToHash());
        }

        private static ILocalVariableName ToAnonymousName(ILocalVariableName variable)
        {
            var identifier = new StringBuilder();
            identifier.AppendAnonymousTypeName(variable.ValueType).Append(' ');
            identifier.Append(variable.Name.ToHash());
            return Names.LocalVariable(identifier.ToString());
        }

        private static IName ToAnonymousName(IName name)
        {
            return Names.General(name.Identifier.ToHash());
        }

        private static ITypeName ToAnonymousName(ITypeName type)
        {
            if (type.IsUnknownType)
            {
                return type;
            }
            if (type.IsNestedType)
            {
                return ToAnonymousType_Nested(type);
            }
            if (type.IsArrayType)
            {
                return ToAnonymousType_Array(type.AsArrayTypeName);
            }
            if (type.IsDelegateType)
            {
                return ToAnonymousType_Delegate(type.AsDelegateTypeName);
            }
            if (type.IsTypeParameter)
            {
                return ToAnonymousType_TypeParameter(type.AsTypeParameterName);
            }

            if (!"TypeName".Equals(type.GetType().Name))
            {
                Asserts.Fail<ITypeName>("unknown type implementation");
            }

           return ToAnonymousType_Regular(type);
        }

        private static ITypeName ToAnonymousType_Nested(ITypeName type)
        {
            throw new NotImplementedException();
        }

        private static IArrayTypeName ToAnonymousType_Array(IArrayTypeName type)
        {
            var rank = type.Rank;
            var anonymousBaseName = type.ArrayBaseType.ToAnonymousName();
            return Names.ArrayType(rank, anonymousBaseName);
        }

        private static IDelegateTypeName ToAnonymousType_Delegate(IDelegateTypeName type)
        {
            var identifier = new StringBuilder();
            identifier.AppendTypeKindPrefix(type);
            identifier.Append("[");
            identifier.Append(type.ReturnType.ToAnonymousName());
            identifier.Append("] [");
            identifier.Append(type.DelegateType.ToAnonymousName());
            identifier.Append("].");
            identifier.AppendParameters(type.Parameters, true);

            return Names.Type(identifier.ToString()).AsDelegateTypeName;
        }

        private static ITypeName ToAnonymousType_Regular(ITypeName type)
        {
            var identifier = new StringBuilder();
            identifier.AppendTypeKindPrefix(type);
            var inEnclosingProject = type.IsDeclaredInEnclosingProjectOrUnknown();
           // TODO NAmeUpdate: WTF?
           // identifier.Append(inEnclosingProject ? type.AnonymizedRawFullName() : type.RawFullName);
            identifier.AppendTypeParameters(type, inEnclosingProject).Append(", ");
            identifier.Append(type.Assembly.ToAnonymousName());
            return Names.Type(identifier.ToString());
        }

        private static void AppendTypeKindPrefix(this StringBuilder identifier, ITypeName type)
        {
            var prefix = type.Identifier.Substring(0, 2);
            if (prefix.EndsWith(":"))
            {
                identifier.Append(prefix);
            }
        }

        private static string AnonymizedRawFullName(this ITypeName type)
        {
            // We want to keep the number of type parameters (`1), array braces ([]), nesting markers (+), and the
            // separation between the namespace and the class name. Examples of raw names in the cases we,
            // therefore, handle are:
            // * Namespace.TypeName`1
            // * OuterType+InnerType[]
            // * TypeName`2[,]
            var @namespace = type.Namespace;
            // TODO NameUpdate: WTF?
            //var rawFullName = type.RawFullName;
            string rawFullName = "";
            rawFullName = rawFullName.Substring(@namespace.Identifier.Length);
            var baseName = rawFullName;
            var suffix = "";
            var indexOfDelimiter = rawFullName.IndexOfAny(new[] {'`', '['});
            if (indexOfDelimiter > -1)
            {
                suffix = rawFullName.Substring(indexOfDelimiter);
                baseName = rawFullName.Substring(0, indexOfDelimiter);
            }
            var baseNameParts = baseName.Split('+');
            if (baseNameParts.Length > 0)
            {
                baseName = string.Join("+", baseNameParts.Select(ToHash));
            }
            else
            {
                baseName = baseName.ToHash();
            }
            if (@namespace.IsGlobalNamespace)
            {
                return baseName + suffix;
            }
            return @namespace.ToAnonymousName() + "." + baseName + suffix;
        }

        private static StringBuilder AppendTypeParameters(this StringBuilder identifier,
            IGenericName type,
            bool anonymizeShortNames)
        {
            if (type.HasTypeParameters)
            {
                identifier.Append("[[");
                identifier.Append(
                    string.Join("],[", type.TypeParameters.ToAnonymousTypeParameters(anonymizeShortNames)));
                identifier.Append("]]");
            }
            return identifier;
        }

        private static IEnumerable<ITypeParameterName> ToAnonymousTypeParameters(this IList<ITypeName> typeParameters,
            bool anonymizeShortNames)
        {
            return
                typeParameters.OfType<ITypeParameterName>()
                              .Select(tp => ToAnonymousTypeParameter(anonymizeShortNames)(tp));
        }

        private static Func<ITypeParameterName, ITypeParameterName> ToAnonymousTypeParameter(bool anonymizeShortNames)
        {
            return typeParameter =>
            {
                var rightmostSideIsAShortName = typeParameter.TypeParameterType.IsTypeParameter &&
                                                (((ITypeParameterName) typeParameter.TypeParameterType)
                                                    .TypeParameterType == null ||
                                                 ((ITypeParameterName) typeParameter.TypeParameterType)
                                                     .TypeParameterType.IsUnknownType);
                return Names.TypeParameter(
                    anonymizeShortNames
                        ? typeParameter.TypeParameterShortName.ToHash()
                        : typeParameter.TypeParameterShortName,
                    rightmostSideIsAShortName
                        ? ((ITypeParameterName) typeParameter.TypeParameterType).TypeParameterShortName.ToHash()
                        : typeParameter.TypeParameterType.ToAnonymousName().Identifier);
            };
        }

        private static ITypeParameterName ToAnonymousType_TypeParameter(ITypeParameterName typeParameter)
        {
            return ToAnonymousTypeParameter(false)(typeParameter);
        }

        private static IAssemblyName ToAnonymousName(IAssemblyName assembly)
        {
            return assembly.IsLocalProject ? Names.Assembly(assembly.Identifier.ToHash()) : assembly;
        }

        private static bool IsDeclaredInEnclosingProjectOrUnknown(this ITypeName type)
        {
            return type.IsUnknownType || type.Assembly.IsLocalProject;
        }

        private static IAliasName ToAnonymousName(IAliasName alias)
        {
            return Names.Alias(alias.Identifier.ToHash());
        }

        [ContractAnnotation("notnull => notnull")]
        public static IDocumentName ToAnonymousName([CanBeNull] this IDocumentName document)
        {
            return document == null
                ? null
                : CreateAnonymizedName(VsComponentNameFactory.GetDocumentName, document.Language, document.FileName);
        }

        [ContractAnnotation("notnull => notnull")]
        public static IProjectItemName ToAnonymousName([CanBeNull] this IProjectItemName projectItem)
        {
            return projectItem == null
                ? null
                : CreateAnonymizedName(VsComponentNameFactory.GetProjectItemName, projectItem.Type, projectItem.Name);
        }

        [ContractAnnotation("notnull => notnull")]
        public static IProjectName ToAnonymousName([CanBeNull] this IProjectName project)
        {
            return project == null
                ? null
                : CreateAnonymizedName(VsComponentNameFactory.GetProjectName, project.Type, project.Name);
        }

        [ContractAnnotation("notnull => notnull")]
        public static ISolutionName ToAnonymousName([CanBeNull] this ISolutionName solution)
        {
            return solution == null
                ? null
                : VsComponentNameFactory.GetSolutionName(solution.Identifier.ToHash());
        }

        [ContractAnnotation("notnull => notnull")]
        public static IWindowName ToAnonymousName([CanBeNull] this IWindowName window)
        {
            return window == null
                ? null
                : CreateAnonymizedName(VsComponentNameFactory.GetWindowName, window.Type, window.Caption);
        }

        private static TName CreateAnonymizedName<TName>(Func<string, string, TName> factory, string type, string name)
        {
            var isFileName = name.Contains("\\") || name.Contains(".");
            if (isFileName)
            {
                name = name.ToHash();
            }
            return factory(type, name);
        }
    }
}