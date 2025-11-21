using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class CardController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabs; // Prefabs que se van a cargar automaticamente

    public int _MaxCardTypes => prefabs.Count;

    public float CardSize = 2f;

    public UnityEvent<CardController> OnClicked; // Para cuando se le haga un click a una carata esto lo notifique

    public int CardType = -1; // Tipo de carta

    private Animator _animator; // Para dar la vuelta a las cartas

    private void Awake() // Voltear las cartas
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (CardType < 0) // No se a inicializado
        {
            CardType = UnityEngine.Random.Range(0, prefabs.Count); // Para que las cartas sean aleatorias
        }
        Instantiate(prefabs[CardType], transform.position, quaternion.identity, transform);
    }
    private void OnMouseUpAsButton()
    {
        OnClicked.Invoke(this);
    }
    public void TestAnimation()
    {
        IEnumerator AnimationCoroutine()
        {
            Reveal();
            yield return new WaitForSeconds(2); // Tiempo de espera antes de llamar al Hide
            Hide();
        }
        StartCoroutine(AnimationCoroutine());
    }

    public void Reveal() // Revelar las cartas: Carta boca arriba
    {
        _animator.SetBool(name: "Revealed", value: true);
    }
    public void Hide() // Escoder la carta: Carta boca abajo
    {
        _animator.SetBool(name: "Revealed", value: false);
    }
}