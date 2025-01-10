
import os  
import base64
import requests
from openai import AzureOpenAI  
from dotenv import load_dotenv

load_dotenv()

api_key = os.getenv("MY_APIM_API_KEY")
apim_endpoint = os.getenv("APIM_ENDPOINT")
openai_deployment_name = os.getenv("OPENAI_DEPLOYMENT_NAME")

base_url = f"https://{apim_endpoint}/openai/deployments/{openai_deployment_name}"
headers = {
    "api-key": api_key
}
params = {
    "api-version": "2024-05-01-preview"
}

question = "What is the capital of France?"

chat_prompt = [
    {
        "role": "system",
        "content": [
            {
                "type": "text",
                "text": "You are an AI assistant that helps people find information."
            }],
        "role": "user",
        "content": [
            {
                "type": "text",
                "text": question
            }
        ]
    }
]

messages = chat_prompt

response = requests.post(
    f"{base_url}/chat/completions",
    headers=headers,
    params=params,
    json={
        "model": openai_deployment_name,
        "messages": messages,
        "max_tokens": 800,
        "temperature": 0.7,
        "top_p": 0.95,
        "frequency_penalty": 0,
        "presence_penalty": 0,
        "stop": None,
        "stream": False
    }
)

print(response.json())