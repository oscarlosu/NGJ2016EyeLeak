﻿using UnityEngine;

/// <summary>
/// Creating instance of particles from code with no effort
/// </summary>
public class Puff : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	public static Puff Instance;
	
	public ParticleSystem PuffParticle;

	
	void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of Puff!");
		}
		
		Instance = this;
	}
	
	/// <summary>
	/// Create an explosion at the given location
	/// </summary>
	/// <param name="position"></param>
	public void Explosion(Vector3 position)
	{
		instantiate(PuffParticle, position);

	}
	
	/// <summary>
	/// Instantiate a Particle system from prefab
	/// </summary>
	/// <param name="prefab"></param>
	/// <returns></returns>
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position + new Vector3(0, 3, 0),
			Quaternion.identity
			) as ParticleSystem;
		
		// Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);
        return newParticleSystem;
	}
}