using Handball.Models.Contracts;
using Handball.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Handball.Models
{
    public class Team : ITeam
    {
        private string name;
        private int pointsEarned;
        private readonly List<IPlayer> players = new List<IPlayer>();

        public Team(string name)
        {
            Name = name;
            players = new List<IPlayer>();
        }

        public string Name
        {
            get => name; private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(string.Format(ExceptionMessages.TeamNameNull));
                }
                name = value;
            }
        }

        public int PointsEarned => pointsEarned;

        public double OverallRating
        {
            get
            {
                if (players.Count == 0)
                {
                    return 0;
                }

                double totalRating = players.Sum(player => player.Rating);
                return Math.Round(totalRating / players.Count, 2);
            }
        }

        public IReadOnlyCollection<IPlayer> Players => players.AsReadOnly();

        public void Draw()
        {
            this.pointsEarned++;
            var goalkeeper = players.Where(p => p.GetType().Name == "Goalkeeper").FirstOrDefault();
            goalkeeper.IncreaseRating();
        }

        public void Lose()
        {
            this.players.ForEach(p => p.DecreaseRating());
        }

        public void SignContract(IPlayer player)
        {
            players.Add(player);
        }

        public void Win()
        {
            this.pointsEarned += 3;
            this.players.ForEach(p => p.IncreaseRating());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Team: {this.Name} Points: {this.PointsEarned}");
            sb.AppendLine($"--Overall rating: {this.OverallRating}");

            if (players.Count == 0)
            {
                sb.AppendLine("--Players: none");
            }
            else
            {
                sb.AppendLine($"--Players: {string.Join(", ", players.Select(p => p.Name))}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
