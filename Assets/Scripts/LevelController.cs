using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Serializable]
    public class LevelData
    {
        public int Columns;
        public int Rows;
        public int Difficulty;
        public int Movements;
    }

    [SerializeField]
    private CardController _cardPrefabs;

    [Header("UI References")]
    [SerializeField]
    private TMP_Text _levelText; // Para indicar al jugador el nivel
    [SerializeField]
    private TMP_Text _movementsText; // Para indicar al jugador los movimientos posibles
    [SerializeField]
    private GameObject _gameOverButton;

    [Header("LevelData")]
    [SerializeField]
    private List<LevelData> _levels = new List<LevelData>();

    private List<CardController> _cards = new List<CardController>();
    private CardController _activeCard; // Carta activa
    private int _movementsUsed = 0;
    private bool _blockInput = true;
    private int _level = 0;

    private void Start()
    {
        /*_level = PlayerPrefs.GetInt("Level", 0);
        StartLevel();*/
        if (_level < 0 || _level >= _levels.Count)
        {
            _level = 0; // Si es inválido, forzamos el reinicio al Nivel 0.
            PlayerPrefs.SetInt("Level", _level);
        }

        StartLevel();
    }

    public void StartLevel()
    {
        _gameOverButton.SetActive(false);
        Debug.Assert((_levels[_level].Rows * _levels[_level].Columns) % 2 == 0); // Cartas pares

        if (_levels[_level].Difficulty > _cardPrefabs._MaxCardTypes)
        {
            _levels[_level].Difficulty = math.min(_levels[_level].Difficulty, _cardPrefabs._MaxCardTypes);
            Debug.Assert(false);
        }
        _cards.ForEach(c => Destroy(c.gameObject));
        _cards.Clear();

        List<int> allTypes = new List<int>();
        for(int i = 0; i < _cardPrefabs._MaxCardTypes; ++i)
        {
            allTypes.Add(i);
        }

        List<int> gameTypes = new List<int>();
        for(int i = 0; i < _levels[_level].Difficulty; ++i)
        {
            int chosenType = allTypes[UnityEngine.Random.Range(0, allTypes.Count)];
            allTypes.Remove(chosenType); // Eliminas cada tipo despues de que se hallan colocado 2 cartas de este tipo
            gameTypes.Add(chosenType);
        }

        List<int> chosenTypes = new List<int>();
        for (int i = 0; i < (_levels[_level].Rows * _levels[_level].Columns) / 2; ++i)
        {
            int chosenType = gameTypes[UnityEngine.Random.Range(0, gameTypes.Count)];
            chosenTypes.Add(chosenType);
            chosenTypes.Add(chosenType);
        }

        Vector3 offset = new Vector3((_levels[_level].Columns - 1) * _cardPrefabs.CardSize, (_levels[_level].Rows - 1)* _cardPrefabs.CardSize, 0f) * 0.5f;

        for (int y = 0; y < _levels[_level].Rows; ++y)
        {
            for(int x = 0; x < _levels[_level].Columns; ++x)
            {
                Vector3 position = new Vector3(x * _cardPrefabs.CardSize, y * _cardPrefabs.CardSize, 0f);
                var card = Instantiate(_cardPrefabs, position - offset , Quaternion.identity);
                card.CardType = chosenTypes[UnityEngine.Random.Range(0, chosenTypes.Count)];
                chosenTypes.Remove(card.CardType);
                card.OnClicked.AddListener(OnCardClicked);
                _cards.Add(card);
            }
        }
        _blockInput = false;
        _movementsUsed = 0;
        _levelText.text = $"Level: {_level +1}";
        _movementsText.text = $"Moves: {_levels[_level].Movements}";
    }

    private void OnCardClicked(CardController card)
    {
        if (_blockInput)
        {
            return;
        }

        _blockInput = true;

        if(_activeCard == null)
        {
            StartCoroutine(SelectCard(card));
            return;
        }
        _movementsUsed++; // Por cada movimiento usado el contador aumenta en uno
        _movementsText.text = $"Moves: {_levels[_level].Movements - _movementsUsed}";

        if (card.CardType == _activeCard.CardType)
        {
            StartCoroutine (Score(card));
            return;
        }
        StartCoroutine(Fail(card));
    }

    private IEnumerator SelectCard(CardController card)
    {
        _activeCard = card;
        _activeCard.Reveal();
        yield return new WaitForSeconds(0.5f);
        _blockInput = false;
    }

    private IEnumerator Score(CardController card) // Cuando hallas acertado en ambas cartas
    {
        card.Reveal();
        yield return new WaitForSeconds(1f);
        _cards.Remove(_activeCard);
        _cards.Remove(card);
        Destroy(card.gameObject);
        Destroy(_activeCard.gameObject);
        _activeCard = null;
        if(_cards.Count < 1)
        {
            Win();
            yield break;
        }
        if (_movementsUsed >= _levels[_level].Movements)
        {
            Lose();
            yield break; // Equivalente a un return
        }
        _blockInput = false;
    }
    private IEnumerator Fail(CardController card) // Cuando el jugador halla fallado
    {
        card.Reveal();
        yield return new WaitForSeconds(1f);
        _activeCard.Hide();
        card.Hide();
        _activeCard = null;
        yield return new WaitForSeconds(0.5f);
        if(_movementsUsed >= _levels[_level].Movements)
        {
            Lose();
            yield break; // Equivalente a un return
        }
        _blockInput = false;
    }
    private void Win()
    {
        _level++;
        if(_level >= _levels.Count)
        {
            _level = 0;
        }
        PlayerPrefs.SetInt("Level", _level);
        Debug.Log("Victory");
        _gameOverButton.SetActive(true);
    }
    private void Lose()
    {
        Debug.Log("Defeat");
        _gameOverButton.SetActive(true);
    }
    public void RestartOrNextLevel()
    {
        // Oculta el botón de Game Over
        _gameOverButton.SetActive(false);
        StartLevel();
    }
}