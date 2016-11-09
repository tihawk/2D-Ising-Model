using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

	int monteCarloSteps;
	Text monteCarloStepsText;
	Text energyText;
	Text magnetisationText;
	Text specificHeatText;
	Text susceptibilityText;

	string monteCarloStepsTextString;
	string energyTextString;
	string magnetisationTextString;
	string specificHeatTextString;
	string susceptibilityTextString;

	// Use this for initialization
	void Start () {

		monteCarloStepsText = transform.GetChild (0).GetComponent<Text> ();
		energyText = transform.GetChild (1).GetComponent<Text> ();
		magnetisationText = transform.GetChild (2).GetComponent<Text> ();
		specificHeatText = transform.GetChild (3).GetComponent<Text> ();
		susceptibilityText = transform.GetChild (4).GetComponent<Text> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		monteCarloSteps = IsingModelController.Instance.isingModel.monteCarloSteps;

		monteCarloStepsTextString = ("Running for T = " + IsingModelController.temperature.ToString("F5") +
			" N = " + IsingModelController.N + "\nCurrent Monte Carlo steps: " + monteCarloSteps);
		energyTextString = ("avg. Energy per spin: " +
			(IsingModelController.Instance.isingModel.averageEnergyTotal () / IsingModelController.N).ToString ("F5"));
		magnetisationTextString = ( "avg. Magnetisation: " + 
			(IsingModelController.Instance.isingModel.averageMagnetisation()).ToString ("F5") );
		specificHeatTextString = ("avg. Specific Heat per spin: " +
			(IsingModelController.Instance.isingModel.specificHeatTotal () / IsingModelController.N).ToString ("F5"));
		susceptibilityTextString = ("avg. Susceptibility: " +
			(IsingModelController.Instance.isingModel.AverageSusceptibility ()).ToString ("F5"));

		monteCarloStepsText.text = monteCarloStepsTextString;
		energyText.text = energyTextString;
		magnetisationText.text = magnetisationTextString;
		specificHeatText.text = specificHeatTextString;
		susceptibilityText.text = susceptibilityTextString;
	
	}
}
