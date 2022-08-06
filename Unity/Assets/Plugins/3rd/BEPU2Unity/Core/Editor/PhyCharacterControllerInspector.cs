namespace vwengame.bephysics.editor.inspector
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [DisallowMultipleComponent]
    [CustomEditor(typeof(PhyCharacterController))]
    public class PhyCharacterControllerInspector : PhyEntityBaseInspector
    {
        private GUIContent heightCTT = new GUIContent("身高");
        private GUIContent radiusCTT = new GUIContent("半径");
        private GUIContent marginCTT = new GUIContent("margin");
        private GUIContent maximumTractionSlopeCTT = new GUIContent("行走最大坡度");
        private GUIContent maximumSupportSlopeCTT = new GUIContent("静止最大坡度");
        private GUIContent standingSpeedCTT = new GUIContent("移速");
        private GUIContent crouchingSpeedCTT = new GUIContent("蹲下移速");
        private GUIContent prongSpeedCTT = new GUIContent("爬行移速");
        private GUIContent tractionForceCTT = new GUIContent("最大牵引力");
        private GUIContent slidingSpeedCTT = new GUIContent("slidingSpeed");
        private GUIContent slidingForceCTT = new GUIContent("slidingForce");
        private GUIContent airSpeedCTT = new GUIContent("airSpeed");
        private GUIContent airForceCTT = new GUIContent("airForce");
        private GUIContent jumpSpeedCTT = new GUIContent("jumpSpeed");
        private GUIContent slidingJumpSpeedCTT = new GUIContent("slidingJumpSpeed");
        private GUIContent maximumGlueForceCTT = new GUIContent("maximumGlueForce");

        private SerializedProperty heightPPY = null;
        private SerializedProperty radiusPPY = null;
        private SerializedProperty marginPPY = null;
        private SerializedProperty maximumTractionSlopePPY = null;
        private SerializedProperty maximumSupportSlopePPY = null;
        private SerializedProperty standingSpeedPPY = null;
        private SerializedProperty crouchingSpeedPPY = null;
        private SerializedProperty prongSpeedPPY = null;
        private SerializedProperty tractionForcePPY = null;
        private SerializedProperty slidingSpeedPPY = null;
        private SerializedProperty slidingForcePPY = null;
        private SerializedProperty airSpeedPPY = null;
        private SerializedProperty airForcePPY = null;
        private SerializedProperty jumpSpeedPPY = null;
        private SerializedProperty slidingJumpSpeedPPY = null;
        private SerializedProperty maximumGlueForcePPY = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            heightPPY = serializedObject.FindProperty("height");;
            radiusPPY = serializedObject.FindProperty("radius");;
            marginPPY = serializedObject.FindProperty("margin");;
            maximumTractionSlopePPY = serializedObject.FindProperty("maximumTractionSlope");;
            maximumSupportSlopePPY = serializedObject.FindProperty("maximumSupportSlope");;
            standingSpeedPPY = serializedObject.FindProperty("standingSpeed");;
            crouchingSpeedPPY = serializedObject.FindProperty("crouchingSpeed");;
            prongSpeedPPY = serializedObject.FindProperty("prongSpeed");;
            tractionForcePPY = serializedObject.FindProperty("tractionForce");;
            slidingSpeedPPY = serializedObject.FindProperty("slidingSpeed");;
            slidingForcePPY = serializedObject.FindProperty("slidingForce");;
            airSpeedPPY = serializedObject.FindProperty("airSpeed");;
            airForcePPY = serializedObject.FindProperty("airForce");;
            jumpSpeedPPY = serializedObject.FindProperty("jumpSpeed");;
            slidingJumpSpeedPPY = serializedObject.FindProperty("slidingJumpSpeed");;
            maximumGlueForcePPY = serializedObject.FindProperty("maximumGlueForce");;
        }

        protected override void CustomOnInspectorGUI()
        {
            base.CustomOnInspectorGUI();
            EditorGUILayout.PropertyField(heightPPY, heightCTT);
            EditorGUILayout.PropertyField(radiusPPY, radiusCTT);
            EditorGUILayout.PropertyField(marginPPY, marginCTT);
            EditorGUILayout.PropertyField(maximumTractionSlopePPY, maximumTractionSlopeCTT);
            EditorGUILayout.PropertyField(maximumSupportSlopePPY, maximumSupportSlopeCTT);
            EditorGUILayout.PropertyField(standingSpeedPPY, standingSpeedCTT);
            EditorGUILayout.PropertyField(crouchingSpeedPPY, crouchingSpeedCTT);
            EditorGUILayout.PropertyField(prongSpeedPPY, prongSpeedCTT);
            EditorGUILayout.PropertyField(tractionForcePPY, tractionForceCTT);
            EditorGUILayout.PropertyField(slidingSpeedPPY, slidingSpeedCTT);
            EditorGUILayout.PropertyField(slidingForcePPY, slidingForceCTT);
            EditorGUILayout.PropertyField(airSpeedPPY, airSpeedCTT);
            EditorGUILayout.PropertyField(airForcePPY, airForceCTT);
            EditorGUILayout.PropertyField(jumpSpeedPPY, jumpSpeedCTT);
            EditorGUILayout.PropertyField(slidingJumpSpeedPPY, slidingJumpSpeedCTT);
            EditorGUILayout.PropertyField(maximumGlueForcePPY, maximumGlueForceCTT);
        }
    }
}
