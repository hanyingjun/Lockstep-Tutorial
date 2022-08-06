namespace vwengame.bephysics.editor.inspector
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(PhyEntityBase))]
    public abstract class PhyEntityBaseInspector : Editor
    {
        private GUIContent isUseRigibodyCTT = new GUIContent("是否启用刚体?");
        private GUIContent massCTT = new GUIContent("质量");
        private GUIContent staticFrictionCTT = new GUIContent("静摩擦");
        private GUIContent kineticFrictionCTT = new GUIContent("动摩擦");
        private GUIContent bouncinessCTT = new GUIContent("弹力");
        private GUIContent isTriggerCTT = new GUIContent("IsTrigger");

        protected PhyEntityBase baseEntity;

        private SerializedProperty isUseRigibodyPPY;   // 是否启用刚体
        private SerializedProperty massPPY;            // 质量
        private SerializedProperty staticFrictionPPY;
        private SerializedProperty kineticFrictionPPY;
        private SerializedProperty bouncinessPPY;
        private SerializedProperty isTriggerPPY;


        protected virtual void OnEnable()
        {
            baseEntity = target as PhyEntityBase;

            isUseRigibodyPPY = serializedObject.FindProperty("_isUseRigibody");
            massPPY = serializedObject.FindProperty("_mass");
            staticFrictionPPY = serializedObject.FindProperty("_staticFriction");
            kineticFrictionPPY = serializedObject.FindProperty("_kineticFriction");
            bouncinessPPY = serializedObject.FindProperty("_bounciness");
            isTriggerPPY = serializedObject.FindProperty("_isTrigger");
        }

        protected virtual void CustomOnInspectorGUI()
        {
            if (baseEntity == null)
                return;
            EditorGUILayout.PropertyField(isTriggerPPY, isTriggerCTT);

            //EditorGUILayout.IntSlider(armorProp, 0, 100, new GUIContent("Armor"));
            EditorGUILayout.PropertyField(isUseRigibodyPPY, isUseRigibodyCTT);
            if (baseEntity.IsRigibody)
            {
                EditorGUILayout.PropertyField(massPPY, massCTT);
                EditorGUILayout.PropertyField(staticFrictionPPY, staticFrictionCTT);
                EditorGUILayout.PropertyField(kineticFrictionPPY, kineticFrictionCTT);
                EditorGUILayout.PropertyField(bouncinessPPY, bouncinessCTT);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CustomOnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
