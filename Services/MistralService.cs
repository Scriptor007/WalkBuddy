using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WalkBuddy.Models;

namespace WalkBuddy.Services
{
    public class MistralService
    {
        private readonly HttpClient _client;

        private readonly SessionRepository _sessionRepository;

        public Session CurrentSession {  get; private set; } = new();

        public string MistralAnswer { get; private set; } = string.Empty;

        public MistralService(HttpClient client, SessionRepository sessionRepository)
        {
            _client = client;
            _sessionRepository = sessionRepository;
        }

        public async Task<string> SendRequest(User user, Session session)
        {
            string payload = BuildPayload(user, session);

            var request = new
            {
                agent_id = "ag_019e10fe28457606995cc3c396988026",
                agent_version = 8,
                inputs = new[]
                {
                    new
                    {
                        role = "user",
                        content = payload
                    }
                }
            };

            string requestPayload = JsonSerializer.Serialize(request);

            var response = await _client.PostAsync(_client.BaseAddress + "/v1/conversations", new StringContent(requestPayload, Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(responseString);

            using JsonDocument doc = JsonDocument.Parse(responseString);

            string botAnswer = doc.RootElement
                .GetProperty("outputs")[0]
                .GetProperty("content")
                .GetString();

            MistralAnswer = botAnswer;
            session.PromptCount += 1;
            await _sessionRepository.SaveSessionAsync(session);
            CurrentSession = session;
            return MistralAnswer;
        }

        private string BuildPayload(User user, Session session)
        {
            string content = $@"
                (User.Username) {user.Username}
                (User.AdditionalInfo) {user.AdditionalInfo}
                (User.Interests) {user.Interests}
                (User.ChallengingStates) {user.ChallengingStates}
                (User.PositiveStates) {user.PositiveStates}
                (Session.StartFeeling) {session.StartEmotion}
                (User.Mobility) {user.Mobility}
                (Device.Gps.Latitude) none
                (Device.Gps.Longitude) none
                (Location.Context) none
                (Device.LocalDateTime) none
                (Season) none
                ".Trim();
            return content;
        }
    }
}
