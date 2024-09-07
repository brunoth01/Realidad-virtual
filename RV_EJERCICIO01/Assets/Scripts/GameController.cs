using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerPong : MonoBehaviour {

	public PlayerPong player;
	public Ball ball;
	public Text scoreText;

	private float gameOverTimer = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool isGameOver = ball.transform.position.z < player.transform.position.z;

		if (isGameOver == false) {
			scoreText.text = "Puntaje: " + ball.score;
		} else {
			scoreText.text = "Game over!\nTu puntaje final: " + ball.score;

			gameOverTimer -= Time.deltaTime;
			if (gameOverTimer <= 0f) {
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			}
		}
	}
}
