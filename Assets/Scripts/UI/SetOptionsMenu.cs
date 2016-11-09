using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SetOptionsMenu : MonoBehaviour {

	int newGridSize = IsingModelController.gridSize;
	double newTemperature = IsingModelController.temperature;
	double readTemperature;

	public void OnClick()
	{
		if (readTemperature == IsingModelController.temperature)
		{
			newTemperature = IsingModel.critTemperature;
		} else
		{
			newTemperature = readTemperature;
		}

		IsingModelController.Instance.isingModel.resetData (IsingModelController.gridSize, IsingModelController.temperature);

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

		IsingModelController.gridSize = newGridSize;
		IsingModelController.temperature = newTemperature;

	}

	public void SetGridSize(string inputFieldString)
	{

		int.TryParse (inputFieldString, out newGridSize);

	}

	public void SetTemperature(string inputFieldString)
	{
		
		double.TryParse (inputFieldString, out readTemperature);

	}

}
