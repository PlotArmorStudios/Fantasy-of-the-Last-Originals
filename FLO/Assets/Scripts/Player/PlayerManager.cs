using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Player[] _players;

    private void Awake()
    {
        _players = FindObjectsOfType<Player>();
    }

    public void AddPlayerToGame(Controller controller)
    {
        var firstNonActivePlayer = _players
            .OrderBy(t => t.PlayerNumber)
            .FirstOrDefault(t => t.HasController == false);
        firstNonActivePlayer.InitializePlayer(controller);
    }
}