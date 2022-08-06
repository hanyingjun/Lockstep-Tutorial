namespace vwengame.bephysics.editor.inspector
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(PhyEntitySphere))]
    public class PhyEntitySphereInspector : PhyEntityBaseInspector
    {
        private GUIContent radiusCTT = new GUIContent("radius");
        private SerializedProperty radiusPPY = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            radiusPPY = serializedObject.FindProperty("_radius");
        }

        protected override void CustomOnInspectorGUI()
        {
            base.CustomOnInspectorGUI();
            EditorGUILayout.PropertyField(radiusPPY, radiusCTT);
        }
    }
}
