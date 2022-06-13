using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;

namespace WeddingConfirmation.Data
{
    public class GoogleSheetService
    {
        private static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private static string ApplicationName = "Google Sheets API .NET Quickstart";
        private SheetConfig _sheetConfig;

        public GoogleSheetService(IOptions<SheetConfig> sheetConfigOption)
        {
            _sheetConfig = sheetConfigOption.Value;
        }

        public void Post(Confirmation confirmation)
        {
            try
            {
                GoogleCredential credential;

                // Load client secrets.
                using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(Scopes);
                }

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                string range = "A1";

                // How the input data should be interpreted.
                SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum valueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

                // How the input data should be inserted.
                SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum insertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;

                // TODO: Assign values to desired properties of `requestBody`:
                ValueRange requestBody = new ValueRange()
                {
                    Values = new List<IList<object>>() {
                        new List<object> {
                            confirmation.Venue,
                            confirmation.Name,
                            confirmation.Relationship ?? "忘了填",
                            confirmation.Joining,
                            confirmation.NumberOfPeople ?? 0,
                            confirmation.NumberOfChildSeat,
                            confirmation.NumberOfVegetarian,
                            confirmation.Send,
                            confirmation.Email,
                            confirmation.PostalCode,
                            confirmation.Address,
                            $"'{confirmation.Phone}",
                            confirmation.Blessing
                        }
                    }
                };

                SpreadsheetsResource.ValuesResource.AppendRequest request = service.Spreadsheets.Values.Append(requestBody, _sheetConfig.SpreadSheetId, range);
                request.ValueInputOption = valueInputOption;
                request.InsertDataOption = insertDataOption;

                // To execute asynchronously in an async method, replace `request.Execute()` as shown:
                AppendValuesResponse response = request.Execute();

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}