using System;
using System.IO;
using System.Text;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditorInternal;
using UnityEngine;

[InitializeOnLoad]
public class CsProjFixer
{
    static CsProjFixer()
    {
        ProjectFilesGenerator.ProjectFileGeneration += FixProj;
        //CompilationPipeline.assemblyCompilationStarted += a => Debug.Log( "[CsProjFixer] - FixProj: " + a );
    }
    private static String FixProj(String name, String content)
    {
        var path = CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(name.Substring(0, name.Length - 7));

        try
        {
            StringBuilder sb = new StringBuilder();
            string line = null;

            Boolean isEditor = name.Contains(".Editor");

            using (var sr = new StringReader(content))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "    <LangVersion>6</LangVersion>")
                        sb.AppendLine("    <LangVersion>7</LangVersion>");

                    else if (line == "    <Reference Include=\"Boo.Lang\" />")
                    {
                    }
                    else if (line == "    <Reference Include=\"UnityScript.Lang\" />")
                    {
                    }
                    else if (!String.IsNullOrEmpty(path) && line.StartsWith("    <None Include"))
                    {

                    }
                    else if (!isEditor)
                    {
                        if (line == "    <Reference Include=\"System.XML\" />")
                        {
                        }
                        else if (line == "    <Reference Include=\"System.Xml.Linq\" />")
                        {
                        }
                        else
                        {
                            sb.AppendLine(line);
                        }
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError("[csProjFixer] - FixProj: Fix Fail for file: " + name);
            Debug.LogException(ex);
        }

        return content;
    }
}