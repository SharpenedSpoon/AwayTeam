using UnityEngine;
using System;
using System.Collections;


public class TerrainGeneration : MonoBehaviour {

	public int seed = 1234;
	public int randomPoints = 50;

	private Terrain terrain;
	private TerrainData tData;
	private System.Random rnd;
	private int tHeight;
	private int tWidth;
	private int step;


	void Start() {
		terrain = Terrain.activeTerrain;
		tData = terrain.terrainData;
		tHeight = tData.heightmapHeight;
		tWidth = tData.heightmapWidth;
		step = Mathf.Min(tHeight, tWidth);
		rnd = new System.Random(seed);

		resetHeightMap();
		seedHeightMap();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Z)) {
			step = Mathf.Min(tHeight, tWidth);
			resetHeightMap();
		}
		if (Input.GetKeyDown(KeyCode.X)) {
			rnd = new System.Random(seed);
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			seedHeightMap();
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			stepGenerateHeightMap();
		}
		if (Input.GetKeyDown(KeyCode.B)) {
			step = Mathf.Min(tHeight, tWidth);
		}
		if (Input.GetKeyDown(KeyCode.N)) {
			step = Mathf.Min(tHeight, tWidth);
			while (step > 1) {
				stepGenerateHeightMap();
			}
			Debug.Log("Done generating height map");
		}
		if (Input.GetKeyDown(KeyCode.M)) {
			randomizeHeightMap(randomPoints);
		}
	}

	private void stepGenerateHeightMap() {
		if (step > 1) {
			float startTime = Time.time;
			int halfStep = Mathf.FloorToInt(step/2.0f);
			float[,] heightMap = tData.GetHeights(0, 0, tWidth, tHeight);
			for (int row = 0; row < tHeight-step; row += step) {
				for (int col = 0; col < tWidth-step; col += step) {
					float tl = heightMap[row, col];
					float tr = heightMap[row, col+step];
					float bl = heightMap[row+step, col];
					float br = heightMap[row+step, col+step];

					// Diamond square
					heightMap[row, col+halfStep] = 0.5f * (tl + tr);
					heightMap[row+step, col+halfStep] = 0.5f * (bl + br);
					heightMap[row+halfStep, col] = 0.5f * (tl + bl);
					heightMap[row+halfStep, col+step] = 0.5f * (tr + br);
					heightMap[row+halfStep, col+halfStep] = 0.25f * (tl + tr + bl + br);

					// Smooth the in between stuff
					/*if (halfStep > 1) {
						for (int n = 1; n < step; n++) {
							for (int m = 1; m < step; m++) {
								if (n != halfStep && m != halfStep) {
									heightMap[row+n, col+m] = (n/step) * (heightMap[row,col], )
								}
							}
						}
					}*/
				}
			}
			step = halfStep;
			float midTime = Time.time;
			tData.SetHeights(0, 0, heightMap);
			float endTime = Time.time;
			Debug.Log("(Step = " + step.ToString() + ") Did a step. Took " + (midTime - startTime).ToString() + " to compute, took " + (endTime - midTime).ToString() + " to apply.");
		}
	}

	private void randomizeHeightMap(int nn) {
		float[,] heightMap = tData.GetHeights(0, 0, tWidth, tHeight);
		for (int i=0; i<50; i++) {
			heightMap[rnd.Next(0,tHeight-1), rnd.Next(0,tWidth-1)] = 0.01f * rnd.Next(0,100);
		}
		tData.SetHeights(0, 0, heightMap);
		Debug.Log("Inserted random heights into height map");
	}

	private void resetHeightMap() {
		float[,] heightMap = tData.GetHeights(0, 0, tWidth, tHeight);
		for (int i = 0; i < tHeight; i++) {
			for (int j = 0; j < tWidth; j++) {
				heightMap[i,j] = 0.0f;
			}
		}
		tData.SetHeights(0, 0, heightMap);
	}

	private void seedHeightMap() {
		float[,] heightMap = tData.GetHeights(0, 0, tWidth, tHeight);
		/*for (int i = 0; i < 10; i++) {

		}*/

		heightMap[0,0] = 0.01f * rnd.Next(0,100);
		heightMap[0,tWidth-1] = 0.01f * rnd.Next(0,100);
		heightMap[tHeight-1,0] = 0.01f * rnd.Next(0,100);
		heightMap[tHeight-1,tWidth-1] = 0.01f * rnd.Next(0,100);

		int newStep = Mathf.FloorToInt(step/2.0f);
		heightMap[newStep, newStep] = 0.01f*rnd.Next(0,100);

		tData.SetHeights(0, 0, heightMap);
	}
	
	/*
	public int resolution = 1025;
	public bool doAll = false;
	void Start () {
		//FixResolution();
		var tdata = new TerrainData();
		float size = 100.0f / resolution * 32.0f;
		tdata.size = new Vector3(size, 20.0f, size);
		tdata.heightmapResolution = resolution;
		terrain = Terrain.activeTerrain; //Terrain.CreateTerrainGameObject(tdata).GetComponent(Terrain);
		//var t = Camera.main.transform;
		//t.position = new Vector3(120,31,-5);
		//t.localEulerAngles = new Vector3(27,310,0);
		//var lo = new GameObject();
		//Light l = lo.AddComponent(Light);
		//l.type = LightType.Directional;
		//lo.transform.localEulerAngles = new Vector3(40,20,0);
	}
	
	void Update(){
		TerrainData tdata = terrain.terrainData;
		var width = tdata.heightmapWidth;
		var height = tdata.heightmapHeight;
		if(!doAll){
			width /= 8;
			height /= 8;
		}
		float[,] heights = new float[height, width];
		System.Random r = new System.Random();
		for(var y=0; y<height; y++) {
			for(var x=0; x<width; x++) {
				//heights[y,x]=0.01f * r.Next(0,100);
				heights[y,x] = Mathf.Sin(Time.time / (x));
			}
		}
		tdata.SetHeights(0,0,heights);
	}
	
	private void FixResolution(){
		var c = 0;
		while(resolution > 0){
			resolution = resolution >> 1;
			c++;
		}
		resolution = Mathf.FloorToInt(Mathf.Pow(2, c - 1) + 1);
		if(resolution > 513) resolution = 513;
		if(resolution < 33) resolution = 33;
	}*/

	/*void Start () {
		makeTerrain();
	}

	void Update () {
	
	}

	private void makeTerrain() {
		TerrainData tData = new TerrainData();
		tData.SetDetailResolution(2048,8);
		tData.baseMapResolution = 1024;
		tData.heightmapResolution = 128;
		tData.size = new Vector3(500,20,500);
		//var heightData = tData.GetHeights(0,0,128,128);
		//heightData[127, 127] = 10.0f;
		//tData.SetHeights(0,0,heightData);
		tData.SetHeights(0, 0, generateHeightArray(tData.GetHeights(0, 0, 128, 128)));
		GameObject tGameObject = Terrain.CreateTerrainGameObject(tData);
		tGameObject.name = "GennedTerrain";
		tGameObject.transform.position=new Vector3(0.0f, 0.0f, 0.0f);
	}

	private float[,] generateHeightArray(float[,] initialHeightMap) {
		float[,] output = initialHeightMap;
		int height = output.GetLength(0);
		int width = output.GetLength(1);
		int delta = Mathf.Min(height, width);

		// ----- Set up timing
		float startTime = Time.time;
		float endTime = Time.time;

		// ----- Seed some random heights
		Debug.Log("Seeding random heights");
		output = seedRandomHeights(output, width, height, 2);
		endTime = Time.time - startTime;
		Debug.Log("Took " + endTime.ToString());

		// ----- Perform the diamond square algorithm knock-off
		Debug.Log("Beginning to generate the height map...");
		output = diamondSquareAlgorithm(output, width, height, delta);
		endTime = Time.time - endTime;
		Debug.Log("Took " + endTime.ToString());

		return output;
	}

	private float[,] seedRandomHeights(float[,] heightMap, int width, int height, int step, int seed = 1234) {
		System.Random r = new System.Random(seed);
		float[,] output = heightMap;
		for (int i = 0; i < 10; i++) {
			int stepWidth = r.Next(0, Mathf.FloorToInt(width/step));
			int stepHeight = r.Next(0, Mathf.FloorToInt(height/step));
			float randHeight = 0.01f * r.Next(0,100);
			output[step*stepHeight, step*stepWidth] = randHeight;
			Debug.Log("Seeding height " + randHeight.ToString() + " at position (" + step.ToString() + "*" + (stepHeight).ToString() + "=" + 
			          (step*stepHeight).ToString() + ", " + step.ToString() + "*" + (stepWidth).ToString() + "=" + (step*stepWidth).ToString() + ")");
		}
		return output;
	}

	private float[,] diamondSquareAlgorithm(float[,] heightMap, int width, int height, int delta) {
		float[,] output = heightMap;
		int sanity = 0;
		while (delta >= 2) {
			sanity++;
			int newDelta = Mathf.FloorToInt(0.5f * delta);
			for (int row = 0; row < (height - delta); row += delta) {
				for (int col = 0; col < (width - delta); col += delta) {
					output[row + newDelta, col + newDelta] = 0.25f * (output[row,col] + output[row+delta,col] + output[row,col+delta] + output[row+delta,col+delta]);
				}
			}
			delta = newDelta;
		}
		return output;
	}*/
}


