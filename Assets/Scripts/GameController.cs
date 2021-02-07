using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public List<Bird> Birds;
    public List<Enemy> Enemies;

    private bool _isGameEnded = false;
    private bool _isGameFailed = false;

    private Bird _shotBird;
    public BoxCollider2D TapCollider;

    public string nextScenePath;
    public string levelName;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;

        SlingShooter.InitiateBird(Birds[0]);

        _shotBird = Birds[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeBird()
    {
        if (TapCollider != null)
        {
            TapCollider.enabled = false;
        }

        if (_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
        else
        {
            _isGameFailed = true;
            StartCoroutine(restartLevel(4));
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if (Enemies.Count == 0)
        {
            _isGameEnded = true;
            StartCoroutine(goToNextLevel(5));
        }
    }

    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }

    private IEnumerator goToNextLevel(float second)
    {
        yield return new WaitForSeconds(second);
        SceneManager.LoadScene(nextScenePath, LoadSceneMode.Single);
    }

    private IEnumerator restartLevel(float second)
    {
        yield return new WaitForSeconds(second);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }

    void OnGUI()
    {
        GUIStyle levelNameStyle = new GUIStyle();
        levelNameStyle.fontSize = 30;
        levelNameStyle.normal.textColor = Color.black;
        GUIStyle succStyle = new GUIStyle();
        succStyle.fontSize = 100;
        succStyle.normal.textColor = Color.blue;
        GUIStyle failStyle = new GUIStyle();
        failStyle.fontSize = 100;
        failStyle.normal.textColor = Color.red;


        GUI.Label(new Rect(30, 20, 100, 100), levelName, levelNameStyle);


        if (_isGameEnded)
        {
            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2, 100, 100), "SUCCESSFUL", succStyle);
        }

        if (_isGameFailed)
        {
            GUI.Label(new Rect(Screen.width / 2 - 160, Screen.height / 2, 100, 100), "FAILED", failStyle);
        }
    }
}
