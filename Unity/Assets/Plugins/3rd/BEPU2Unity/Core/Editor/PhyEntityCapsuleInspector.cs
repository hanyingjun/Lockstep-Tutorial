namespace vwengame.bephysics.editor.inspector
{
    using UnityEditor;
    using UnityEngine;
    [CustomEditor(typeof(PhyEntityCapsule))]
    public class PhyEntityCapsuleInspector : PhyEntityBaseInspector
    {
        private GUIContent radiusCTT = new GUIContent("半径");
        private GUIContent heightCTT = new GUIContent("高度");

        private SerializedProperty radiusPPY = null;
        private SerializedProperty heightPPY = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            radiusPPY = serializedObject.FindProperty("_radius");
            heightPPY = serializedObject.FindProperty("_height");
        }

        protected override void CustomOnInspectorGUI()
        {
            base.CustomOnInspectorGUI();
            EditorGUILayout.PropertyField(radiusPPY, radiusCTT);
            EditorGUILayout.PropertyField(heightPPY, heightCTT);
        }
    }
}
