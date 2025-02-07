// AXSharp.Compiler
// Copyright (c) 2023 Peter Kurhajec (PTKu), MTS,  and Contributors. All Rights Reserved.
// Contributors: https://github.com/ix-ax/axsharp/graphs/contributors
// See the LICENSE file in the repository root for more information.
// https://github.com/ix-ax/axsharp/blob/dev/LICENSE
// Third party licenses: https://github.com/ix-ax/axsharp/blob/master/notices.md

using Newtonsoft.Json;
using Polly;
using System.IO;
using AXSharp.Compiler.Exceptions;

namespace AXSharp.Compiler;

/// <summary>
/// Provides configuration setting for the AX# project.
/// </summary>
public class AXSharpConfig : ICompilerOptions
{
    /// <summary>
    /// Creates new instance of IxConfig object.
    /// </summary>
    [Obsolete("Use 'Create IxConfig' instead.")]
    public AXSharpConfig()
    {
            
    }

    /// <summary>
    /// Ix config file name.
    /// </summary>
    public const string CONFIG_FILE_NAME = "AXSharp.config.json";

   
    private string _outputProjectFolder = "ix";

    /// <summary>
    /// Gets or sets the output folder for the Ix project.
    /// </summary>
    public string OutputProjectFolder
    {
        get => _outputProjectFolder.Replace("\\", Path.DirectorySeparatorChar.ToString());
        set => _outputProjectFolder = value;
    }

    
    private string _axProjectFolder;

    /// <summary>
    /// Gets or sets the output folder for the Ix project.
    /// </summary>
    [JsonIgnore]
    public string AxProjectFolder
    {
        get => _axProjectFolder.Replace("\\",Path.DirectorySeparatorChar.ToString());
        set => _axProjectFolder = value;
    }

    /// <summary>
    /// Gets updated or creates default config for given AX project.
    /// </summary>
    /// <param name="directory">AX project directory</param>
    /// <param name="cliCompilerOptions">Compiler options.</param>
    /// <returns>Ix configuration for given AX project.</returns>
    public static AXSharpConfig UpdateAndGetIxConfig(string directory, ICompilerOptions? cliCompilerOptions = null)
    {
        var ixConfigFilePath = Path.Combine(directory, CONFIG_FILE_NAME);

        AXSharpConfig? AXSharpConfig = null;

        if (File.Exists(ixConfigFilePath))
        {
            using (StreamReader r = new StreamReader(ixConfigFilePath))
            {
                AXSharpConfig = JsonConvert.DeserializeObject<AXSharpConfig>(r.ReadToEnd());
            }
        }

        if (AXSharpConfig != null)
        {
            AXSharpConfig.AxProjectFolder = directory;
            OverridesFromCli(AXSharpConfig, cliCompilerOptions);
        }

        using (StreamWriter file = File.CreateText(ixConfigFilePath))
        {
#pragma warning disable CS0618
            AXSharpConfig = AXSharpConfig == null ? new AXSharpConfig() { AxProjectFolder = directory } : AXSharpConfig;
#pragma warning restore CS0618
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, AXSharpConfig);
        }

        using (StreamReader r = new StreamReader(ixConfigFilePath))
        {
            AXSharpConfig = JsonConvert.DeserializeObject<AXSharpConfig>(r.ReadToEnd());
        }

        if (AXSharpConfig != null)
        {
            AXSharpConfig.AxProjectFolder = directory;
        }

        return AXSharpConfig;
    }

   
    public static AXSharpConfig RetrieveIxConfig(string ixConfigFilePath)
    {
        try
        {
            using StreamReader r = new StreamReader(ixConfigFilePath);
            var config = JsonConvert.DeserializeObject<AXSharpConfig>(r.ReadToEnd());
            if (config != null)
            {
                var fi = new FileInfo(ixConfigFilePath);
                if (fi.DirectoryName != null) config.AxProjectFolder = fi.DirectoryName;
            }

            return config;
        }
        catch (Exception ex)
        {
            throw new FailedToReadIxConfigurationFileException($"Unable to process '{ixConfigFilePath}'", ex);
        }
        
    }

    private static void OverridesFromCli(ICompilerOptions fromConfig, ICompilerOptions? fromCli)
    {
        // No CLI params
        if (fromCli == null)
            return;

        // Items to override from the CLI
        fromConfig.OutputProjectFolder = fromCli.OutputProjectFolder ?? fromConfig.OutputProjectFolder;
    }
}