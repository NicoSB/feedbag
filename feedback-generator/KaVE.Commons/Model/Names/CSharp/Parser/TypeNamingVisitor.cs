//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5-SNAPSHOT
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\Jonas\Documents\Visual Studio 2013\Projects\Grammar\Grammar\TypeNaming.g4 by ANTLR 4.5-SNAPSHOT

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace KaVE.Commons.Model.Names.CSharp.Parser
{

/**
 * Copyright 2016 Sebastian Proksch
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

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="TypeNamingParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5-SNAPSHOT")]
[System.CLSCompliant(false)]
public interface ITypeNamingVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.typeEOL"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeEOL([NotNull] TypeNamingParser.TypeEOLContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.methodEOL"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodEOL([NotNull] TypeNamingParser.MethodEOLContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] TypeNamingParser.TypeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.typeParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeParameter([NotNull] TypeNamingParser.TypeParameterContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.notTypeParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNotTypeParameter([NotNull] TypeNamingParser.NotTypeParameterContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.regularType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRegularType([NotNull] TypeNamingParser.RegularTypeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.delegateType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDelegateType([NotNull] TypeNamingParser.DelegateTypeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.arrayType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayType([NotNull] TypeNamingParser.ArrayTypeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.nestedType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNestedType([NotNull] TypeNamingParser.NestedTypeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.nestedTypeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNestedTypeName([NotNull] TypeNamingParser.NestedTypeNameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.resolvedType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitResolvedType([NotNull] TypeNamingParser.ResolvedTypeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.namespace"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNamespace([NotNull] TypeNamingParser.NamespaceContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.typeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeName([NotNull] TypeNamingParser.TypeNameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.possiblyGenericTypeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPossiblyGenericTypeName([NotNull] TypeNamingParser.PossiblyGenericTypeNameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.enumTypeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnumTypeName([NotNull] TypeNamingParser.EnumTypeNameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.interfaceTypeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInterfaceTypeName([NotNull] TypeNamingParser.InterfaceTypeNameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.structTypeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStructTypeName([NotNull] TypeNamingParser.StructTypeNameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.simpleTypeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSimpleTypeName([NotNull] TypeNamingParser.SimpleTypeNameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.genericTypePart"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGenericTypePart([NotNull] TypeNamingParser.GenericTypePartContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.genericParam"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGenericParam([NotNull] TypeNamingParser.GenericParamContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.assembly"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssembly([NotNull] TypeNamingParser.AssemblyContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.assemblyVersion"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssemblyVersion([NotNull] TypeNamingParser.AssemblyVersionContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.method"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethod([NotNull] TypeNamingParser.MethodContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.regularMethod"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRegularMethod([NotNull] TypeNamingParser.RegularMethodContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.methodParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodParameters([NotNull] TypeNamingParser.MethodParametersContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.nonStaticCtor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNonStaticCtor([NotNull] TypeNamingParser.NonStaticCtorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.staticCctor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStaticCctor([NotNull] TypeNamingParser.StaticCctorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.customMethod"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCustomMethod([NotNull] TypeNamingParser.CustomMethodContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.formalParam"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFormalParam([NotNull] TypeNamingParser.FormalParamContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.staticModifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStaticModifier([NotNull] TypeNamingParser.StaticModifierContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.id"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitId([NotNull] TypeNamingParser.IdContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TypeNamingParser.num"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNum([NotNull] TypeNamingParser.NumContext context);
}
} // namespace KaVE.Commons.Model.Names.CSharp.Parser
