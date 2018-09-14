using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombie : MonoBehaviour {
	public GameObject zombiePrefab;
	public Transform[] spawnPoints;
	public Transform player;
	Vector3 currentPosition;
	public float spawnTime = 5f;
	int wave = 0;
	int [] maxNumberZombies; 
	int currentNumberZombies = 0;
	//Constantes
	public const int NUMBER_ZOMBIES_W1 = 15;
	public const int NUMBER_ZOMBIES_W2 = 25;
	public const int NUMBER_ZOMBIES_W3 = 35;
	public const int RANGE_SPAWN = 2;

	// Use this for initialization
	void Start () {
		maxNumberZombies = new int[3]{NUMBER_ZOMBIES_W1,NUMBER_ZOMBIES_W2,NUMBER_ZOMBIES_W3};
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	void Spawn(){
		Debug.Log (wave);
		if (currentNumberZombies >= maxNumberZombies [wave]) {
			if (Target.count == 0) {
				if (wave == 2) {
					Debug.Log ("Game Over WIN"); //Fim do jogo
				} 
				else {
					currentNumberZombies = 0;	//Resetar count de spawn
					Invoke ("StartWave", Random.Range(2f,3f));
				}
			}
			return;
		}

		Debug.Log ("Spawnando zombie");
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		currentPosition.x = spawnPoints [spawnPointIndex].position.x + Random.insideUnitSphere.x * RANGE_SPAWN;
		currentPosition.z = spawnPoints [spawnPointIndex].position.z + Random.insideUnitSphere.z * RANGE_SPAWN;
		currentPosition.y = spawnPoints [spawnPointIndex].position.y;
		Instantiate (zombiePrefab, currentPosition, spawnPoints [spawnPointIndex].rotation);
		zombiePrefab.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().SetTarget(player);
		currentNumberZombies++;
	}

	void StartWave()
	{
		wave++; 					//nova wave
	}
}
