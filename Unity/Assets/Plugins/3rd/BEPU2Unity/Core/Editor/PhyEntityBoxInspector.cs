namespace vwengame.bephysics.editor.inspector
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(PhyEntityBox))]
    public class PhyEntityBoxInspector : PhyEntityBaseInspector
    {
        private PhyEntityBox entity;

        private GUIContent sizeCTT = new GUIContent("宽X高X长");

        private SerializedProperty sizePPY = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            entity = target as PhyEntityBox;
            sizePPY = serializedObject.FindProperty("_size");
        }

        protected override void CustomOnInspectorGUI()
        {
            base.CustomOnInspectorGUI();
            EditorGUILayout.PropertyField(sizePPY, sizeCTT);
        }
    }
}
