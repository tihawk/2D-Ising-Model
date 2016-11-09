using System.Collections.Generic;
using System;
using UnityEngine;

public class IsingModel {

	// Critical Temperature
	public static double critTemperature = 2d / Math.Log (1d + Math.Sqrt (2d));
	// Size of the grid:
	int gridSize;
	// Number of spins:
	int N;
	// A Dictionary for the spins, contains the Vector2 position of the
	// spin, and its value (+1/-1)
	public Dictionary<Vector2, int> spins { get; protected set; }
	// Temperature of the simulation, used for computing physical properties,
	// AND for the boltzmann coefficient, thus changing che probability of
	// a successful flip:
	public double temperature = critTemperature;
	// Number of Monte Carlo steps
	public int monteCarloSteps { get; protected set; }
	// Total energy of the system
	int energy; 
	// A number of successful flips of the spin. Don't know why do we need it,
	// but it was in the pseudo-code I was using.
	int successfulFlips = 0;
	// Used for calculations of physical properties:
	double energyAccumulator = 0d;
	double energySquaredAccumulator = 0d;
	int magnetisation = 0;
	double magnetisationAccumulator = 0d;
	double magnetisationSquaredAccumulator = 0d;
	// An array for the Boltzmann factors, used for changing
	// the probability of a successful flip of the spin:
	double[] w = new double[9]; 

	// Constructor for the class:
	public IsingModel(int gridSize, double temperature)
	{
		this.temperature = temperature;
		spins = new Dictionary<Vector2, int>();
		// Set number of spins and grid size
		this.gridSize = gridSize;
		N = gridSize * gridSize;
		// Since we are going to start at minimal energy, we need
		// all spins to be pointing in the same direction,
		// in our case +1
		for (int i = 0; i < gridSize; i++)
		{
			for (int j = 0; j < gridSize; j++) {
				spins.Add (new Vector2 (i, j), 1);
			}
		}

		// ground state energy is equal to -1*the number of spins if 1D or:
		// TODO: -2*N for 2D. Why?
		energy = -2 * N;
		magnetisation = N;
		// setting two of the boltzmann array elements manually
		// Because of the lack of a magnetic field, only these coefficients,
		// coresponding to their energies, will ever occur, so we can skip the
		// calculation of the others.
		// NOTE: For the 2D Ising Model, the energies +-8, +-4 and 0 will appear, 
		// while for the 1D model, energies of +-4, +-2, and 0 appear.
		w [8] = Math.Exp (-8d / temperature);
		w [4] = Math.Exp (-4d / temperature);
		//w [2] = Math.Exp (-2d / temperature);

		resetData (gridSize, temperature);
	}

	public double averageMagnetisation()
	{
		return magnetisationAccumulator / monteCarloSteps;
	}

	public double averageEnergyTotal()
	{
		return energyAccumulator / monteCarloSteps;
	}
		
	public double specificHeatTotal()
	{

		double averageEnergySquared = energySquaredAccumulator / monteCarloSteps;
		double averageEnergy = energyAccumulator / monteCarloSteps;
		double averageEnergyVariance = averageEnergySquared - averageEnergy * averageEnergy;

		return averageEnergyVariance / (temperature * temperature);
	}

	public double AverageSusceptibility()
	{
		double averageMagnetisationSquared = magnetisationSquaredAccumulator / monteCarloSteps;
		double averageMagnetisation = magnetisationAccumulator / monteCarloSteps;
		double averageMagnetisationVariance = averageMagnetisationSquared - averageMagnetisation * averageMagnetisation;

		return averageMagnetisationVariance / (temperature * N);
	}

	public void resetData(int gridSize, double temperature)
	{
		this.gridSize = gridSize;
		N = gridSize * gridSize;
		this.temperature = temperature;

		monteCarloSteps = successfulFlips = 0;
		energyAccumulator = energySquaredAccumulator = 
			magnetisationAccumulator = magnetisationSquaredAccumulator = 0d;
	}

	public void doOneMonteCarloStep()
	{
		System.Random random = new System.Random ();
		Vector2 currentSpin = new Vector2 ();

		for (int k = 0; k < N; k++)
		{
			// Choosing a random spin from the N available ones
			// by picking an x and y value for the coordinates
			currentSpin.x = random.Next ( 0, gridSize );
			currentSpin.y = random.Next ( 0, gridSize );
			//Debug.Log ("spin position: " + x + " " + y);
			// Calculating the difference between the energy of the
			// neighbourhood before and after a test flip of the spin.
			// NOTE: The modulus helps us to implement periodic boundary
			// conditions. Check for yourselves. It's brilliant.

			int dE = 2 * spins [currentSpin] *
					(
					spins [new Vector2 ( (currentSpin.x + 1) % gridSize, currentSpin.y )] + 
					spins [new Vector2 ( (currentSpin.x - 1 + gridSize) % gridSize, currentSpin.y )] + 
					spins [new Vector2 ( currentSpin.x, (currentSpin.y + 1) % gridSize )] + 
					spins [new Vector2 ( currentSpin.x, (currentSpin.y - 1 + gridSize) % gridSize )]
					);
				
			if (dE <= 0 || w[dE] > random.Next (0, 100000001) / 100000000d)
			{				
				//Debug.Log ("random.Next: " + temp);
				// If the difference between the old and new energies
				// of the neighbourhood of spin i is less than zero, or
				// if bigger than zero, but the boltzmann coeff. for this 
				// energy is bigger than a randomly generated number between
				// 0 and 1, then we successfuly flip the spin.
				spins [currentSpin] = -spins [currentSpin];
				successfulFlips++;
				energy += dE;
				magnetisation += 2 * spins [currentSpin];
			}
			// Otherwise nothing changes.
		}
		// At the end of the cycle we just change some variables.
		energyAccumulator += energy;
		energySquaredAccumulator += energy * energy;
		magnetisationAccumulator += magnetisation;
		magnetisationSquaredAccumulator += magnetisation * magnetisation;
		monteCarloSteps++;
		//Debug.Log ("steps: " + monteCarloSteps);
		//Debug.Log( 4/Math.Log( 1d + 4d/ (energyAccumulator / (monteCarloSteps*N)) ));

	}
}
