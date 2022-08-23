﻿using Dapper;
using gamesPlatform.Shared;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace gamesPlatform.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase, IScoreController
    {
        private NpgsqlConnection? connection { get; } = null;

        public ScoreController()
        {
            var envString = Environment.GetEnvironmentVariable("DATABASE_URL")!;
            if (envString?.Equals(string.Empty) == false)
            {
                var dbURI = new Uri(envString);
                var dbUserInfo = dbURI.UserInfo.Split(":");
                var connectionString = new NpgsqlConnectionStringBuilder
                {
                    Username = dbUserInfo.FirstOrDefault(),
                    Password = dbUserInfo.LastOrDefault(),
                    Host = dbURI.Host,
                    Port = dbURI.Port,
                    Database = dbURI.LocalPath.Substring(1),
                    SslMode = SslMode.Require,
                    TrustServerCertificate = true
                };
                connection = new NpgsqlConnection(connectionString.ToString());
                connection.Open();
            }
            else
                throw new ArgumentException("invalid DATABASE_URL");
        }

        [HttpPost("setscore")]
        public async Task<ActionResult<List<Score>>> dbAddScore(Score s)
        {
            const string query = "INSERT INTO scores (appid, scorevalue, runstart, runlength, nickname) " +
                "VALUES (@id, @scorevalue, @runstart, @runlength, @nickname);";

            var vals = new
            {
                appid = s.appID,
                scorevalue = s.scoreValue,
                runStart = s.runStart,
                runLength = s.runLength,
                nickname = s.nickname,
            };
            await connection.ExecuteAsync(query, vals);
            return Ok(vals);
        }

        [HttpGet("leaderboard/{appID}")]
        public async Task<ActionResult<List<Score>>> dbGetLeaderboard(int appID)
        {
            string query = $"SELECT * FROM scores WHERE appid=@id;";
            var vals = new { id = appID };
            var results = await connection.QueryAsync<Score>(query, vals);

            return Ok(results);
        }

        [HttpGet("scores/{scoreID}")]
        public async Task<ActionResult<Score>> dbGetScore(int scoreID)
        {
            string query = $"SELECT * FROM scores WHERE id=@id;";
            var vals = new { id = scoreID };
            var results = await connection.QueryFirstAsync<Score>(query, vals);
            return Ok(results);
        }
    }
}
