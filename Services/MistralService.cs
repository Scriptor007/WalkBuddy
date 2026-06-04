#if ANDROID
using AndroidX.Core.App;
#endif
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using WalkBuddy.Models;
using WalkBuddy.Utilities;

namespace WalkBuddy.Services
{
    public class MistralService
    {
        private readonly HttpClient _client;

        private readonly SessionRepository _sessionRepository;

        public string MistralAnswer { get; private set; } = string.Empty;

        public MistralService(HttpClient client, SessionRepository sessionRepository)
        {
            _client = client;
            _sessionRepository = sessionRepository;
        }

        public async Task<string> SendRequest(User user, Session session)
        {
            CoordinatesDto coordinates = await GetCoordinates();

            string contentText = BuildPayload(user, session, coordinates);

            var request = new
            {
                agent_id = "ag_019e10fe28457606995cc3c396988026",
                agent_version = 23,
                inputs = new[] { new { role = "user", content = contentText } }
            };

            string requestPayload = JsonSerializer.Serialize(request);

            var response = await _client.PostAsync(_client.BaseAddress + "v1/conversations", new StringContent(requestPayload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            JsonNode? root = JsonNode.Parse(responseString);

            string? conversation_id = root?["conversation_id"]?.ToString();
            JsonArray? output = root?["outputs"]?.AsArray();
            string? botAnswer = output?[0]?["content"]?.ToString();

            if (string.IsNullOrEmpty(botAnswer))
            {
                throw new Exception("Server reponse is empty");
            }

            Preferences.Set("conversation_id", conversation_id);

            session.PromptCount++;
            await _sessionRepository.SaveSessionAsync(session);

            MistralAnswer = botAnswer ?? "Server is currently not available";
            return MistralAnswer;
        }

        public async Task<string> GetNewPrompt(string? checkIn = null)
        {
            int id = Preferences.Get("active_session_id", 0);
            Session? currSession = await _sessionRepository.GetSessionAsync(id);

            string conversationId = Preferences.Get("conversation_id", "");

            string contentText = checkIn != null ? "(New Prompt) Give me another prompt" : "(Check In)" + checkIn;

            var request = new
            {
                inputs = new[] { new { role = "user", content = contentText } }
            };

            string requestPayload = JsonSerializer.Serialize(request);

            var response = await _client.PostAsync(_client.BaseAddress + "v1/conversations/" + conversationId, new StringContent(requestPayload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            JsonNode? root = JsonNode.Parse(responseString);

            string? conversation_id = root?["conversation_id"]?.ToString();
            JsonArray? output = root?["outputs"]?.AsArray();
            string? botAnswer = output?[0]?["content"]?.ToString();

            if (string.IsNullOrEmpty(botAnswer))
            {
                throw new Exception("Server reponse is empty");
            }

            Preferences.Set("conversation_id", conversation_id);

            currSession.PromptCount++;
            await _sessionRepository.SaveSessionAsync(currSession);

            MistralAnswer = botAnswer ?? "Server is currently not available";
            return MistralAnswer;
        }

        private async Task<CoordinatesDto> GetCoordinates()
        {
#if ANDROID
            Android.Content.PM.Permission status = ActivityCompat.CheckSelfPermission(Platform.AppContext, Android.Manifest.Permission.AccessCoarseLocation);
            if (status == Android.Content.PM.Permission.Granted)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    Placemark? placemark = placemarks?.FirstOrDefault();
                    string? context = null;

                    if (placemark != null)
                    {
                        context = $@"
                            FeatureName: {placemark.FeatureName}
                            Locality: {placemark.Locality}
                            PostalCode: {placemark.PostalCode}
                            SubAdminArea: {placemark.SubAdminArea}
                            SubLocality: {placemark.SubLocality}
                            Thoroughfare: {placemark.Thoroughfare}
                            SubThoroughfare: {placemark.SubThoroughfare}
                            ".Trim();
                    }

                    return new CoordinatesDto(location.Longitude, location.Latitude, context);
                }
            }
# endif     
            return new CoordinatesDto();
        }

        private string BuildPayload(User user, Session session, CoordinatesDto coordinates)
        {
            string latitude = coordinates.Latitude.HasValue ? coordinates.Latitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "none";
            string longitude = coordinates.Longitude.HasValue ? coordinates.Longitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "none";
            string context = coordinates.Context ?? "none";
            string dateTime = DateTime.Now.ToString("s", System.Globalization.CultureInfo.InvariantCulture);

            string content = $@"
                (User.Username) {user.Username}
                (User.AdditionalInfo) {user.AdditionalInfo}
                (User.Interests) {user.InterestsString}
                (User.ChallengingStates) {user.ChallengingString}
                (User.PositiveStates) {user.PositiveString}
                (Session.StartFeeling) {session.StartEmotion}
                (User.Mobility) {user.Mobility}
                (Device.Gps.Latitude) {latitude}
                (Device.Gps.Longitude) {longitude}
                (Location.Context) {context}
                (Device.LocalDateTime) {dateTime}
                ".Trim();
            return content;
        }
    }
}
