using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField] [Range(1f, 10f)] private float moveSpeed; // Velocità di movimento del cubo
    [SerializeField] [Range(25f, 30f)] private float maxYPosition; // Massima altezza del movimento verso l'alto
    [SerializeField] [Range(20f, 27f)] private float minYPosition; // Minima altezza del movimento verso il basso
    [SerializeField] private bool movingUp = true; // Flag per direzione di movimento

	void Update()
	{
		// Verifica la direzione del movimento
		if (transform.position.y >= maxYPosition)
		{
			movingUp = false;
		}
		else if (transform.position.y <= minYPosition)
		{
			movingUp = true;
		}
		
		if (movingUp)
		{
			transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
		}
		else
		{
			transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
		}
	}
}
