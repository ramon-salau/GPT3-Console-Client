using System;
using System.Text;
using Newtonsoft.Json;

Console.WriteLine("Welcome to GPT3 Client");

Console.Write("Question : ");

var question = Console.ReadLine();

if (!string.IsNullOrEmpty(question))
{
    HttpClient client = new HttpClient();

    client.DefaultRequestHeaders.Add("authorization", "Bearer auth-token");

    var content = new StringContent("{\"model\": \"text-davinci-001\", \"prompt\": \"" + question + "\",\"temperature\": 1,\"max_tokens\": 100}",
        Encoding.UTF8, "application/json");

    HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

    string responseString = await response.Content.ReadAsStringAsync();

    try
    {
        var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

        string answer = GuessCommand(dyData!.choices[0].text);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Answer : {answer}");
        Console.ResetColor();

    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Unable to Serialize JSON : {ex.Message}");
        Console.ResetColor();
    }
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Error : Kindly ask a valid question.");
    Console.ResetColor();
}

Console.Write("\n<--Press enter to exit-->");
Console.ReadLine();


static string GuessCommand(string raw)
{
    var lastIndex = raw.LastIndexOf('\n');

    string guess = raw.Substring(lastIndex + 1);

    TextCopy.ClipboardService.SetText(guess);

    return guess;
}
