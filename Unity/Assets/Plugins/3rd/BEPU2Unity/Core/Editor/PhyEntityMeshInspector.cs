namespace vwengame.bephysics.editor.inspector
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(PhyEntityMesh))]
    public class PhyEntityMeshInspector : PhyEntityBaseInspector
    {
        private GUIContent meshFilterCTT = new GUIContent("meshFitter");
        private SerializedProperty meshFilterPPY = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            meshFilterPPY = serializedObject.FindProperty("meshFilter");
        }

        protected override void CustomOnInspectorGUI()
        {
            base.CustomOnInspectorGUI();
            EditorGUILayout.PropertyField(meshFilterPPY, meshFilterCTT);
        }
    }
}