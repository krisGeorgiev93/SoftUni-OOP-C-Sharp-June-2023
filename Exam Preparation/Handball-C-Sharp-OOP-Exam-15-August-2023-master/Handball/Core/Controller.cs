using Handball.Core.Contracts;
using Handball.Models.Contracts;
using Handball.Models;
using Handball.Repositories;
using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Handball.Utilities.Messages;
using System.Numerics;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace Handball.Core
{
    public class Controller : IController
    {
        private PlayerRepository players;
        private TeamRepository teams;

        public Controller()
        {
            players = new PlayerRepository();
            teams = new TeamRepository();
        }

        public string LeagueStandings()
        {
            var sortedTeams = teams.Models.OrderByDescending(t => t.PointsEarned).ThenByDescending(t => t.OverallRating).ThenBy(t => t.Name);

            StringBuilder league = new StringBuilder();

            league.AppendLine("***League Standings***");

            foreach (var team in sortedTeams) 
            {
                league.AppendLine(team.ToString());
            }
            
            return league.ToString().TrimEnd();
        }

        public string NewContract(string playerName, string teamName)
        {
            var currentPlayer = players.GetModel(playerName);
            var currentTeam = teams.GetModel(teamName);

            if (players.ExistsModel(playerName) == false)
            {
                return string.Format(OutputMessages.PlayerNotExisting, playerName, nameof(PlayerRepository));
            }
            else if (teams.ExistsModel(teamName) == false)
            {
                return string.Format(OutputMessages.TeamNotExisting, teamName, nameof(TeamRepository));
            }
            else if (currentPlayer.Team is not null)
            {
                return string.Format(OutputMessages.PlayerAlreadySignedContract, currentPlayer.Name, currentPlayer.Team);
            }

            currentTeam.SignContract(currentPlayer);
            currentPlayer.JoinTeam(teamName);
            return string.Format(OutputMessages.SignContract, playerName, teamName);
        }

        public string NewGame(string firstTeamName, string secondTeamName)
        {
            var homeTeam = teams.GetModel(firstTeamName);
            var awayTeam = teams.GetModel(secondTeamName);

            if (homeTeam.OverallRating > awayTeam.OverallRating)
            {
                homeTeam.Win();
                awayTeam.Lose();
                return string.Format(OutputMessages.GameHasWinner, homeTeam.Name, awayTeam.Name);
            }
            else if (homeTeam.OverallRating < awayTeam.OverallRating)
            {
                homeTeam.Lose();
                awayTeam.Win();
                return string.Format(OutputMessages.GameHasWinner, awayTeam.Name, homeTeam.Name);
            }
            else
            {
                homeTeam.Draw();
                awayTeam.Draw();
                return string.Format(OutputMessages.GameIsDraw, homeTeam.Name, awayTeam.Name);
            }
        }

        public string NewPlayer(string typeName, string name)
        {
            if (players.ExistsModel(name))
            {
                return string.Format(OutputMessages.PlayerIsAlreadyAdded, name, nameof(PlayerRepository), players.GetModel(name).GetType().Name);
            }

            IPlayer player;

            if (typeName == nameof(Goalkeeper))
            {
                player = new Goalkeeper(name);
            }

            else if (typeName == nameof(CenterBack))
            {
                player = new CenterBack(name);
            }

            else if (typeName == nameof(ForwardWing))
            {
                player = new ForwardWing(name);
            }

            else
            {
                return string.Format(OutputMessages.InvalidTypeOfPosition, typeName);
            }

            players.AddModel(player);
            return string.Format(OutputMessages.PlayerAddedSuccessfully, name);
        }

        public string NewTeam(string name)
        {
            if (teams.ExistsModel(name))
            {
                return string.Format(OutputMessages.TeamAlreadyExists, name, nameof(TeamRepository)); 
            }

            Team team = new Team(name);
            teams.AddModel(team);
            return string.Format(OutputMessages.TeamSuccessfullyAdded, team.Name, nameof(TeamRepository)); 
        }

        public string PlayerStatistics(string teamName)
        {
            var currentTeam = teams.GetModel(teamName);

            var sortedPlayers = currentTeam.Players.OrderByDescending(p => p.Rating).ThenBy(p => p.Name);

            StringBuilder stats = new StringBuilder();

            stats.AppendLine($"***{teamName}***");

            foreach (var player in sortedPlayers)
            {
                stats.AppendLine(player.ToString());
            }

            return stats.ToString().TrimEnd();
        }
    }
}
