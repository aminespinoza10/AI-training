import os
import openai

openai.api_type = "azure"
openai.api_base = "https://aminesai4.openai.azure.com/"
openai.api_version = "2023-09-15-preview"
openai.api_key = os.getenv("OPENAI_API_KEY")

response = openai.Completion.create(
  engine="aminesTest",
  prompt="# Write a python function to reverse a string. The function should be an optimal solution in terms of time and space complexity.\n# Example input to the function: abcd123\n# Example output to the function: 321dcba",
  temperature=0.2,
  max_tokens=150,
  top_p=1,
  frequency_penalty=0,
  presence_penalty=0,
  stop=["#"])

print(response.choices[0].text.strip(" \n"))