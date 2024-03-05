using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Vari�veis Globais
    // Unity Inspector:
    [Header("Configura��es:")] 

    [Header("Carregar Fase:")] 
    [SerializeField] private float loadTime;

    [Header("Transi��o Restart:")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float transitionDelay;

    // Refer�ncias:
    private CollisionLayersManager _collisionLayersManager;
    #endregion

    #region Fun��es Unity
    private void Start() => _collisionLayersManager = FindObjectOfType<CollisionLayersManager>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _collisionLayersManager.LevelTrigger.Index)
            LoadLevel(other.gameObject.GetComponent<LevelTrigger>().levelName);
    }
    #endregion

    #region Fun��es Pr�prias
    public void LoadLevel(string name)
    {
        // Carregar Pr�xima Fase
        // Mover Player para a pr�xima cena
        StartCoroutine(CallLevel(name, loadTime));
    }
    
    public void Restart() => TransitionManager.Instance().Transition(SceneManager.GetActiveScene().name, transitionSettings, transitionDelay);

    private IEnumerator CallLevel(string levelName, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(levelName);
    }
    #endregion
}
