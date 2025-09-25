using UnityEditor;
using UnityEngine;

namespace Code.Test.Editor
{
    public class AssetsInstaller
    {
        [MenuItem("Tools/Test/Say Hello")]
        private static void SayHello()
        {
            Debug.Log("Hello from a custom menu item!");
        }
    }
}