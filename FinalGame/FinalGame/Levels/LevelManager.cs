using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FinalGame.PlayerManager;

using RC_Framework;

namespace FinalGame.Levels
{
    class LevelManager
    {
        private List<Player> players;

        private List<Player> deadPlayers;

        private Player player;

        private bool gameActive;

        private Player winningPlayer = null;

        RC_GameStateManager gameStateManager;

        public LevelManager(List<Player> players, RC_GameStateManager manager)
        {
            this.players = players;
            this.deadPlayers = new List<Player>();
            this.gameActive = true;
            this.gameStateManager = manager;
        }
           
        public void setCurrentPlayer(int currentPlayer)
        {
            if (players[currentPlayer].isAlive()) this.player = players[currentPlayer];
        }

        public void setCurrentPlayer(Player a)
        {
            if (a.isAlive()) this.player = a;
        }

        public void setGameStatus(bool status)
        {
            this.gameActive = status;
        }

        public bool getGameStatus()
        {
            return this.gameActive;
        }

        public Player getCurrentPlayer()
        {
            return this.player;
        }

        public Player getPlayer(int i)
        {
            if (players[i].isAlive() && players[i] != null) return players[i];
            return null;
        }

        public List<Player> getPlayers()
        {
            return this.players;
        }


        public void addDeadPlayer(Player player)
        {
            this.deadPlayers.Add(player);
        }

        public void nextTurn()
        {
            int temp = this.player.getID();
            System.Diagnostics.Debug.WriteLine("TEMP: " + temp);
            temp = (temp + 1) % this.players.Count;
            System.Diagnostics.Debug.WriteLine("TEMP: " + temp);

            while (!players[temp].isAlive())
            {
                temp = (temp + 1) % this.players.Count;
                System.Diagnostics.Debug.WriteLine("LOOOP: " + temp);
            }

            this.setCurrentPlayer(temp);
        }

        public void checkEnd()
        {
            int temp = 0;
            foreach(Player p in players)
            {
                if (!p.isAlive()) temp++;
                else winningPlayer = p;
            }

            if (temp == players.Count - 1)
            {
                actionEnd();
            }
            else
            {
                nextTurn();
            }

        }

        public void actionEnd()
        {
            this.setGameStatus(false);
            WinState.wonPlayer = winningPlayer;
            WinState.leaderBoard = deadPlayers;
            gameStateManager.getLevel(4).UnloadContent();
            this.gameStateManager.setLevel(3);
        }

        public void update()
        {
           
        }
    }
}
