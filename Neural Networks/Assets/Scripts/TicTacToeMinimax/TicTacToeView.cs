using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct PlayersMapData
{
    public char player;
    public Transform prefab;
}

public class TicTacToeView : MonoBehaviour
{
    public float spacing;

    public TextMeshProUGUI winnerText;

    public Slot slotPrefab;

    public PlayersMapData[] players;

    private Transform[] board;

    public void Init(int size)
    {
        var newSpacing = slotPrefab.transform.localScale.x + spacing;

        board = new Transform[size * size];

        var boardParent = new GameObject("Board").transform;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var position = new Vector3(i * newSpacing, j * newSpacing, 0);
                position.x -= newSpacing;

                var slot = Instantiate(slotPrefab, position, Quaternion.identity, boardParent);

                var index = Array2DUtils.ToIndex(i, j, size);
                board[index] = slot.transform;
                slot.index = index;
            }
        }

    }

    public void Move(int cell, int player)
    {
        var position = board[cell].position;

        var prefab = players.Where(x => x.player == player).FirstOrDefault().prefab;

        Instantiate(prefab, position, Quaternion.identity);
    }

    public void ShowWinner(char winner)
    {
        winnerText.text = winner.ToString();
    }
}