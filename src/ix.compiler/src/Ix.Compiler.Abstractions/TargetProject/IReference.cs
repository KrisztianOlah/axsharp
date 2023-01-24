﻿// Ix.Compiler.Abstractions
// Copyright (c) 2023 Peter Kurhajec (PTKu), MTS,  and Contributors. All Rights Reserved.
// Contributors: https://github.com/ix-ax/ix/graphs/contributors
// See the LICENSE file in the repository root for more information.
// https://github.com/ix-ax/ix/blob/master/LICENSE
// Third party licenses: https://github.com/ix-ax/ix/blob/master/notices.md

// ReSharper disable once CheckNamespace

namespace Ix.Compiler;

public interface IReference
{
    string ReferencePath { get; }
    string Version { get; }

    public string MetadataPath => Path.Combine(ReferencePath, ".meta", "meta.json");

    public string ProjectInfo => Path.Combine(ReferencePath, ".meta", "sourceinfo.json");

    public bool IsIxDependency => File.Exists(MetadataPath);
}