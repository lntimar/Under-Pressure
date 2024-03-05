using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Variáveis Globais
    // Unity Inspector:
    [Header("Configurações:")] 

    [Header("Carregar Fase:")] 
    [SerializeField] private float loadTime;

    [Header("Transição Restart:")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float transitionDelay;

    // Referências:
    private CollisionLayersManager _collisionLayersManager;
    #endregion

    #region Funções Unity
    private void Start() => _collisionLayersManager = FindObjectOfType<CollisionLayersManager>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _collisionLayersManager.LevelTrigger.Index)
            LoadLevel(other.gameObject.GetComponent<LevelTrigger>().levelName);
    }
    #endregion

    #region Funções Próprias
    public void LoadLevel(string name)
    {
        // Carregar Próxima Fase
        // Mover Player para a próxima cena
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
