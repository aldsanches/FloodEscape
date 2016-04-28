using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject PauseUI;

    private bool paused = false;

    void Start() {
        PauseUI.SetActive(false);
    }

    void Update() {
        if(Input.GetButtonDown("Pause")) {
            paused = !paused; //troca a posição de pause, se estiver no pause, sai e viceversa
        }
        if (paused) {
            PauseUI.SetActive(true);
            Time.timeScale = 0; //seta o tempo para 0, pausa o tempo de jogo.
        }
        if (!paused) {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume() {
        paused = false;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //carrega a scene atual
    }

    public void MainMenu() {
        SceneManager.LoadScene(0); //carrega a scene de index 0
    }

    public void Quit() {
        Application.Quit(); //funciona apenas no build do game, não no unity
    }
}
