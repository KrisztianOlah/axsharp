﻿// Ix.Compiler.Cs
// Copyright (c) 2023 Peter Kurhajec (PTKu), MTS,  and Contributors. All Rights Reserved.
// Contributors: https://github.com/ix-ax/ix/graphs/contributors
// See the LICENSE file in the repository root for more information.
// https://github.com/ix-ax/ix/blob/master/LICENSE
// Third party licenses: https://github.com/ix-ax/ix/blob/master/notices.md

using System.Text;
using AX.ST.Semantic;
using AX.ST.Semantic.Model;
using AX.ST.Semantic.Model.Declarations;
using AX.ST.Semantic.Model.Declarations.Types;
using AX.ST.Semantic.Pragmas;
using Ix.Compiler.Core;
using IX.Compiler.Core;
using Ix.Compiler.Cs.Helpers;
using Ix.Compiler.Cs.Helpers.Onliners;
using AX.ST.Syntax.Tree;

namespace Ix.Compiler.Cs.Onliner;

internal class CsOnlinerPlainerPlainToOnlineBuilder : ICombinedThreeVisitor
{
    private readonly StringBuilder _memberDeclarations = new();

    protected CsOnlinerPlainerPlainToOnlineBuilder(Compilation compilation)
    {
        Compilation = compilation;
    }

    private Compilation Compilation { get; }

    public string Output => _memberDeclarations.ToString().FormatCode();

    
    public void CreateFieldDeclaration(IFieldDeclaration fieldDeclaration, IxNodeVisitor visitor)
    {
        if (fieldDeclaration.IsMemberEligibleForTranspile(Compilation))
        {
            CreateAssignment(fieldDeclaration.Type, fieldDeclaration);
        }
    }

    public void CreateInterfaceDeclaration(IInterfaceDeclaration interfaceDeclaration, IxNodeVisitor visitor)
    {
        //
    }

    public void CreateVariableDeclaration(IVariableDeclaration variableDeclaration, IxNodeVisitor visitor)
    {
        if (variableDeclaration.IsMemberEligibleForTranspile(Compilation))
        {
            CreateAssignment(variableDeclaration.Type, variableDeclaration);
        }
    }

    private void CreateAssignment(ITypeDeclaration typeDeclaration, IDeclaration declaration)
    {
        switch (typeDeclaration)
        {
            case IInterfaceDeclaration interfaceDeclaration:
            case IClassDeclaration classDeclaration:
            //case IAnonymousTypeDeclaration anonymousTypeDeclaration:
            case IStructuredTypeDeclaration structuredTypeDeclaration:
                AddToSource($" this.{declaration.Name}.{MethodName}(plain.{declaration.Name});");
                break;
            case IArrayTypeDeclaration arrayTypeDeclaration:
                AddToSource($"Ix.Connector.BuilderHelpers.Arrays.CopyPlainToOnline<Pocos.{arrayTypeDeclaration.ElementTypeAccess.Type.FullyQualifiedName}, {arrayTypeDeclaration.ElementTypeAccess.Type.FullyQualifiedName}>(plain.{declaration.Name}, {declaration.Name});");
                break;
            case IReferenceTypeDeclaration referenceTypeDeclaration:
                break;
            case IEnumTypeDeclaration enumTypeDeclaration:
                AddToSource($" {declaration.Name}.Cyclic = (short)plain.{declaration.Name};");
                break;
            case INamedValueTypeDeclaration namedValueTypeDeclaration:
                AddToSource($" {declaration.Name}.Cyclic = plain.{declaration.Name};");
                break;
            case IScalarTypeDeclaration scalarTypeDeclaration:
            case IStringTypeDeclaration stringTypeDeclaration:
                AddToSource($" {declaration.Name}.Cyclic = plain.{declaration.Name};");
                break;
        }
    }

    public void CreateArrayTypeDeclaration(IArrayTypeDeclaration arrayTypeDeclaration, IxNodeVisitor visitor)
    {
        //
    }


    protected void AddToSource(string token, string separator = " ")
    {
        _memberDeclarations.Append($"{token}{separator}");
    }

    public void AddTypeConstructionParameters(string parametersString)
    {
        AddToSource(parametersString);
    }

    private static readonly string MethodName = "PlainToOnline";

    public static CsOnlinerPlainerPlainToOnlineBuilder Create(IxNodeVisitor visitor, IStructuredTypeDeclaration semantics,
        Compilation compilation)
    {
        var builder = new CsOnlinerPlainerPlainToOnlineBuilder(compilation);
        builder.AddToSource($"public void {MethodName}(Pocos.{semantics.FullyQualifiedName} plain){{\n");
        semantics.Fields.ToList().ForEach(p => p.Accept(visitor, builder));
        builder.AddToSource($"}}");
        return builder;
    }

    public static CsOnlinerPlainerPlainToOnlineBuilder Create(IxNodeVisitor visitor, IClassDeclaration semantics,
        Compilation compilation, bool isExtended)
    {
        var builder = new CsOnlinerPlainerPlainToOnlineBuilder(compilation);
        builder.AddToSource($"public void {MethodName}(Pocos.{semantics.FullyQualifiedName} plain){{\n");
       

        if (isExtended)
        {
            builder.AddToSource($"base.{MethodName}(plain);");
        }

        semantics.Fields.ToList().ForEach(p => p.Accept(visitor, builder));

        builder.AddToSource($"}}");
        return builder;
    }
}