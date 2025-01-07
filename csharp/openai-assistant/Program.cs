using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private const string API_KEY = "<Open AI Keys>";
    private const string ENDPOINT = "<Open AI Endpoint>";

    static async Task Main(string[] args)
    {
        await PromptUser();
    }

    static async Task PromptUser()
    {
        Console.WriteLine("Dime tu pregunta:");
        var question = Console.ReadLine();
        if (!string.IsNullOrEmpty(question))
        {
            await AskQuestion(question);
        }
        else
        {
            Console.WriteLine("La pregunta no puede estar vacía.");
            await PromptUser();
        }   
    }

    static async Task AskQuestion(string question)
    {
        var payload = new
        {
            messages = new object[]
            {
                new {
                    role = "system",
                    content = new object[] {
                        new {
                            type = "text",
                            text = "You are an AI assistant that helps people find information."
                        }
                    }
                },
                new {
                    role = "user",
                    content = new object[] {
                        new {
                            type = "text",
                            text = question
                        }
                    }
                }
            },
            temperature = 0.7,
            top_p = 0.95,
            max_tokens = 800,
            stream = false
        };

        await SendRequest(payload);
        await PromptUser();
    }

    static async Task SendRequest(object payload)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("api-key", API_KEY);

            var response = await httpClient.PostAsync(ENDPOINT, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                var responseData = jsonObject?.choices?[0]?.message?.content;
                var completionTokens = jsonObject?.usage.completion_tokens;
                var prompt_tokens = jsonObject?.usage.prompt_tokens;
                var total_tokens = jsonObject?.usage.total_tokens;

                if (responseData != null)
                {
                    Console.WriteLine(responseData);
                    Console.WriteLine("--------------------");
                    Console.WriteLine($"Completion tokens: {completionTokens}");
                    Console.WriteLine($"Prompt tokens: {prompt_tokens}");
                    Console.WriteLine($"Total tokens: {total_tokens}");
                    Console.WriteLine("--------------------");
                }
                else
                {
                    Console.WriteLine("Response data is null.");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            }
        }
    }
}