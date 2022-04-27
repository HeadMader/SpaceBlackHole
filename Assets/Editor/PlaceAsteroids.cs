using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlaceAsteroids : EditorWindow
{

    static int numberOfCopies = 10;
    static GameObject Asteroid;
    static Vector4 Value;

    [MenuItem("Place/Asteroids")]
	public static void Init()
	{
		CreateWindow<PlaceAsteroids>("Place Asteroids");
    }
    [MenuItem("Place/ScreenShot")]
    public static void ScreenShot()
    {
        ScreenCapture.CaptureScreenshot("G:/GitProjects/SpaceBackHole/ss.png",2);
    }

    public static void PlaceRandomly()
    {
        for (int i =0; i < numberOfCopies; i++)
		{
            Vector2 positionOfAsteroid = new Vector2(Random.Range(Value.x,Value.y), Random.Range(Value.x, Value.y));
            GameObject gameObject = Instantiate(Asteroid, new Vector3(positionOfAsteroid.x,0 ,positionOfAsteroid.y), Quaternion.identity, Asteroid.transform.parent);
            Undo.RegisterCreatedObjectUndo(gameObject, "Add object"); //How to undo single object?
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        numberOfCopies = EditorGUILayout.IntField("Number of Copies",numberOfCopies);

		Value = EditorGUILayout.Vector4Field("Values",Value);

		EditorGUILayout.MinMaxSlider(ref Value.x, ref Value.y, Value.z, Value.w);
        Asteroid = EditorGUILayout.ObjectField("Trampoline prefab" , Asteroid, typeof(GameObject),true) as GameObject;
   
        if (GUILayout.Button("Place", GUILayout.Height(30)))
        {
            PlaceRandomly();
        }
    }
}

