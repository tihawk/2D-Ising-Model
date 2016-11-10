using UnityEngine;
using System.Collections;
using AwokeKnowing.GnuplotCSharp;

public class IsingModelController : MonoBehaviour {

	public static IsingModelController Instance { get; protected set; }
	public IsingModel isingModel { get; protected set; }
	public static int gridSize = 3;
	public static int N { get; protected set; }
	public static double temperature = 0;

	// Use this for initialization
	void OnEnable () {

		Instance = this;
		if (temperature == 0)	temperature = IsingModel.critTemperature;
		N = gridSize * gridSize;
		isingModel = new IsingModel (gridSize, temperature);

	}
	
	// Update is called once per frame
	void Update () {

		isingModel.doOneMonteCarloStep ();

	}
}
