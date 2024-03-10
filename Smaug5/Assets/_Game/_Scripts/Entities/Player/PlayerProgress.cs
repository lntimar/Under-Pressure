using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerProgress : MonoBehaviour
{
    #region Vari�veis Globais
    private enum WriteType
    {
        CheckPoint,
        LoadPoint
    }

    // Unity Inspector:
    [Header("Configura��es:")]

    [Header("Transi��o Restart:")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float loadTime;

    // Refer�ncias:
    private CollisionLayersManager _collisionLayersManager;

    // Componentes:
    private PlayerStats _playerStats;

    // CheckPoint Atual:
    private static CheckPoint _currentCheckPoint;
    private static LoadPoint _currentLoadPoint;
    #endregion

    #region Fun��es Unity
    private void Awake()
    {
        _collisionLayersManager = FindObjectOfType<CollisionLayersManager>();
        _playerStats = GetComponent<PlayerStats>();

        // Carregar Valores no PlayerLog
        LoadStats();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == _collisionLayersManager.CheckPoint.Index)
            ChangeCheckPoint(col.gameObject.GetComponent<CheckPoint>());
        else if (col.gameObject.layer == _collisionLayersManager.LoadPoint.Index)
            LoadNewScene(col.gameObject.GetComponent<LoadPoint>());
    }
    #endregion

    #region Fun��es Pr�prias
    public void Restart() => TransitionManager.Instance().Transition(SceneManager.GetActiveScene().name, transitionSettings, loadTime);

    private void LoadNewScene(LoadPoint newLoadPoint)
    {
        _currentLoadPoint = newLoadPoint;
        WriteStats(WriteType.LoadPoint);
        SceneManager.LoadScene(_currentLoadPoint.SceneName);
    } 

    private void ChangeCheckPoint(CheckPoint newCheckPoint)
    {
        _currentCheckPoint = newCheckPoint; 
        // Registrar valores no PlayerLog
        WriteStats(WriteType.CheckPoint);
    }

    private void LoadStats()
    {
        // Carregue:
        // Posi��o do CheckPoint
        transform.position = new Vector3(PlayerPrefs.GetFloat("playerX"),PlayerPrefs.GetFloat("playerY"), PlayerPrefs.GetFloat("playerZ"));
        // �ltimo valor de HP
        _playerStats.Health = PlayerPrefs.GetInt("playerHP");
        // Quantidade Muni��o
        // Itens (A Fazer)
    }

    private void WriteStats(WriteType writeType)   
    {
        // Registre:
        if (writeType == WriteType.CheckPoint)
        {
            // Posi��o do CheckPoint
            PlayerPrefs.SetFloat("playerX", _currentCheckPoint.checkPointSpawnPos.position.x);
            PlayerPrefs.SetFloat("playerY", _currentCheckPoint.checkPointSpawnPos.position.y);
            PlayerPrefs.SetFloat("playerZ", _currentCheckPoint.checkPointSpawnPos.position.z);
        }
        else
        {
            // Posi��o na Pr�xima Cena
            PlayerPrefs.SetFloat("playerX", _currentLoadPoint.NextPos.x);
            PlayerPrefs.SetFloat("playerY", _currentLoadPoint.NextPos.y);
            PlayerPrefs.SetFloat("playerZ", _currentLoadPoint.NextPos.z);
        }
        
        // �ltimo valor de HP
        PlayerPrefs.SetInt("playerHP", _playerStats.Health);
        // Quantidade Muni��o
        // Itens (A Fazer)
    }
    #endregion
}
