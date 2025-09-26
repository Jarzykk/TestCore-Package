using System.IO;
using UnityEditor;
using UnityEngine;

namespace Code.Test.Editor
{
    public class AssetsInstaller
    {
        [MenuItem("Tools/Initialize Core")]
        private static void InitializeCore() => 
            CopyAssetsDirectoryContents();

        private static void CopyAssetsDirectoryContents()
        {
            string packagePath = "Packages/com.jarzykk.test-core/Runtime/Internal";
            string destinationPath = "Assets";
            
            if (!Directory.Exists(packagePath))
            {
                Debug.LogError($"AssetsDirectory not found at: {packagePath}");
                return;
            }
            
            int copiedFiles = 0;
            int skippedFiles = 0;
            
            CopyDirectory(packagePath, destinationPath, ref copiedFiles, ref skippedFiles);
            
            AssetDatabase.Refresh();
            
            Debug.Log($"Core initialization complete! Copied {copiedFiles} files, skipped {skippedFiles} existing files.");
        }
        
        private static void CopyDirectory(string sourcePath, string destinationPath, ref int copiedFiles, ref int skippedFiles)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);
            
            foreach (FileInfo file in sourceDir.GetFiles())
            {
                if (file.Extension == ".meta")
                    continue;
                    
                string destFilePath = Path.Combine(destinationPath, file.Name);
                
                if (File.Exists(destFilePath))
                {
                    skippedFiles++;
                    Debug.Log($"Skipped existing file: {destFilePath}");
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
                    file.CopyTo(destFilePath, false);
                    copiedFiles++;
                    Debug.Log($"Copied: {file.Name} to {destFilePath}");
                }
            }
            
            foreach (DirectoryInfo subDir in sourceDir.GetDirectories())
            {
                string destSubDirPath = Path.Combine(destinationPath, subDir.Name);
                CopyDirectory(subDir.FullName, destSubDirPath, ref copiedFiles, ref skippedFiles);
            }
        }
    }
}