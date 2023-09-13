using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    SideSwapping _sideSwapping;
    private bool _gameFinished => _ghostsDead || _timeIsUp;
    private bool _timeIsUp;
    private bool _ghostsDead => false;
    [SerializeField] float GameTime = 120f;
    private float currentTime;
    public TMPro.TMP_Text WinText;
    void Awake()
    {
        _sideSwapping = GetComponent<SideSwapping>();
        _sideSwapping.enabled = true;
        StartCoroutine(GameCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameCoroutine()
    {
        currentTime = GameTime;
        while (!_gameFinished)
        {
            yield return new WaitForFixedUpdate();
            currentTime -= Time.fixedDeltaTime;
            if (currentTime <= 0f)
                _timeIsUp = true;
            WinText.text = $"{currentTime} seconds";
        }

        WinText.text = _ghostsDead ? "Soldiers Win" : "Ghosts Win!";
        yield return new WaitForSeconds(20);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
