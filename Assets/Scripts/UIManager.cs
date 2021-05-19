using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Sprite[] _lives;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Text _ammoText;
    
    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";

        _restartText.gameObject.SetActive(false);

        _gameOverText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("game manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        
        _livesImage.sprite = _lives[currentLives];

        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        StartCoroutine(GameOverFlicker());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
        
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            _gameOverText.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }
    }    
}
