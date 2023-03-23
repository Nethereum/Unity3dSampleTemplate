

    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;

    public class SetIl2cppSettings : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerSettings.SetAdditionalIl2CppArgs("--generic-virtual-method-iterations=5");
        }
    }

